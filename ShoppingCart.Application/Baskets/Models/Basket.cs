using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Baskets.Models
{
    public class Basket
    {
        private const decimal vat = 10; 

        public int BasketId { get; set; }
        public Guid BasketCode { get; set; }
        public string Customer { get; set; }
        public bool PaysVAT { get; set; }
        public bool IsClosed { get; set; }
        public bool IsPayed { get; set; }

        public decimal TotalNet { get { return CalculateTotalNet(); } }
        public decimal TotalGross { get { return CalculateTotalGross(); } }      

        public List<Article> Articles { get; set; }


        public static Basket FromEntity(DataAccess.Entities.Basket basket)
        {
            return new Basket
            {
                BasketId = basket.BasketId,
                BasketCode = basket.BasketCode,
                Customer = basket.Customer,
                PaysVAT = basket.PaysVAT,
                IsClosed = basket.IsClosed,
                IsPayed = basket.IsPayed,

                Articles = basket.Articles?.Select(art => Article.FromEntity(art)).ToList()
            };
        }

        private decimal CalculateTotalNet()
        {
            if(Articles != null)
                return Articles.Sum(art => art.Price);

            return 0;
        }

        private decimal CalculateTotalGross()
        {
            if (PaysVAT)
                return TotalNet + (TotalNet * vat/100);
            return TotalNet;
        }
    }
}
