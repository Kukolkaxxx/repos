using System;
using System.Collections.Generic;
using System.Text;
using test.Models;

namespace test.Data
{
    public static class CreateDatabase
    {
        public static void InitializeDatabase()
        {
            using (var context = new ConncetionDbContext())
            {
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    context.Users.Add(new User
                    {
                        UserName = "admin",
                        Password = "admin123",
                        Role = "admin"
                    });

                    // Примеры книг
                    context.Books.AddRange(
                        new Book { Title = "Война и мир", Author = "Л.Н. Толстой", Year = 1869,  TotalCopies = 5, Publishment = "" },
                        new Book { Title = "Преступление и наказание", Author = "Ф.М. Достоевский", Year = 1866, TotalCopies = 3, Publishment = "" },
                        new Book { Title = "Мастер и Маргарита", Author = "М.А. Булгаков", Year = 1967, TotalCopies = 4, Publishment = "" }
                    );

                    context.SaveChanges();
                }
            }
        }

    }
}
