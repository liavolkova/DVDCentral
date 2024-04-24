using LV.DVDCentral.BL.Models;
using LV.DVDCentral.PL;
using Microsoft.EntityFrameworkCore.Storage;

namespace LV.DVDCentral.BL
{
    public static class MovieGenreManager
    {
        
        public static void Insert(int movieId, int genreId, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                   if(rollback) transaction = dc.Database.BeginTransaction();

                    tblMovieGenre tblMovieGenre = new tblMovieGenre();
                    tblMovieGenre.Id = dc.tblMovieGenres.Any() ? dc.tblMovieGenres.Max(mg => mg.Id) + 1 : 1;

                    tblMovieGenre.GenreId = genreId;
                    tblMovieGenre.MovieId = movieId;
                  
                    dc.tblMovieGenres.Add(tblMovieGenre);
                    results = dc.SaveChanges();

                    if(rollback) transaction?.Rollback();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public static void Update(int movieId, int genreId, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblMovieGenre entity = dc.tblMovieGenres.FirstOrDefault(s => s.MovieId == movieId && s.GenreId == genreId);

                    if (entity != null)
                    {
                        entity.MovieId = movieId;
                        entity.GenreId = genreId;
                        dc.tblMovieGenres.Add(entity);
                        results = dc.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Row does not exist");
                    }

                    if (rollback) transaction.Rollback();
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void Delete(int movieId, int genreId, bool rollback = false)
        {
            try
            {
                //int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblMovieGenre entity = dc.tblMovieGenres.FirstOrDefault(s => s.MovieId == movieId && s.GenreId == genreId);

                    if (entity != null)
                    {
                        dc.tblMovieGenres.Remove(entity);
                        dc.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Row does not exist");
                    }

                    if (rollback) transaction.Rollback();
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
       
    }
}
