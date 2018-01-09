using System;
using System.Collections.Generic;
using System.Text;
using Dot.Kitchen.Ons.Application.Interfaces;
using Dot.Kitchen.Ons.Infrastructure;

namespace Dot.Kitchen.Ons.Application.Commands
{
    public class TriggerScrapeCommand : ITriggerScrapeCommand
    {
        private IScraper _scraper;
        private IScrapeRepository _repository;

        public TriggerScrapeCommand(IScrapeRepository repository, IScraper scraper)
        {
            if (scraper == null)
                throw new ArgumentNullException(nameof(scraper));

            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _scraper = scraper;
        }

        public void Execute()
        {
            var results = _scraper.Execute("surname");
        }
    }
}
