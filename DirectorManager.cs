using LV.DVDCentral.BL.Models;
using LV.DVDCentral.PL;
using Microsoft.EntityFrameworkCore.Storage;

namespace LV.DVDCentral.BL
{
    public static class DirectorManager
    {
        public static int Insert(string firstName,
                                 string lastName,
                                 ref int id,
                                 bool rollback = false)
        {
            try
            {
                Director director = new Director
                {
                    FirstName = firstName,
                    LastName = lastName
                };

                int results = Insert(director, rollback);

                id = director.Id; ;

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int Insert(Director director, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblDirector entity = new tblDirector();

                    entity.Id = dc.tblDirectors.Any() ? dc.tblDirectors.Max(c => c.Id) + 1 : 1;
                    entity.FirstName = director.FirstName;
                    entity.LastName = director.LastName;

                    director.Id = entity.Id;

                    dc.tblDirectors.Add(entity);
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

        public static int Update(Director director, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblDirector entity = dc.tblDirectors.FirstOrDefault(s => s.Id == director.Id);

                    if (entity != null)
                    {
                        entity.FirstName = director.FirstName;
                        entity.LastName = director.LastName;
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

                    tblDirector entity = dc.tblDirectors.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        dc.tblDirectors.Remove(entity);
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
        public static Director LoadById(int id)
        {
            try
            {
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    tblDirector entity = dc.tblDirectors.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        return new Director
                        {
                            Id = entity.Id,
                            FirstName = entity.FirstName,
                            LastName = entity.LastName
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
        public static List<Director> Load()
        {
            try
            {
                List<Director> list = new List<Director>();

                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    (from c in dc.tblDirectors
                     select new
                     {
                         c.Id,
                         c.FirstName,
                         c.LastName
                     })
                     .ToList()
                     .ForEach(director => list.Add(new Director
                     {
                         Id = director.Id,
                         FirstName = director.FirstName,
                         LastName = director.LastName
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
