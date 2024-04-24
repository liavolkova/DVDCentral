using LV.DVDCentral.BL.Models;
using LV.DVDCentral.PL;
using Microsoft.EntityFrameworkCore.Storage;

namespace LV.DVDCentral.BL
{
    public static class OrderManager
    {
        public static int Insert(int customerId,
                                 DateTime orderDate,
                                 int UserId,
                                 DateTime shipDate,
                                 ref int id,
                                 bool rollback = false)
        {
            try
            {
                Order order = new Order
                {
                    CustomerId = customerId, 
                    OrderDate = orderDate,
                    UserId = UserId,
                    ShipDate = shipDate
                };

                int results = Insert(order, rollback);

                id = order.Id; ;

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int Insert(Order order, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    
                    tblOrder entity = new tblOrder();

                    entity.Id = dc.tblOrders.Any() ? dc.tblOrders.Max(c => c.Id) + 1 : 1;
                    entity.CustomerId = order.CustomerId;
                    entity.OrderDate = order.OrderDate;
                    entity.UserId = order.UserId;
                    entity.ShipDate = order.ShipDate;


                    order.Id = entity.Id;

                    dc.tblOrders.Add(entity);
                    results = dc.SaveChanges();

                    if (rollback) transaction.Rollback();

                    if (results > 0 && order.OrderItems != null)
                    {
                        foreach (var OrderItem in order.OrderItems)
                        {
                            OrderItem.OrderId = order.Id;
                            OrderItemManager.Insert(OrderItem, rollback);
                        }
                    }

                }

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int Update(Order order, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblOrder entity = dc.tblOrders.FirstOrDefault(s => s.Id == order.Id);

                    if (entity != null)
                    {
                        entity.CustomerId = order.CustomerId;
                        entity.OrderDate = order.OrderDate;
                        entity.UserId = order.UserId;
                        entity.ShipDate = order.ShipDate;
                        results = dc.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Row does not exist");
                    }

                    if (rollback) transaction.Rollback();
                }
                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static int Delete(int id, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblOrder entity = dc.tblOrders.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        dc.tblOrders.Remove(entity);
                        results = dc.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Row does not exist");
                    }

                    if (rollback) transaction.Rollback();
                }
                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static Order LoadById(int id)
        {
            try
            {
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                   // tblOrder entity = dc.tblOrders.FirstOrDefault(o => o.Id == id);
                    var entity = (from c in dc.tblOrders
                    join dt in dc.tblCustomers on c.CustomerId equals dt.Id
                    where c.CustomerId == id 
                    select new
                    {
                        c.Id,
                        c.CustomerId,
                        c.OrderDate,
                        c.UserId,
                        c.ShipDate,
                        CustomerName = dt.FirstName + " " + dt.LastName

                    })
                    .FirstOrDefault();

                    if (entity != null)
                    {
                        return new Order
                        {
                            Id = entity.Id,
                            CustomerId = entity.CustomerId,
                            OrderDate = entity.OrderDate,
                            UserId = entity.UserId,
                            ShipDate = entity.ShipDate,
                            OrderItems = OrderItemManager.LoadByOrderId(id),
                            CustomerName = entity.CustomerName,
                        };

                    }

                    else
                    {
                        throw new Exception();
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static List<Order> Load(int? customerId = null)
        {
            try
            {
                List<Order> list = new List<Order>();

                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    (from c in dc.tblOrders
                     join oi in dc.tblOrderItems on c.Id equals oi.Id
                     join dt in dc.tblCustomers on c.CustomerId equals dt.Id
                     where c.CustomerId == customerId || customerId == null
                     select new
                     {
                         c.Id,
                         c.CustomerId,
                         c.OrderDate,
                         c.UserId,
                         c.ShipDate,
                         CustomerName = dt.FirstName + " " + dt.LastName

                     })
                     .ToList()
                     .ForEach(order => list.Add(new Order
                     {
                         Id = order.Id,
                         CustomerId = order.CustomerId, 
                         OrderDate = order.OrderDate,
                         UserId = order.UserId,
                         ShipDate = order.ShipDate,
                         CustomerName = order.CustomerName
                     }));
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
