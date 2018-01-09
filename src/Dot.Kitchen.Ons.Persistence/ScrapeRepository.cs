using System;
using System.Linq;
using Dot.Kitchen.Ons.Application;
using Dot.Kitchen.Ons.Domain;
using Dot.Kitchen.Ons.Application.Interfaces;

namespace Dot.Kitchen.Ons.Persistence
{
    public class ScrapeRepository : IScrapeRepository
    {
        private IDatabaseContext _context;

        public ScrapeRepository(IDatabaseContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        public void Add(Scrape entity)
        {
            throw new NotImplementedException();
        }

        public Scrape Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Scrape> GetAll()
        {
            return _context.Scrapes;
        }
    }
}
