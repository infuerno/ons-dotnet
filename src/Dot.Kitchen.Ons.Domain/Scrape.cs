using System;

namespace Dot.Kitchen.Ons.Domain
{
    public class Scrape : IEntity
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int NumberOfRecordsScraped { get; set; }
        public int SourceId { get; set; }
        public Source Source { get; set; }
    }
}
