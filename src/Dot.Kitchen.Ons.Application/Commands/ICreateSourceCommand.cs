using System;
using System.Collections.Generic;
using System.Text;
using Dot.Kitchen.Ons.Application.Models;

namespace Dot.Kitchen.Ons.Application.Commands
{
    public interface ICreateSourceCommand
    {
        void Execute(SourceModel model);
    }
}
