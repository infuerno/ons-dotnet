using Dot.Kitchen.Ons.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;

namespace Dot.Kitchen.Ons.Application.Models
{
    public class SourceModel
    {
        public int Id { get; set; }

        [RegularExpression("[A-Za-z]+", ErrorMessage = "No spaces, numbers or special characters"), Required, StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Friendly Name"), Required, StringLength(100)]
        public string FriendlyName { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
    }
}
