using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.DataAccess.Entities
{
    public class Basket
    {
        public int BasketId { get; set; }
        public string Customer { get; set; }
        public bool PaysVAT { get; set; }
        public bool IsPayed { get; set; }
        public bool IsClosed { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
