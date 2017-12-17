namespace PhotoShare.Client.Core
{
    using System;
    using Commands;
    using PhotoShare.Models;

    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParameters, Session session)
        {
            var command = commandParameters[0].ToLower();
            string result;

            switch (command)
            {
                case "registeruser":
                    result = RegisterUserCommand.Execute(commandParameters, session);
                    break;
                case "addtown":
                    result = AddTownCommand.Execute(commandParameters, session);
                    break;
                case "exit":
                    result = ExitCommand.Execute();
                    break;
                case "deleteuser":
                result = DeleteUser.Execute(commandParameters, session);
                    break;
                case "uploadpicture":
                    result = UploadPictureCommand.Execute(commandParameters, session);
                    break;
                case "acceptfriend":
                    result = AcceptFriendCommand.Execute(commandParameters, session);
                    break;
                case "addfriend":
                    result = AddFriendCommand.Execute(commandParameters, session);
                    break;
                case "createalbum":
                    result = CreateAlbumCommand.Execute(commandParameters, session);
                    break;
                case "listfriends":
                    result = PrintFriendsListCommand.Execute(commandParameters, session);
                    break;
                case "addtag":
                    result = AddTagCommand.Execute(commandParameters, session);
                    break;
                case "modifyuser":
                    result = ModifyUserCommand.Execute(commandParameters, session);
                    break;
                case "addtagto":
                    result = AddTagToCommand.Execute(commandParameters, session);
                    break;
                case "login":
                    result = LoginCommand.Execute(commandParameters, session);
                    break;
                case "logout":
                    result = LogoutCommand.Execute(commandParameters, session);
                    break;
                case "sharealbum":
                    result = ShareAlbumCommand.Execute(commandParameters, session);
                    break;
                default:
                    throw new InvalidOperationException($"Command {command} not valid!");
            }

            return result;
        }
    }
}
