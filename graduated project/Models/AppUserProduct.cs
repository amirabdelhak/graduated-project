﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace graduated_project.Models
{
    [PrimaryKey(nameof(AppUserId), nameof(ProductId))]
    public class AppUserProduct
    {
        [ForeignKey("AppUser") ]
        [Required]
        public string AppUserId { get; set; }
        public virtual AppUser? AppUser { get; set; }
        [ForeignKey("Product")]
        [Required]

        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }

    


        public int? Rate { get; set; }
        public string? Comment { get; set; }
        public DateTime? Date { get; set; }
    }
}
