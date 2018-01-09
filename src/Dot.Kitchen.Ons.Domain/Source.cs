using System;
using System.Collections.Generic;

namespace Dot.Kitchen.Ons.Domain
{
    public class Source : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
    }
}
