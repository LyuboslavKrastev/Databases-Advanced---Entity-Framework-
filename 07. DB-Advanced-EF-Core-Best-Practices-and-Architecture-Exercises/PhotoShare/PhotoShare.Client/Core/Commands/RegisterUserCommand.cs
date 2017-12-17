namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class RegisterUserCommand
    {
        public static string Execute(string[] data, Session session)
        {
            if (session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            string username = data[1];
            string password = data[2];
            string repeatPassword = data[3];
            string email = data[4];

            if (password != repeatPassword)
            {
                throw new ArgumentException("Passwords do not match!");
            }

            using (var context = new PhotoShareContext())
            {
                var existingUser = context.Users.FirstOrDefault(u => u.Username == username);
                if (existingUser != null)
                {
                    throw new InvalidOperationException($"Username {username} is already taken!");
                }
            }

            User user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false,
                RegisteredOn = DateTime.Now,
                LastTimeLoggedIn = DateTime.Now
            };

            using (PhotoShareContext context = new PhotoShareContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return "User " + user.Username + " was registered successfully!";
        }
    }
}
