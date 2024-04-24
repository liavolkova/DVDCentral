using LV.DVDCentral.BL.Models;
using LV.DVDCentral.PL;
using Microsoft.EntityFrameworkCore.Storage;

namespace LV.DVDCentral.BL
{
    public static class CustomerManager
    {
        public static int Insert(string firstName,
                                 string lastName,
                                 int userId,
                                 string address,
                                 string city,
                                 string state,
                                 string zip,
                                 string phone,
                                 ref int id,
                                 bool rollback = false)
        {
            try
            {
                Customer customer = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserId = userId,
                    Address = address,
                    City = city,
                    State = state,
                    ZIP = zip,
                    Phone = phone
                };

                int results = Insert(customer, rollback);

                id = customer.Id; ;

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int Insert(Customer customer, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblCustomer entity = new tblCustomer();

                    entity.Id = dc.tblCustomers.Any() ? dc.tblCustomers.Max(c => c.Id) + 1 : 1;
                    entity.FirstName = customer.FirstName;
                    entity.LastName = customer.LastName;
                    entity.UserId = customer.UserId;
                    entity.Address = customer.Address;
                    entity.City = customer.City;
                    entity.State = customer.State;
                    entity.ZIP = customer.ZIP;
                    entity.Phone = customer.Phone;


                    customer.Id = entity.Id;

                    dc.tblCustomers.Add(entity);
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

        public static int Update(Customer customer, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblCustomer entity = dc.tblCustomers.FirstOrDefault(s => s.Id == customer.Id);

                    if (entity != null)
                    {
                        entity.FirstName = customer.FirstName;
                        entity.LastName = customer.LastName;
                        entity.UserId = customer.UserId;
                        entity.Address = customer.Address;
                        entity.City = customer.City;
                        entity.State = customer.State;
                        entity.ZIP = customer.ZIP;
                        entity.Phone = customer.Phone;
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

                    tblCustomer entity = dc.tblCustomers.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        dc.tblCustomers.Remove(entity);
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
        public static Customer LoadById(int id)
        {
            try
            {
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    tblCustomer entity = dc.tblCustomers.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        return new Customer
                        {
                            Id = entity.Id,
                            FirstName = entity.FirstName,
                            LastName = entity.LastName,
                            UserId = entity.UserId,
                            Address = entity.Address,
                            City = entity.City,
                            State = entity.State,
                            ZIP = entity.ZIP,
                            Phone = entity.Phone
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
        public static List<Customer> Load()
        {
            try
            {
                List<Customer> list = new List<Customer>();

                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    (from c in dc.tblCustomers
                     select new
                     {
                         c.Id,
                         c.FirstName,
                         c.LastName,
                         c.UserId,
                         c.Address,
                         c.City,
                         c.State,
                         c.ZIP,
                         c.Phone
                     })
                     .ToList()
                     .ForEach(customer => list.Add(new Customer
                     {
                         Id = customer.Id,
                         FirstName = customer.FirstName,
                         LastName = customer.LastName,
                         UserId = customer.UserId,
                         Address = customer.Address,
                         City = customer.City,
                         State = customer.State,
                         ZIP = customer.ZIP,
                         Phone = customer.Phone
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
