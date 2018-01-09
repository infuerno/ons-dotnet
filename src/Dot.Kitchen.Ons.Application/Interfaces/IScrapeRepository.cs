using System;
using System.Collections.Generic;
using System.Linq;
using Dot.Kitchen.Ons.Domain;

namespace Dot.Kitchen.Ons.Application.Interfaces
{
    public interface IScrapeRepository
    {
        IQueryable<Scrape> GetAll();

        Scrape Get(int id);

        void Add(Scrape entity);

        //void Remove(Scrape entity);
    }
}
