using Dot.Kitchen.Ons.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dot.Kitchen.Ons.Infrastructure.Gro
{
    public class GroBirthScrapeItem : ScrapeItem
    {
        public string MothersMaidenSurname { get; set; }

        public string BirthYear { get; set; }
        public BirthQuarterType BirthQuarter { get; set; }
        public string District { get; set; }
        public string Volume { get; set; }
        public string Page { get; set; }
    }

    public enum BirthQuarterType
    {
        M,
        J,
        S,
        D
    }
}
