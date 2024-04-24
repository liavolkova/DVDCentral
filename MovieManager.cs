using LV.DVDCentral.BL.Models;
using LV.DVDCentral.PL;
using Microsoft.EntityFrameworkCore.Storage;

namespace LV.DVDCentral.BL
{
    public static class MovieManager
    {
        public static int Insert(
                                 string title,
                                 string description,
                                 int formatId,
                                 int directorId,
                                 int ratingId,
                                 double cost,
                                 int inStkQty,
                                 string imagePath,
                                 ref int id,
                                 bool rollback = false)
        {
            try
            {
                Movie movie = new Movie
                {
                    Title = title,
                    Description = description,
                    FormatId = formatId,
                    DirectorId = directorId,
                    RatingId = ratingId,
                    Cost = cost,
                    InStkQty = inStkQty,
                    ImagePath = imagePath
                };

                int results = Insert(movie, rollback);

                id = movie.Id; ;

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int Insert(Movie movie, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblMovie entity = new tblMovie();

                    entity.Id = dc.tblMovies.Any() ? dc.tblMovies.Max(c => c.Id) + 1 : 1;
                    entity.Title = movie.Title;
                    entity.Description = movie.Description;
                    entity.FormatId = movie.FormatId;
                    entity.DirectorId = movie.DirectorId;
                    entity.RatingId = movie.RatingId;
                    entity.Cost = movie.Cost;
                    entity.InStkQty = movie.InStkQty;
                    entity.ImagePath = movie.ImagePath;


                    movie.Id = entity.Id;

                    dc.tblMovies.Add(entity);
                    results = dc.SaveChanges();

                    if (rollback) transaction.Rollback();
                    return results;
                }

                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int Update(Movie movie, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblMovie entity = dc.tblMovies.FirstOrDefault(s => s.Id == movie.Id);

                    if (entity != null)
                    {
                        entity.Title = movie.Title;
                        entity.Description = movie.Description;
                        entity.FormatId = movie.FormatId;
                        entity.DirectorId = movie.DirectorId;
                        entity.RatingId = movie.RatingId;
                        entity.Cost = movie.Cost;
                        entity.InStkQty = movie.InStkQty;
                        entity.ImagePath = movie.ImagePath;


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

                    tblMovie entity = dc.tblMovies.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        dc.tblMovies.Remove(entity);
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
        public static Movie LoadById(int id)
        {
            try
            {
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    var entity = (from m in dc.tblMovies
                                  join r in dc.tblRatings on m.RatingId equals r.Id
                                  join f in dc.tblFormats on m.FormatId equals f.Id
                                  join mg in dc.tblMovieGenres on m.Id equals mg.MovieId
                                  join g in dc.tblGenres on mg.GenreId equals g.Id
                                  join d in dc.tblDirectors on m.DirectorId equals d.Id
                                  where m.Id == id 
                                  select new
                                  {
                                      m.Id,
                                      m.Title,
                                      m.Description,
                                      m.Cost,
                                      m.InStkQty,
                                      m.ImagePath,
                                      GenreName = g.Description,
                                      FormatName = f.Description,
                                      RatingName = r.Description,
                                      FullName = d.FirstName + " " + d.LastName,
                                  })
                      .FirstOrDefault();

                    if (entity != null)
                    {
                        return new Movie
                        {
                            Id = entity.Id,
                            Title = entity.Title,
                            Description = entity.Description,
                            FormatName = entity.FormatName,
                            FullName = entity.FullName,
                            RatingName = entity.RatingName,
                            Cost = entity.Cost,
                            InStkQty = entity.InStkQty,
                            ImagePath = entity.ImagePath,
                            //GenreName = entity.GenreName,
                            GenreList = GenreManager.Load(entity.Id)
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
        public static List<Movie> Load(int? genreId = null)
        {
            try
            {
                List<Movie> list = new List<Movie>();

                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                   (from m in dc.tblMovies
                     join r in dc.tblRatings on m.RatingId equals r.Id
                     join f in dc.tblFormats on m.FormatId equals f.Id
                     //join mg in dc.tblMovieGenres on m.Id equals mg.MovieId
                    // join g in dc.tblGenres on mg.GenreId equals g.Id
                     join d in dc.tblDirectors on m.DirectorId equals d.Id
                     where m.Id == genreId || genreId == null
                     select new
                     {
                         MovieId = m.Id,
                         FormatId = m.FormatId,
                         RatingId = m.RatingId,
                         DirectorId = m.DirectorId,
                         Title = m.Title,
                         Description = m.Description,
                         Cost = m.Cost,
                         InStkQty = m.InStkQty,
                         ImagePath = m.ImagePath,
                         //GenreName = g.Description,
                         FormatName = f.Description,
                         RatingName = r.Description,
                         FullName = d.FirstName + " " + d.LastName,
                     })
                     .Distinct()
                     .ToList()
                     .ForEach(movie => list.Add(new Movie
                     {
                         Id = movie.MovieId,
                         Title = movie.Title,
                         Description = movie.Description,
                         Cost = movie.Cost,
                         //GenreName = movie.GenreName,
                         FormatId = movie.FormatId,
                         FormatName = movie.FormatName,
                         RatingName = movie.RatingName,
                         RatingId = movie.RatingId,
                         DirectorId=movie.DirectorId,
                         InStkQty = movie.InStkQty,
                         ImagePath = movie.ImagePath,
                         FullName = movie.FullName
                     }));
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
         public static List<Movie> LoadByGenreId(int genreId)
        {
            return Load(genreId);
        }
    }
}
