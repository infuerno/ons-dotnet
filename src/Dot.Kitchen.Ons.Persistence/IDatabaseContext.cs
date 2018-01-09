using System;
using Dot.Kitchen.Ons.Domain;
using Microsoft.EntityFrameworkCore;


namespace Dot.Kitchen.Ons.Persistence
{
    public interface IDatabaseContext
    {
        DbSet<ScrapeItem> ScrapeItems { get; set; }
        DbSet<Scrape> Scrapes { get; set; }
        DbSet<Source> Sources { get; set; }
        DbSet<T> Set<T>() where T : class, IEntity;
        void Save();
    }
}
