using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazine.Core.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public float Price { get; set; } = 0;
        public string Image { get; set; } = string.Empty; // Ссылка на изображение товара
    }
}
