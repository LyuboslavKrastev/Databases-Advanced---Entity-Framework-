namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PrintFriendsListCommand 
    {
        // PrintFriendsList <username>
        public static string Execute(string[] data, Session session)
        {
            var username = data[1];
            StringBuilder result = new StringBuilder();
            using (var context = new PhotoShareContext())
            {
                if (!context.Users.Any(u=>u.Username == username))
                {
                    throw new ArgumentException($"User {username} not found!");
                }
                var chckUser = context.Users
                    .Include(u => u.FriendsAdded)
                    .ThenInclude(af => af.Friend)
                    .FirstOrDefault(u => u.Username == username);
                if (!chckUser.FriendsAdded.Any())
                {
                    throw new ArgumentException("No friends for this user. :(");
                }
                result.AppendLine("Friends:");
                foreach (var friend in chckUser.FriendsAdded)
                {
                    var currFriend = friend.Friend.Username;
                    result.AppendLine($"-{currFriend}");
                }
            }
            return result.ToString().Trim();
        }
    }
}
