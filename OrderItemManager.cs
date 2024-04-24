using LV.DVDCentral.BL.Models;
using LV.DVDCentral.PL;
using Microsoft.EntityFrameworkCore.Storage;

namespace LV.DVDCentral.BL
{
    public static class OrderItemManager
    {
        public static int Insert(int orderId,
                                 int quantity,
                                 int movieId,
                                 float cost,
                                 ref int id,
                                 bool rollback = false)
        {
            try
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = orderId,
                    Quantity = quantity,
                    MovieId = movieId,
                    Cost = cost
                };

                int results = Insert(orderItem, rollback);

                id = orderItem.Id; ;

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int Insert(OrderItem orderItem, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblOrderItem entity = new tblOrderItem();

                    entity.Id = dc.tblOrderItems.Any() ? dc.tblOrderItems.Max(c => c.Id) + 1 : 1;
                    entity.OrderId = orderItem.OrderId;
                    entity.Quantity = orderItem.Quantity;
                    entity.MovieId = orderItem.MovieId;
                    entity.Cost = orderItem.Cost;

                    orderItem.Id = entity.Id;

                    dc.tblOrderItems.Add(entity);
                    results = dc.SaveChanges();

                    if (rollback) transaction.Rollback();

                }

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int Update(OrderItem orderItem, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblOrderItem entity = dc.tblOrderItems.FirstOrDefault(s => s.Id == orderItem.Id);

                    if (entity != null)
                    {
                        entity.OrderId = orderItem.OrderId;
                        entity.Quantity = orderItem.Quantity;
                        entity.MovieId = orderItem.MovieId;
                        entity.Cost = orderItem.Cost;
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

                    tblOrderItem entity = dc.tblOrderItems.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        dc.tblOrderItems.Remove(entity);
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
        public static OrderItem LoadById(int id)
        {
            try
            {
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    tblOrderItem entity = dc.tblOrderItems.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        return new OrderItem
                        {
                            Id = entity.Id,
                            OrderId = entity.OrderId,
                            Quantity = entity.Quantity,
                            MovieId = entity.MovieId,
                            Cost = entity.Cost

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
        public static List<OrderItem> Load()
        {
            try
            {
                List<OrderItem> list = new List<OrderItem>();

                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    (from c in dc.tblOrderItems
                     select new
                     {
                         c.Id,
                         c.OrderId,
                         c.Quantity,
                         c.MovieId,
                         c.Cost
                     })
                     .ToList()
                     .ForEach(orderItem => list.Add(new OrderItem
                     {
                         Id = orderItem.Id,
                         OrderId = orderItem.OrderId, 
                         Quantity = orderItem.Quantity,
                         MovieId = orderItem.MovieId,
                         Cost = orderItem.Cost
                     }));
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static List <OrderItem> LoadByOrderId (int orderId)
        {
            try
            {
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    List<OrderItem> orderItems = new List<OrderItem>();

                    var entities = dc.tblOrderItems.Where(item => item.OrderId == orderId).ToList();

                    foreach (var entity in entities)
                    {
                        orderItems.Add(new OrderItem
                        {
                            Id = entity.Id,
                            OrderId = entity.OrderId,
                            Quantity = entity.Quantity,
                            MovieId = entity.MovieId,
                            Cost = entity.Cost
                        });
                    }

                    return orderItems;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
