using Dot.Kitchen.Ons.Application.Interfaces;
using Dot.Kitchen.Ons.Application.Models;
using System;

namespace Dot.Kitchen.Ons.Application.Queries
{
    public class GetScrapeDetailQuery : IGetScrapeDetailQuery
    {
        private readonly IScrapeRepository _repository;

        public GetScrapeDetailQuery(IScrapeRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            _repository = repository;
        }

        public ScrapeDetailModel Execute(int id)
        {
            throw new NotImplementedException();
        }
    }
}
