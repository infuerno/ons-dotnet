using System;

namespace Dot.Kitchen.Ons.Application.Models
{
    public class ScrapeModel
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string SourceName { get; set; }
        public DateTime StartedAt { get; set; }
        public TimeSpan? TimeTaken { get; set; }
        public int NumberOfRecordsScraped { get; set; }
    }
}
