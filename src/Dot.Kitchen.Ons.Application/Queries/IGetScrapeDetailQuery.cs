using Dot.Kitchen.Ons.Application.Models;
using System;

namespace Dot.Kitchen.Ons.Application.Queries
{
    public interface IGetScrapeDetailQuery
    {
        ScrapeDetailModel Execute(int id);
    }
}
