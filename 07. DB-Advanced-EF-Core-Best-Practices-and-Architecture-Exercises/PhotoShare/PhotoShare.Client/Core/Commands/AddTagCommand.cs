namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using Data;
    using Utilities;
    using System.Linq;
    using System;

    public class AddTagCommand
    {
        // AddTag <tag>
        public static string Execute(string[] data, Session session)
        {
            if (!session.IsLoggedIn())
            {
                throw new InvalidOperationException("Invalid credentials!");
            }
            string tag = data[1];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                if (context.Tags.Any(t=>t.Name == tag))
                {
                    throw new ArgumentException($"Tag {tag} exists!");
                }
                tag = tag.ValidateOrTransform();
                context.Tags.Add(new Tag
                {
                    Name = tag
                });

                context.SaveChanges();
            }

            return tag + " was added successfully to database!";
        }
    }
}
