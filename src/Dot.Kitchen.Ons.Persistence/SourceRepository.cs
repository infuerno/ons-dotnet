using System;
using System.Linq;
using Dot.Kitchen.Ons.Application;
using Dot.Kitchen.Ons.Domain;
using Dot.Kitchen.Ons.Application.Interfaces;

namespace Dot.Kitchen.Ons.Persistence
{
    public class SourceRepository : ISourceRepository
    {
        private IDatabaseContext _context;

        public SourceRepository(IDatabaseContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        public void Add(Source entity)
        {
            _context.Set<Source>().Add(entity);
            _context.Save();
        }

        public Source Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Source> GetAll()
        {
            return _context.Sources;
        }
    }
}
