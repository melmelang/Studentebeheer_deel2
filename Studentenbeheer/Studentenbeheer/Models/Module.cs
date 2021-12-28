﻿using System.ComponentModel.DataAnnotations;

namespace Studentenbeheer.Models
{
    public class Module
    {
        public int Id { get; set; }
        [Required]
        public string? Naam { get; set; }
        [Required]
        public string? Omschrijving { get; set; }
        public DateTime? Deleted { get; set; } = DateTime.MaxValue;

    }
}
