using System;
using System.Collections.Generic;
using System.Text;
using PhotoShare.Data;
using System.Linq;
using PhotoShare.Models;

namespace PhotoShare.Client.Core.Commands
{
    public class LogoutCommand
    {

        public static string Execute(string[] data, Session session)
        {
            var user = string.Empty; ;
            using (var context = new PhotoShareContext())
            {
                if (!session.IsLoggedIn())
                {
                    throw new ArgumentException("You should log in first in order to logout.");
                }
                 user = session.User.Username;
                session.Logout();
            }
            return  $"User {user} successfully logged out!";
        }
    }
}
