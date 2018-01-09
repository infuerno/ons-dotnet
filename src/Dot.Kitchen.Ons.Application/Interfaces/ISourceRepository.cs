using System;
using System.Collections.Generic;
using System.Linq;
using Dot.Kitchen.Ons.Domain;

namespace Dot.Kitchen.Ons.Application.Interfaces
{
    public interface ISourceRepository
    {
        IQueryable<Source> GetAll();

        Source Get(int id);

        void Add(Source entity);

        //void Remove(Scrape entity);
    }
}
