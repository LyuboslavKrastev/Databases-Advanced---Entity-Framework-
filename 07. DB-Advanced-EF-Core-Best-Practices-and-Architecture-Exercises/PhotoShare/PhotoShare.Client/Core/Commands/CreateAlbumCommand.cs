namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utilities;

    public class CreateAlbumCommand
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public static string Execute(string[] data, Session session)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            var username = data[1];
            var albumtitle = data[2];
            if (username != session.User.Username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            Color bgColor = new Color();
            bool chckColor = Enum
                .TryParse(data[3].First().ToString().ToUpper() + data[3].Substring(1), out bgColor);
            if (chckColor == false)
            {
                throw new ArgumentException($"Color {data[3]} not found!");
            }
            string[] tags = data.Skip(3).ToArray();
            using (var context = new PhotoShareContext())
            {
                var tagsToAdd = tags.Select(t => t.ValidateOrTransform()).ToArray();
                var dbTags = context.Tags.Select(t=>t.Name).ToArray();
                bool testTags = tagsToAdd.Intersect(dbTags).Any();
                
                if (testTags==false)
                {
                    throw new ArgumentException("Invalid tags!");
                }
                if (!context.Users.Any(u=>u.Username==username))
                {
                    throw new ArgumentException($"User {username} not found!");
                }
                if (context.Albums.Any(a=>a.Name == albumtitle))
                {
                    throw new ArgumentException($"Album {albumtitle} already exists!");
                }
               

                var albTags = new List<AlbumTag>();
                foreach (var t in tagsToAdd)
                {
                    Tag tagg = new Tag
                    {
                        Name = t
                    };
                    var currAlbumT = new AlbumTag
                    {
                        Tag = tagg,
                        Album = new Album
                        {
                            Name = albumtitle
                        }

                    };
                    
                    albTags.Add(currAlbumT);
                }
                var user = context.Users.Where(u => u.Username == username).SingleOrDefault();
                Album newAlbum = new Album
                {
                    Name = albumtitle,
                    BackgroundColor = bgColor,
                    AlbumTags = albTags,
                    AlbumRoles = new List<AlbumRole>()
                    {
                        new AlbumRole()
                        {
                            User = user,
                            Role = Role.Owner
                        }
                    }

                };
                context.Albums.Add(newAlbum);
                context.SaveChanges();
            }
            return $"Album {albumtitle} successfully created!";
            //var tags = 
        }
    }
}
