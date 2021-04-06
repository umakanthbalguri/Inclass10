using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InClass10Assignment
{
    public class order_header
    {
        public string ID { get; set; }
        public DateTime order_date { get; set; }
        public string store { get; set; }
        public string customer_id { get; set; }
        public double order_total { get; set; }
        public ICollection<order_items> order_items_link { get; set; }
    }

    public class product
    {
        public string ID { get; set; }
        public string product_name { get; set; }
        public string catogery { get; set; }
        public double price { get; set; }
        public ICollection<order_items> order_items_link { get; set; }
    }

    public class order_items
    {
        public string ID { get; set; }
        public order_header order_header { get; set; }
        public product product { get; set; }
        public int quantity { get; set; }
        public double running_total { get; set; }
    }

    class ApplicationDbContext : DbContext
    {
        public DbSet<product> product { get; set; }
        public DbSet<order_header> order_header { get; set; }
        public DbSet<order_items> order_items { get; set; }
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=InClass10;Trusted_Connection=True;MultipleActiveResultSets=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString); 
        }
    }
    class MainProgram
    {
        static void Main(string[] args)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Database.EnsureCreated();
                product[] plist = new product[]
               {
                    new product{ID="001",product_name="mnm candy",catogery="grocerry",price=45.00},
                    new product{ID="002",product_name="doormat",catogery="Home Furnishing",price=8.00},
                    new product{ID="003",product_name="Organic Avacado",catogery="grocerry",price=4.30},
                    new product{ID="004",product_name="Kettle Chips",catogery="grocerry",price=3.68},
                    new product{ID="005",product_name="Heniken Ultra",catogery="Beer & Wine",price=17.54},
                    new product{ID="006",product_name=" GreatValue Grated Cheese",catogery="grocerry",price=3.25}
               };

                order_header[] olist = new order_header[]
                {
                    new order_header{ID="001",order_date=DateTime.Parse("2021-04-19"),store="flecherAve", customer_id="IS112", order_total=21.4},
                    new order_header{ID="002",order_date=DateTime.Parse("2021-04-19"),store="flecherAve", customer_id="IS112", order_total=40.4},
                    new order_header{ID="003",order_date=DateTime.Parse("2021-04-20"),store="flecherAve", customer_id="IS113", order_total=23.4},
                    new order_header{ID="004",order_date=DateTime.Parse("2021-04-20"),store="flecherAve", customer_id="IS113", order_total=76.4},
                    new order_header{ID="005",order_date=DateTime.Parse("2021-04-21"),store="flecherAve", customer_id="IS113", order_total=34.4}
                };

                order_items[] dlist = new order_items[]
                {
                    new order_items{ID="01001",order_header=olist[0],product=plist[0],quantity=2,running_total=3.45},
                    new order_items{ID="01003",order_header=olist[0],product=plist[2],quantity=1, running_total=3.45},
                    new order_items{ID="01004",order_header=olist[0],product=plist[3],quantity=5, running_total=3.45},
                    new order_items{ID="02001",order_header=olist[1],product=plist[0],quantity=2, running_total=3.45},
                    new order_items{ID="02005",order_header=olist[1],product=plist[4],quantity=1, running_total=3.45},
                    new order_items{ID="03002",order_header=olist[3],product=plist[3],quantity=4, running_total=3.45},
                    new order_items{ID="03004",order_header=olist[2],product=plist[3],quantity=1, running_total=3.45},
                    new order_items{ID="03005",order_header=olist[2],product=plist[4],quantity=5, running_total=3.45},
                    new order_items{ID="04001",order_header=olist[3],product=plist[2],quantity=1, running_total=3.45},
                    new order_items{ID="04002",order_header=olist[2],product=plist[1],quantity=4, running_total=3.45},
                    new order_items{ID="04003",order_header=olist[3],product=plist[2],quantity=3, running_total=3.45},
                    new order_items{ID="04004",order_header=olist[1],product=plist[3],quantity=4, running_total=3.45},
                    new order_items{ID="04005",order_header=olist[1],product=plist[4],quantity=8, running_total=3.45},
                };
                if (!context.order_header.Any())
                {
                    foreach (order_header o in olist)
                    {
                        context.order_header.Add(o);
                    }
                    context.SaveChanges();
                }

                if (!context.product.Any())
                {
                    foreach (product p in plist)
                    {
                        context.product.Add(p);
                    }
                    context.SaveChanges();
                }

                if (!context.order_items.Any())
                {
                    foreach (order_items d in dlist)
                    {
                        context.order_items.Add(d);
                    }
                    context.SaveChanges();
                }

                var a = context.order_header
                    .Include(c => c.order_items_link)
                    .Where(c => c.order_items_link.Count != 0);
                Console.WriteLine("**********List of products Sold **********");
                foreach (var i in a)
                {
                    Console.WriteLine("OrderID={0},OrderDate={1},store={2},CustomerName={3}", i.ID, i.order_date,i.store, i.customer_id);
                }

                order_header output = context.order_items
                    .Where(c => c.product.product_name == "doormat")
                    .OrderByDescending(c => c.quantity)
                    .Select(c => c.order_header)
                    .First();
                Console.WriteLine("**********Order with Maximum of Doormats Sold **********");
                Console.WriteLine("OrderID={0},OrderDate={1},CustomerName={2},store={3}", output.ID, output.order_date, output.customer_id,output.store);
            }
        }
    }
}