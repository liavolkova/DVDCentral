using LV.DVDCentral.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LV.DVDCentral.BL
{
    public class ShoppingCartManager
    {
        public static void Add(ShoppingCart cart, Movie movie)
        {
            if (cart != null) { cart.Items.Add(movie); }
        }

        public static void Remove(ShoppingCart cart, Movie movie)
        {
            if (cart != null) { cart.Items.Remove(movie); }
        }

        public static void Checkout(ShoppingCart cart)
        {
            if (cart == null || cart.Items.Count == 0)
            {
                return; 
            }
            // Make a new order
            // Set the Order fields as needed.
            Order order = new Order
            {
                CustomerId = 1,
                UserId = 1,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now.AddDays(3)
            };
           

            ////create a list to store orderItems for batch insert
            List<OrderItem> orderItems = new List<OrderItem>();

            // foreach(Movie item in cart.Items)
            // Make a new orderitem
            // Set the OrderItem fields from the item.
            foreach (Movie item in cart.Items)
            {
                OrderItem orderItem = new OrderItem
                {
                    MovieId = item.Id,
                    Quantity = 1,
                    Cost = item.Cost
                };
                // order.OrderItems.Add(orderItem)
                order.OrderItems.Add(orderItem);

                // Decrement the tblMovie.InStkQty appropriately.
                //item.InStkQty -= 1;

            }

            OrderManager.Insert(order);
            

            //empty the cart
            cart.Items.Clear();

        }
    }
}
