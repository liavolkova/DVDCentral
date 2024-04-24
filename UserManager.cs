using LV.DVDCentral.BL.Models;
using LV.DVDCentral.PL;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Cryptography;
using System.Text;

namespace LV.DVDCentral.BL
{
    public static class UserManager
    {
        //public static int Insert(
        //                         string firstName,
        //                         string lastName,
        //                         string userId,
        //                         string password,
        //                         ref int id,
        //                         bool rollback = false)
        //{
        //    try
        //    {
        //        User user = new User
        //        {
        //            FirstName = firstName,
        //            LastName = lastName,
        //            UserId = userId,
        //            Password = password
        //        };

        //        int results = Insert(user, rollback);

        //        id = user.Id; ;

        //        return results;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public static int Insert(User user, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblUser entity = new tblUser();

                    entity.Id = dc.tblUsers.Any() ? dc.tblUsers.Max(c => c.Id) + 1 : 1;
                   
                    entity.FirstName = user.FirstName;
                    entity.LastName = user.LastName;
                    entity.UserId = user.UserId;
                    entity.Password = GetHash(user.Password);

                    user.Id = entity.Id;

                    dc.tblUsers.Add(entity);
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

        public static int Update(User user, bool rollback = false)
        {
            try
            {
                int results = 0;
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    IDbContextTransaction transaction = null;
                    if (rollback) transaction = dc.Database.BeginTransaction();

                    tblUser entity = dc.tblUsers.FirstOrDefault(s => s.Id == user.Id);

                    if (entity != null)
                    {
                        entity.FirstName = user.FirstName;
                        entity.LastName = user.LastName;
                        entity.UserId = user.UserId;
                        entity.Password = user.Password;
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
        //public static int Delete(int id, bool rollback = false)
        //{
        //    try
        //    {
        //        int results = 0;
        //        using (DVDCentralEntities dc = new DVDCentralEntities())
        //        {
        //            IDbContextTransaction transaction = null;
        //            if (rollback) transaction = dc.Database.BeginTransaction();

        //            tblUser entity = dc.tblUsers.FirstOrDefault(s => s.Id == id);

        //            if (entity != null)
        //            {
        //                dc.tblUsers.Remove(entity);
        //                results = dc.SaveChanges();
        //            }
        //            else
        //            {
        //                throw new Exception("Row does not exist");
        //            }

        //            if (rollback) transaction.Rollback();
        //        }
        //        return results;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        public static User LoadById(int id)
        {
            try
            {
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    tblUser entity = dc.tblUsers.FirstOrDefault(s => s.Id == id);

                    if (entity != null)
                    {
                        return new User
                        {
                            Id = entity.Id,
                            FirstName = entity.FirstName,
                            LastName = entity.LastName,
                            UserId = entity.UserId,
                            Password = entity.Password
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
        public static List<User> Load()
        {
            try
            {
                List<User> list = new List<User>();

                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    (from c in dc.tblUsers
                     select new
                     {
                         c.Id,
                         c.FirstName, 
                         c.LastName,
                         c.UserId,
                         c.Password
                     })
                     .ToList()
                     .ForEach(user => list.Add(new User
                     {
                         Id = user.Id,
                         FirstName = user.FirstName, 
                         LastName = user.LastName,
                         UserId = user.UserId,
                         Password = user.Password
                     }));
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void Seed()
        {
            using (DVDCentralEntities dc = new DVDCentralEntities())
            {

                if (!dc.tblUsers.Any())
                {
                    User user = new User
                    {
                        UserId = "lvolkova",
                        FirstName = "Liana",
                        LastName = "Volkova",
                        Password = "testtest"
                    };
                    Insert(user);

                    user = new User
                    {
                        UserId = "bfoote",
                        FirstName = "Brian",
                        LastName = "Foote",
                        Password = "maple"
                    };
                    Insert(user);
                }
            }
        }
        public class LoginFailureException : Exception
        {
            public LoginFailureException() : base("Cannot log in with these credentials.  You IP Address has been saved.")
            {

            }
            public LoginFailureException(string message) : base(message)
            {

            }

        }
        public static string GetHash(string password)
        {
            using (var hasher = SHA1.Create())
            {
                var hashbytes = Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(hasher.ComputeHash(hashbytes));
            }
        }

        public static int DeleteAll()
        {
            try
            {
                using (DVDCentralEntities dc = new DVDCentralEntities())
                {
                    dc.tblUsers.RemoveRange(dc.tblUsers.ToList());
                    return dc.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool Login(User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.UserId))
                {
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        using (DVDCentralEntities dc = new DVDCentralEntities())
                        {
                            tblUser tblUser = dc.tblUsers.FirstOrDefault(u => u.UserId == user.UserId);
                            if (tblUser != null)
                            {
                                if (tblUser.Password == GetHash(user.Password))
                                {
                                    // Login successful
                                    user.Id = tblUser.Id;
                                    user.FirstName = tblUser.FirstName;
                                    user.LastName = tblUser.LastName;
                                    return true;
                                }
                                else
                                {
                                    throw new LoginFailureException();
                                }
                            }
                            else
                            {
                                throw new LoginFailureException("UserId was not found.");
                            }
                        }
                    }
                    else
                    {
                        throw new LoginFailureException("Password was not set.");
                    }
                }
                else
                {
                    throw new LoginFailureException("UserId was not set.");
                }
            }
            catch (LoginFailureException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
