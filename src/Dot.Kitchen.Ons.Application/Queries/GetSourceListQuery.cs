using System;
using System.Collections.Generic;
using System.Text;
using Dot.Kitchen.Ons.Application.Models;
using Dot.Kitchen.Ons.Application.Interfaces;
using System.Linq;
using Dot.Kitchen.Ons.Domain;

namespace Dot.Kitchen.Ons.Application.Queries
{
    public class GetSourceListQuery : IGetSourceListQuery
    {
        private readonly ISourceRepository _repository;

        public GetSourceListQuery(ISourceRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            _repository = repository;
        }

        public List<SourceModel> Execute()
        {
            var sources = _repository.GetAll()
                .Select(s => new SourceModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    FriendlyName = s.FriendlyName,
                    Description = s.Description
                });

            return sources.ToList();
        }
    }
}
