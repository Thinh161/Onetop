using Microsoft.AspNetCore.Mvc;
using OneTop.Models;

namespace OneTop.ViewModels
{
    public class ProductManagerVM
    {
        public List<Product> Products { get; set; }
        public List<Product> Categories { get; set; }
    }
}

