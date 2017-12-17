namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class AcceptFriendCommand
    {
        public static string Execute(string[] data, Session session)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            var firstUser = data[1];
            var secondUser = data[2];
            using (var context = new PhotoShareContext())
            {
                var chkFirstUser = context.Users
                    .Include(u => u.FriendsAdded)
                    .ThenInclude(af => af.Friend)
                    .FirstOrDefault(u => u.Username == firstUser);
                var chkSecondUser = context.Users
                    .Include(u => u.FriendsAdded)
                    .ThenInclude(af => af.Friend)
                    .FirstOrDefault(u => u.Username == secondUser);

                if (chkFirstUser == null)
                {
                    throw new ArgumentException($"{firstUser} not found!");
                }
                if (chkSecondUser == null)
                {
                    throw new ArgumentException($"{secondUser} not found!");
                }

                bool alreadyAdded = chkFirstUser.FriendsAdded.Any(u => u.Friend == chkSecondUser);
                bool alreadyAccepted = chkSecondUser.FriendsAdded.Any(u => u.Friend == chkFirstUser);

                if (alreadyAdded && alreadyAccepted)
                {
                    throw new InvalidOperationException
                        ($"{firstUser} is already friend to {secondUser}.");
                }
                bool secalrdyAdded = chkSecondUser.FriendsAdded.Any(u => u.Friend == chkFirstUser);

                if (!secalrdyAdded)
                {
                    throw new InvalidOperationException($"{secondUser} has not added {firstUser} as a friend");
                }
                chkFirstUser.FriendsAdded.Add(new Friendship
                {
                    User = chkFirstUser,
                    Friend = chkSecondUser
                });
                context.SaveChanges();
            }
            return $"{firstUser} accepted {secondUser} as a friend";
        }
    }
}
