namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Models;
    using System.Linq;
    using PhotoShare.Data;

    public class ShareAlbumCommand
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public static string Execute(string[] data, Session session)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            var albumid = int.Parse(data[1]);
            var albumName = string.Empty;
            var username = (data[2]);
            if (username != session.User.Username)
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            Role Role = new Role();
            var chckRole = Enum
                .TryParse(data[3].First().ToString().ToUpper() + data[3].Substring(1), out Role);
            if (chckRole == false)
            {
                throw new ArgumentException($"Permission must be either “Owner” or “Viewer”!");
            }
            using (var context = new PhotoShareContext())
            {
                if (!context.Users.Any(u=>u.Username==username))
                {
                    throw new ArgumentException($"User {username} not found!");
                }
                if (!context.Albums.Any(a=>a.Id == albumid))
                {
                    throw new ArgumentException($"Album {albumid} not found!");
                }
                var album = context.Albums.Where(a => a.Id == albumid).SingleOrDefault();
                var user = context.Users.Where(u => u.Username == username).SingleOrDefault();
                var albumId = album.Id;
                AlbumRole currAR = new AlbumRole
                {
                    Album = album,
                    User = user,
                    Role = Role

                };
                album.AlbumRoles.Add(currAR);
                albumName = album.Name;
                context.SaveChanges();
            }
            return $"Username {username} added to album {albumName} ({Role.ToString()})";
            
        }
    }
}
