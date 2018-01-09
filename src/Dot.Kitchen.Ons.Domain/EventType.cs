using System;
using System.Collections.Generic;

namespace Dot.Kitchen.Ons.Domain
{
    public abstract class EventDate : IEqualityComparer<EventDate>
    {
        public int Year { get; set; }

        public bool Equals(EventDate x, EventDate y)
        {
            // TODO need to be able to compare dates of different types
            return false;
        }

        public int GetHashCode(EventDate obj)
        {
            return obj.GetHashCode();
        }
    }

    public class SimpleDate : EventDate, IEqualityComparer<SimpleDate>
    {
        public int? Month { get; set; }
        public int? Day { get; set; }

        public bool Equals(SimpleDate x, SimpleDate y)
        {
            if (x.Year != y.Year)
                return false;
            if (x.Month != y.Month)
                return false;
            return x.Day == y.Day;
        }

        public int GetHashCode(SimpleDate obj)
        {
            return obj.GetHashCode();
        }
    }

    public class QuarterDate : EventDate, IEqualityComparer<QuarterDate>
    {
        private int _quarter;

        public int Quarter
        {
            get { return _quarter; }
            set
            {
                if (value < 1 || value > 4)
                    throw new ArgumentException(nameof(value));
                _quarter = value;
            }
        }

        public QuarterType QuarterLetter
        {
            get { return (QuarterType)_quarter; }
            set { _quarter = (int)value; }
        }

        public bool Equals(QuarterDate x, QuarterDate y)
        {
            if (x.Year != y.Year)
                return false;
            return x.Quarter == y.Quarter;
        }

        public int GetHashCode(QuarterDate obj)
        {
            throw new NotImplementedException();
        }
    }

    public enum QuarterType
    {
        M = 1,
        J = 2,
        S = 3,
        D = 4
    }
}
