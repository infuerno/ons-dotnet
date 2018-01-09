using Dot.Kitchen.Ons.Infrastructure.Gro;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace Dot.Kitchen.Ons.Infrastructure.Tests
{
    public class GroScraperTests
    {
        IConfiguration Configuration { get; set; }

        private string _username;
        private string _password;

        public GroScraperTests()
        {
            // the type specified here is just so the secrets library can 
            // find the UserSecretId we added in the csproj file
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<GroScraperTests>();

            Configuration = builder.Build();

            _username = Configuration["GroUsername"];
            _password = Configuration["GroPassword"];
        }

        [Fact]
        public void NullUsernameThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new GroBirthScraper(null, _password));
        }

        [Fact]
        public void NullPasswordThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new GroBirthScraper(_username, null));
        }

        [Fact]
        public void SurnameWithNoEntriesReturnsEmptyResultSet()
        {
            var scraper = new GroBirthScraper(_username, _password);
            var results = scraper.Execute("Furneyx");
            Assert.Empty(results.ScrapeItems);
        }

        [Fact]
        public void SurnameWithSeveralEntriesReturnsResults()
        {
            var scraper = new GroBirthScraper(_username, _password);
            var results = scraper.Execute("Furney");
        }

        [Fact]
        public void SurnameWithEntriesOnMoreThanOnePageReturnsResults()
        {
            var scraper = new GroBirthScraper(_username, _password);
            var results = scraper.Execute("Smith");
        }
    }
}
