using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.DataAccess.Context
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options) { }

        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<Event> Events { get; set; }
    }
}
