using System;

namespace Dot.Kitchen.Ons.Domain
{
    public class ScrapeItem
    {
        public int Id { get; set; }
        public string UniqueIdFromSource { get; set; }
        public EventType Event { get; set; }
        public string Surname { get; set; }
        public string Forenames { get; set; }
    }

    public enum EventType
    {
        Birth, Marriage, Death, Census
    }

}
