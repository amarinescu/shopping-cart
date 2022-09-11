using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.DataAccess.Entities
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Item { get; set; }
        public decimal Price { get; set; }
    }
}
