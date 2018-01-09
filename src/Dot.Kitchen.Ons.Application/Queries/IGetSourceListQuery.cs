using Dot.Kitchen.Ons.Application.Models;
using System;
using System.Collections.Generic;

namespace Dot.Kitchen.Ons.Application.Queries
{
    public interface IGetSourceListQuery
    {
        List<SourceModel> Execute();
    }
}
