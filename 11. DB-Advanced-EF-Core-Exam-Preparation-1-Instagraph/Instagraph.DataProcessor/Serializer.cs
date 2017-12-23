using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using AutoMapper.QueryableExtensions;

using Instagraph.Data;
using Instagraph.DataProcessor.DtoModels;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Xml;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.Posts
                .Where(p => p.Comments.Count == 0)
                .OrderBy(p => p.Id)
                .ProjectTo<UncommentedPostDto>()
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(posts, Newtonsoft.Json.Formatting.Indented);

            return jsonString;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context.Users
                .Where(u => u.Posts
                    .Any(p => p.Comments
                        .Any(c => u.Followers
                            .Any(f => f.FollowerId == c.UserId))))
                .OrderBy(u => u.Id)
                .ProjectTo<PopularUserDto>()
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);

            return jsonString;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context.Users
                .Select(u => new
                {
                    Username = u.Username,
                    PostsCommentCount = u.Posts.Select(p => p.Comments.Count).ToArray()
                });

            var userDtos = new List<UserTopPostDto>();

            var xDoc = new XDocument();
            xDoc.Add(new XElement("users"));

            foreach (var u in users)
            {
                int mostComments = 0;
                if (u.PostsCommentCount.Any())
                {
                    mostComments = u.PostsCommentCount.OrderByDescending(c => c).First();
                }

                var userDto = new UserTopPostDto()
                {
                    Username = u.Username,
                    MostComments = mostComments
                };

                userDtos.Add(userDto);
            }

            userDtos = userDtos.OrderByDescending(u => u.MostComments)
                .ThenBy(u => u.Username).ToList();

            var sb = new StringBuilder();

            var serializer = new XmlSerializer(typeof(List<UserTopPostDto>), new XmlRootAttribute("users"));
            serializer.Serialize(new StringWriter(sb), userDtos, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            var result = sb.ToString();
            return result;
        }
    }
}
