namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class ModifyUserCommand
    {
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public static string Execute(string[] data, Session session)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            string username = data[1];
            string property = data[2].ToLower();
            string newValue = data[3];

            using (var context = new PhotoShareContext())
            {
                var user = context.Users.Where(u => u.Username == username).SingleOrDefault();
                if (user == null)
                {
                    throw new ArgumentException($"User {username} not found!");
                }
                switch (property)
                {
                    case "password":
                        if (!newValue.Any(c=> char.IsLower(c)) || !newValue.Any(c => char.IsDigit(c)))
                        {

                            throw new
                                ArgumentException($"Value {newValue} not valid!" +
                                Environment.NewLine + "Invalid Password!"); 
                        }
                        user.Password = newValue;
                        break;
                    case "borntown":
                        var bornTown = context.Towns.Where(t => t.Name == newValue).SingleOrDefault();
                        if (bornTown == null)
                        {
                            throw new
                            ArgumentException($"Value {newValue} not valid!" +
                       Environment.NewLine + $"Town {newValue} not found!"); 
                        }
                       
                        user.BornTown = bornTown;
                        break;
                    case "currenttown":
                        var currentTown = context.Towns.Where(t => t.Name == newValue).SingleOrDefault();
                        if (currentTown == null)
                        {
                            throw new
                            ArgumentException($"Value {newValue} not valid!" +
                       Environment.NewLine + $"Town {newValue} not found!");
                        }

                        user.CurrentTown = currentTown;
                        break;

                    default:
                        throw new ArgumentException($"Property {property} not supported!");
                }
                context.SaveChanges();
                return $"{username} {property} is {newValue}"; 

            }
        }
    }
}
