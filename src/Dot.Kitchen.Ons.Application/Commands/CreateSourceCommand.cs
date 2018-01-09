using System;
using System.Collections.Generic;
using System.Text;
using Dot.Kitchen.Ons.Application.Interfaces;
using Dot.Kitchen.Ons.Application.Models;
using Dot.Kitchen.Ons.Domain;

namespace Dot.Kitchen.Ons.Application.Commands
{
    public class CreateSourceCommand : ICreateSourceCommand
    {
        private ISourceRepository _repository;
        public CreateSourceCommand(ISourceRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(_repository));
            _repository = repository;
        }

        public void Execute(SourceModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            _repository.Add(new Source()
            {
                Name = model.Name,
                FriendlyName = model.FriendlyName,
                Description = model.Description
            });
        }
    }
}
