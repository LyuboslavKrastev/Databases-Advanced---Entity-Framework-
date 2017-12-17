namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using Utilities;

    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public class AddTagToCommand 
    {
        // AddTagTo <albumName> <tag>
        public static string Execute(string[] data, Session session)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            var albumName = data[1];
            var tag = data[2];

            using (var context = new PhotoShareContext())
            {
                bool noSuchTag = !context.Tags.Any(t => t.Name == tag);
                bool noSuchAlbum = !context.Albums.Any(a => a.Name == albumName);
                if (noSuchAlbum || noSuchTag)
                {
                    throw new ArgumentException("Either tag or album do not exist!");
                }
             
                var currAlbum = context.Albums.Where(a => a.Name == albumName)
                   .Include(a => a.AlbumRoles)
                   .ThenInclude(ar => ar.User).SingleOrDefault();
                
                bool isUserOwner = currAlbum.AlbumRoles
                .Any(ar => ar.Role == Role.Owner && ar.User.Username == session.User.Username);

                if (!isUserOwner)
                {
                    throw new InvalidOperationException("Invalid credentials!");
                }
                var currTag = context.Tags.Where(a => a.Name == tag).SingleOrDefault();
                var albTag = new AlbumTag
                {
                    Tag = currTag,
                    Album = currAlbum
                };
                currAlbum.AlbumTags.Add(albTag);
                context.SaveChanges();
            }
            return $"Tag {tag} added to {albumName}!";
        }
    }
}
