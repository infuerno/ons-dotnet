using System;
using System.Collections.Generic;
using System.Text;

namespace Dot.Kitchen.Ons.Infrastructure.Gro
{
    public class GroBirthScraper : GroScraper
    {
        public GroBirthScraper(string username, string password) : base(username, password)
        {

        }

        protected override string SearchIndexTypeFieldId { get => "EW_Birth";}

        protected override List<GroSearchParameters> GetSearches()
        {
            var searches = new List<GroSearchParameters>();
            // index starts in 1837, so search 1842 with a range of 5 years (1837-1847 inc), then 1853 with range of 5 years (1848-1858 inc) etc
            // 1908 with a range of 5 years will be the last full 11 year range search (1903-1913 inc)
            for (int year = 1842; year <= 1908; year += 11)
            {
                searches.Add(new GroSearchParameters() { Year = year, Range = 5 });
            }
            // finally search for the last 3 years, 1914-1916 inc
            searches.Add(new GroSearchParameters() { Year = 1915, Range = 1 });
            return searches;
        }
    }
}
