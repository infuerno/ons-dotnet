using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using Dot.Kitchen.Ons.Domain;

namespace Dot.Kitchen.Ons.Infrastructure.Gro
{
    public abstract class GroScraper : IScraper
    {
        //private const string BaseAddress = "https://www.gro.gov.uk";
        //private const int BirthIndexStartYear = 1837;
        //private const int BirthIndexEndYear = 1916;
        //private const int DeathIndexStartYear = 1837;
        //private const int DeathIndexEndYear = 1957;
        //private const int MaxYearSearchRange = 5;

        private const string LoginPage = "https://www.gro.gov.uk/gro/content/certificates/login.asp";
        private readonly string _username;
        private readonly string _password;
        

        public GroScraper(string username, string password)
        {
            _username = username ?? throw new ArgumentNullException(nameof(username));
            _password = password ?? throw new ArgumentNullException(nameof(password));
        }

        protected abstract List<GroSearchParameters> GetSearches();
        protected abstract string SearchIndexTypeFieldId { get; }

        public ScrapeResults Execute(string surname)
        {
            return ExecuteAsync(surname).Result;
        }

        public async Task<ScrapeResults> ExecuteAsync(string surname)
        {
            var context = SetupAngleSharpContext();
            await Login(context);
            await NavigateToSearchPage(context);

            var results = new ScrapeResults();
            results.ScrapeItems = new List<ScrapeItem>();
            results.ScrapeItems.AddRange(Scrape(context, surname).Result);
            return results;
        }

        protected class GroSearchParameters
        {
            public int Year;
            public int Range;
        }

        private async Task<IEnumerable<ScrapeItem>> Scrape(IBrowsingContext context, string surname)
        {
            await context.Active
                .QuerySelector<IHtmlFormElement>("form")
                .SubmitAsync(new
                {
                    index = SearchIndexTypeFieldId
                });

            var searches = GetSearches();
            var entries = new List<ScrapeItem>();
            foreach (var search in searches)
            {
                foreach (var gender in new ArrayList() {"F", "M"})
                {
                    //Thread.Sleep(1000); // sleep 1 second between searches
                    var form = context.Active.QuerySelector<IHtmlFormElement>("form");

                    var optionForFiveYearRange = context.Active.CreateElement<IHtmlOptionElement>();
                    optionForFiveYearRange.Value = "5";
                    form.QuerySelector<IHtmlSelectElement>("select#Range").AddOption(optionForFiveYearRange);

                    await form.QuerySelectorAll<IHtmlInputElement>("input.formButton").Single(b => b.Value == "Search")
                        .SubmitAsync(new
                        {
                            Surname = surname,
                            Gender = gender,
                            search.Year,
                            search.Range
                        });

                    var resultsRows = GetResultsRows(context);

                    var resultsStats = GetResultsStats(resultsRows);
                    var resultsCount = resultsStats.Item1;
                    var currentPage = resultsStats.Item2;
                    var pageCount = resultsStats.Item3;
                    if (resultsCount == 0)
                        continue;

                    entries.AddRange(ParseResults(resultsRows));

                    // get the next page if multiple pages
                    while (currentPage < pageCount)
                    {
                        //puts 'More pages, getting next one...'
                        //Thread.Sleep(1000); // sleep 1 second between searches
                        await context.Active.QuerySelector<IHtmlFormElement>("form")
                            .QuerySelectorAll<IHtmlInputElement>("input.formButton").Single(b => b.Value == "Next Page")
                            .SubmitAsync();

                        resultsRows = GetResultsRows(context);

                        resultsStats = GetResultsStats(resultsRows);
                        resultsCount = resultsStats.Item1;
                        currentPage = resultsStats.Item2;
                        pageCount = resultsStats.Item3;

                        entries.AddRange(ParseResults(resultsRows));
                    }
                }
            }
            return entries;
        }

        private IHtmlCollection<IElement> GetResultsRows(IBrowsingContext context)
        {
            var resultsTable = context.Active.QuerySelectorAll("form table table table")[3];
            var resultsRows = resultsTable.QuerySelectorAll("tr");
            var heading = resultsRows[0].QuerySelector("h3").Text();
            if (!heading.StartsWith("Results:"))
            {
                throw new ApplicationException(
                    "No results table found, or header was not 'Results:' - check markup, NOT formatted as expected");
            }
            return resultsRows;
        }

        private Tuple<int, int, int> GetResultsStats(IHtmlCollection<IElement> resultsRows)
        {
            var numberOfResultsInfo = resultsRows[resultsRows.Length - 1].Text().Trim();
            int resultsCount, currentPage, pageCount;
            if (numberOfResultsInfo == "No Matching Results Found")
            {
                //"No matching results found, continuing with next search in batch"
                //continue;
            }

            var pattern = @"(\d+) Record\(s\) Found - Showing Page (\d+) of (\d+)Go to page";
            var matches = Regex.Match(numberOfResultsInfo, pattern);
            if (matches.Groups.Count != 4)
                throw new ApplicationException($"Trying to parse number of results from string '{numberOfResultsInfo}' using pattern '{pattern}', expecting 4 group matches, only {matches.Groups.Count} found.");
            resultsCount = int.Parse(matches.Groups[1].Value);
            currentPage = int.Parse(matches.Groups[2].Value);
            pageCount = int.Parse(matches.Groups[3].Value);

            return new Tuple<int, int, int>(resultsCount, currentPage, pageCount);
        }

        private IBrowsingContext SetupAngleSharpContext()
        {
            var config = Configuration.Default.WithDefaultLoader().WithCookies();
            var context = BrowsingContext.New(config);
            return context;
        }

        private async Task Login(IBrowsingContext context)
        {
            var loginPage = LoginPage;
            await context.OpenAsync(loginPage);

            await context.Active
                .QuerySelector<IHtmlFormElement>("form")
                .QuerySelectorAll<IHtmlInputElement>("input.formButton").Single(b => b.Value == "Submit")
                .SubmitAsync(new Dictionary<string, string>()
                {
                    {"username", _username},
                    {"password", _password}
                });
        }

        private async Task NavigateToSearchPage(IBrowsingContext context)
        {
            var result =
                context.Active.Links.Single(a => a.TextContent == "Search the GRO Indexes") as IHtmlAnchorElement;
            await result.NavigateAsync();
        }

        private IEnumerable<ScrapeItem> ParseResults(IHtmlCollection<IElement> resultsRows)
        {
            const int NumberOfHeaderRows = 2;
            const int NumberOfFooterRows = 4;
            const int NumberOfRowsPerItem = 2;

            var scrapeItems = new List<ScrapeItem>();
            //RemoveHeaderAndFooterRows(resultsRows);

            // there are 2 rows for each result
            //Array(0...results_rows.length).select {|i| i % 2 == 0 }.each do |i| // e.g for 3 results will give array 2,4,6
            for (int i = NumberOfHeaderRows; i < resultsRows.Length - NumberOfFooterRows; i += NumberOfRowsPerItem)
            {
                //var countInfoOutputSize = 20;
                //if (i % countInfoOutputSize == 0)
                //{
                //    var startEntryIndex = (i / 2) + 1;
                //    var endEntryIndex = ((i + Math.Min(resultsRows.Length - 2, countInfoOutputSize - 2)) / 2) + 1;
                    //"Parsing results for entries: #{start_entry_index} to #{end_entry_index} of #{results_rows.length/2}" # rows 0,1 are for entry 1, rows 2,3 for entry 2, rows 4,5 for entry 3 etc
                //}
                
                var tds = resultsRows[i].QuerySelectorAll("td");

                var nameArray = tds[0].TextContent.Replace("\n", "").Replace("\t", "").Trim().Split(',');
                var surname = nameArray[0].Trim();
                var forenames = nameArray[1].Trim();

                var mothersMaidenName = tds[1].TextContent.Replace("\n", "").Replace("\t", "").Trim();
                var orderLink = tds[2].QuerySelector("a[href]") as IHtmlAnchorElement;
                var orderUrl = orderLink?.Href;
                var entryId = orderUrl?.Split("EntryID=")[1];

                var groReference = resultsRows[i + 1].TextContent.Replace("\n", "").Replace("\t", "").Trim();
                var pattern = @"GRO Reference:(\d+).([MJSD]).Quarterin([A-Z\-\.\'\&, ]+).Volume.(\w+).Page.(\w+)";
                var matches = Regex.Match(groReference, pattern);
                if (matches.Groups.Count != 6)
                    throw new ApplicationException($"Trying to parse GRO reference information from string '{groReference}' using pattern '{pattern}', expecting 6 group matches, only {matches.Groups.Count} found.");
                var year = matches.Groups[1].Value.Trim();
                var quarter = matches.Groups[2].Value.Trim();
                var district = matches.Groups[3].Value.Trim();
                var volume = matches.Groups[4].Value.Trim();
                var page = matches.Groups[5].Value.Trim();

                var item = new GroBirthScrapeItem()
                {
                    Event = EventType.Birth,
                    UniqueIdFromSource = entryId,
                    Surname = surname,
                    Forenames = forenames,
                    MothersMaidenSurname = mothersMaidenName,
                    BirthYear = year,
                    BirthQuarter = (BirthQuarterType)Enum.Parse(typeof(BirthQuarterType), quarter),
                    District = district,
                    Volume = volume,
                    Page = page
                };
                scrapeItems.Add(item);
            }
            return scrapeItems;
        }

    }
}