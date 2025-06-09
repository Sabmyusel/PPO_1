using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Magazine.Core.Models
{
    [Table("Products")]
    [Index(nameof(Id))]
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public float Price { get; set; } = 0;
        public string Image { get; set; } = string.Empty;
        public int Count { get; set; } = 0;
    }
}
