using LV.DVDCentral.BL.Models;
using LV.DVDCentral.PL;
using Microsoft.EntityFrameworkCore.Storage;

namespace LV.DVDCentral.BL
{
    public static class GenreManager
    {
        public static int Insert(string description,
                                 ref int id,
                                 bool rollback = false)
        {
            try
            {
                Genre genre = new Genre
                {
                    Description = description
                };

                int results = Insert(genre, rollback);

                id = genre.Id; ;

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int Insert(Genre genre, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblGenre entity = new tblGenre();

                    entity.Id = dc.tblGenres.Any() ? dc.tblGenres.Max(c => c.Id) + 1 : 1;
                    entity.Description = genre.Description;

                    genre.Id = entity.Id;

                    dc.tblGenres.Add(entity);
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

        public static int Update(Genre genre, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblGenre entity = dc.tblGenres.FirstOrDefault(s => s.Id == genre.Id);

                    if (entity != null)
                    {
                        entity.Description = genre.Description;
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

                    tblGenre entity = dc.tblGenres.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        dc.tblGenres.Remove(entity);
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
        public static Genre LoadById(int id)
        {
            try
            {
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    tblGenre entity = dc.tblGenres.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        return new Genre
                        {
                            Id = entity.Id,
                            Description = entity.Description
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
        public static List<Genre> Load(int movieId)
        {
            try
            {
                List<Genre> list = new List<Genre>();

                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    (from g in dc.tblGenres
                     join mg in dc.tblMovieGenres on g.Id equals mg.GenreId
                     where mg.MovieId == movieId
                     select new
                     {
                         g.Id,
                         g.Description
                     })
                     .ToList()
                     .ForEach(genre => list.Add(new Genre
                     {
                         Id = genre.Id,
                         Description = genre.Description
                     }));
                    return list;
                }

               
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<Genre> Load()
        {
            try
            {
                List<Genre> list = new List<Genre>();

                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    (from c in dc.tblGenres
                     select new
                     {
                         c.Id,
                         c.Description
                     })
                     .ToList()
                     .ForEach(genre => list.Add(new Genre
                     {
                         Id = genre.Id,
                         Description = genre.Description
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
