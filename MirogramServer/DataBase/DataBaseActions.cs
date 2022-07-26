using MirogramServer.Network.RequestPackets;
using MirogramServer.Network.TransferMiniClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.DataBase
{
    public static class DataBaseActions
    {
        public static ServerDataEntities ServerEntity { get; set; } = new ServerDataEntities();


        public static List<Messages> GetMessages(string username, string userNumber, string targetUsername, string targetNumber)
        {
            var messages = from message in ServerEntity.Messages where
                           (message.FromUserID == username && message.FromNumber == userNumber && message.ToUserID == targetUsername && message.ToNumber == targetNumber) ||
                           (message.FromUserID == targetUsername && message.FromNumber == targetNumber && message.ToUserID == username && message.ToNumber == userNumber)
                           select message;

            var list =  messages.ToList();

            return list;

           
        }
        public static void AddMessage(string username, string userNumber, string targetUsername, string targetNumber, string content, string time)
        {
            ServerEntity.Messages.Add(new Messages()
            {
                FromUserID = username,
                FromNumber = userNumber,
                ToUserID = targetUsername,
                ToNumber = targetNumber,
                Content = content,
                TimeSent = time,
            });

            ServerEntity.SaveChanges();
        }


        public static void StartChatWith(string username, string userNumber, string targetUsername, string targetNumber)
        {
            if(!UserHasChatWithNumber(username , userNumber, targetNumber))
            {
                ServerEntity.ChatLists.Add(new ChatLists()
                {
                    UserName = username,
                    Number = userNumber,
                    WithUser = targetUsername,
                    WithNumber = targetNumber
                });

                ServerEntity.ChatLists.Add(new ChatLists()
                {
                    UserName = targetUsername,
                    Number = targetNumber,
                    WithUser = username,
                    WithNumber = userNumber
                });
                ServerEntity.SaveChanges();
            }
        }

        public static bool UserHasChatWithNumber(string username, string userNumber, string targetNumber)
        {
            return (from chat in ServerEntity.ChatLists where chat.UserName == username && chat.Number == userNumber && chat.WithNumber == targetNumber select chat).Count() > 0;
        }

        public static List<string> GetFriendsOfUser(string user)
        {
            return (from u in ServerEntity.Users where u.Username == user select u).Single().Friends.Split(',').ToList();
        }

        public static List<string> GetNumbersOfUser(string user)
        {
            return (from u in ServerEntity.Users where u.Username == user select u).Single().Phones.Split(',').ToList();
        }

        public static List<UserAndPhone> GetListOfChats(string username, string number)
        {
            var query = from chat in ServerEntity.ChatLists where chat.UserName == username select chat;
            var listOfChats = new List<UserAndPhone>();
            foreach (var chat in query.ToList())
            {
                listOfChats.Add(new UserAndPhone(chat.WithUser, chat.WithNumber));
            }

            

            return listOfChats;
        }

        public static void DeleteFriendRequest(string user1, string user2)
        {
            var x = ServerEntity.FriendRequests.Where(req => req.FromUser == user1 && req.ToUser == user2).Single();
            ServerEntity.FriendRequests.Remove(x);
            ServerEntity.SaveChanges();
        }

        public static void SubmitFriendship(string user1, string user2)
        {
            var selectedUser1 = ServerEntity.Users.Where(user => user.Username == user1).Single();
            var selectedUser2 = ServerEntity.Users.Where(user => user.Username == user2).Single();
            selectedUser1.Friends = selectedUser1.Friends.AddElementToString(user2);
            selectedUser2.Friends = selectedUser2.Friends.AddElementToString(user1);
            ServerEntity.SaveChanges();
        }

        public static bool UserValidiation(string username, string password, string number)
        {
            var queryResult = ServerEntity.Users.Where(user => user.Username == username && user.Password == password);
            if (queryResult.Count() > 0 && queryResult.Single().Phones.ElementHasItem(number)) return true;
            return false;

        }

        public static bool RegisterUser(string username, string password, string phoneNumbers)
        {
            bool usernameTaken = ServerEntity.Users.Where(user => user.Username == username).Count() > 0;

            if (usernameTaken) return false;

            ServerEntity.Users.Add(new Users() { Username = username, Password = password, Phones = phoneNumbers,LastOnline = DateTime.Now.ToShortDateString(),Friends = "" });
            ServerEntity.SaveChanges();
            return true;
        }

        public static bool IsFriendWith(string user1, string user2)
        {
            try
            {
                return (from users in ServerEntity.Users where (users.Username == user1) && users.Friends.Split(',').Contains(user2) select users).Count() > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool AddFriendRequest(string fromUser, string token, bool isUserID = true)
        {
            // Token is PhoneNumber or UserName
            try
            {
                // Convert phonenumber to UserName
                if (!isUserID)
                    token = (from user in ServerEntity.Users where user.Phones.Split(',').Contains(token) select user.Username).Single();

                if(!IsFriendWith(fromUser, token))
                {
                    var friendRequest = new FriendRequests()
                    {
                        FromUser = fromUser,
                        ToUser = token
                    };
                    ServerEntity.FriendRequests.Add(friendRequest);
                    ServerEntity.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(">> Server Friend Request Error " + ex.Message);
                return false;
            }
  
        }

        public static List<string> GetFriendRequestsOfUser(string username)
        {
            var requests = from request in ServerEntity.FriendRequests where (request.ToUser == username) select request.FromUser;
            return requests.ToList();
        }


    }
}
