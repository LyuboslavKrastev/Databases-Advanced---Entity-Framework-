namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using System;
    using System.Linq;
    using Models;

    public class UploadPictureCommand
    {
        public static string Execute(string[] data, Session session)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            string albumname = data[1];
            string picturetitle = data[2];
            string picturefilepath = data[3];

            using (var context = new PhotoShareContext())
            {
                var chkAlbum = context.Albums.Where(a => a.Name == albumname).SingleOrDefault();
                if (chkAlbum == null)
                {
                    throw new ArgumentException($"Album {albumname} not found!");
                }
                bool isUserOwner = chkAlbum.AlbumRoles
                .Any(ar => ar.Role == Role.Owner && ar.User.Username == session.User.Username);

                if (!isUserOwner)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }
                Picture pic = new Picture
                {
                   Title = picturetitle,
                   Path = picturefilepath
                };

                chkAlbum.Pictures.Add(pic);
                context.SaveChanges();
            }
            return $"Picture {picturetitle} added to {albumname}!";
        }
    }
}
