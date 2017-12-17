namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class AddFriendCommand
    {
        public static string Execute(string[] data, Session session)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            var requesterName = data[1];
            var friendToAddName = data[2];


            using (var context = new PhotoShareContext())
            {
                var requestingUsr = context.Users
                    .Include(u => u.FriendsAdded)
                    .ThenInclude(af => af.Friend)
                    .FirstOrDefault(u => u.Username == requesterName);
                if (requestingUsr == null )
                {
                    throw new ArgumentException($"{requesterName} not found!");
                }

                var acceptingUsr = context.Users
                    .Include(u=>u.FriendsAdded)
                    .ThenInclude(af=>af.Friend)
                    .FirstOrDefault(u => u.Username == friendToAddName);
                if (acceptingUsr == null)
                {
                    throw new ArgumentException($"{friendToAddName} not found!");
                }

                bool alreadyAdded = requestingUsr.FriendsAdded.Any(u => u.Friend == acceptingUsr);
                bool alreadyAccepted = acceptingUsr.FriendsAdded.Any(u => u.Friend == requestingUsr);

                if (alreadyAdded && !alreadyAccepted) //added
                {
                    throw new ArgumentException("Friend request has already been sent");
                }
                if (alreadyAdded && alreadyAccepted)
                {
                    throw new ArgumentException
                        ($"{friendToAddName} is already friend to {requesterName}.");
                }

                if (!alreadyAdded && alreadyAccepted)
                {
                    throw new ArgumentException
                     ($"{friendToAddName} has already sent {requesterName} a friend request.");
                }

                requestingUsr.FriendsAdded.Add(new Friendship
                {
                    User = requestingUsr,
                    Friend = acceptingUsr
                });

                context.SaveChanges();
            }
         

            return $"Friend r {friendToAddName} added to {requesterName}";

        }
    }
}
