using System;
using System.Collections.Generic;
using System.Text;
using PhotoShare.Data;
using System.Linq;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class LoginCommand
    {

        public static string Execute(string[] data, Session session)
        {
            if (session.IsLoggedIn())
            {
                return $"You should logout first!";
            }
            var username = data[1];
            var password = data[2];

            using (var context = new PhotoShareContext())
            {
                var user = context.Users.Where(u => u.Username == username).SingleOrDefault();
                var chckPass = context.Users.Where(u => u.Username == username).Select(p => p.Password).SingleOrDefault();
                if (user == null || password !=chckPass)
                {
                    throw new ArgumentException("Invalid username or password!");
                }
                session.Login(user);

            }
            return $"User {username} successfully logged in!";
        }
    }
}
