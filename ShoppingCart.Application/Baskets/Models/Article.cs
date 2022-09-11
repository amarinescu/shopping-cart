using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.Baskets.Models
{
    public class Article
    {
        public string Item { get; set; }
        public decimal Price { get; set; }

        public static Article FromEntity(ShoppingCart.DataAccess.Entities.Article article)
        {
            return new Article
            {
                Item = article.Item,
                Price = article.Price
            };
        }
    }
}
