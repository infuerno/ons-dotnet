using Dot.Kitchen.Ons.Application.Interfaces;
using Dot.Kitchen.Ons.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dot.Kitchen.Ons.Application.Queries
{
    public class GetScrapeListQuery : IGetScrapeListQuery
    {
        private readonly IScrapeRepository _repository;

        public GetScrapeListQuery(IScrapeRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;            
        }

        public List<ScrapeModel> Execute()
        {
            var scrapes = _repository.GetAll()
                .Select(s => new ScrapeModel()
                {
                    Id = s.Id,  
                    Surname = s.Surname,
                    SourceName = s.Source.FriendlyName,
                    StartedAt = s.StartedAt,
                    //TimeTaken = s.FinishedAt.HasValue ? s.FinishedAt - s.StartedAt : null,
                    NumberOfRecordsScraped = s.NumberOfRecordsScraped
                });

            // return a list rather than an IEnumerable so the controller can deal with list semantics
            // and .... (what is the other reason)
            return scrapes.ToList();
        }
    }
}
