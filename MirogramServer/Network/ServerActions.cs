using MirogramServer.DataBase;
using MirogramServer.Network.RequestPackets;
using MirogramServer.Network.TransferMiniClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MirogramServer.Network
{
    public static class ServerActions
    {

        public static void HandlePacket(Packet request)
        {
            if (request.Type == Packet.PacketKinds.LoginRequest)
            {
                ServerActions.CheckUserLoginAttemp(request);
            }
            else if (request.Type == Packet.PacketKinds.RegisterRequest)
            {
                ServerActions.RegisterUser(request);
            }
            else if (request.Type == Packet.PacketKinds.AddFriendByNumberRequest)
            {
                ServerActions.AddByNumberRequest(request);
            }
            else if (request.Type == Packet.PacketKinds.AddFriendByIDRequest)
            {
                ServerActions.AddByIDRequest(request);
            }
            else if (request.Type == Packet.PacketKinds.GetFriendRequestsToMeRequest)
            {
                ServerActions.GiveMeMyFriendRequests(request);
            }
            else if (request.Type == Packet.PacketKinds.FriendAcceptDeclineRequest)
            {
                ServerActions.FriendAcceptOrDecline(request);
            }
            else if (request.Type == Packet.PacketKinds.GetFriendsRequest)
            {
                ServerActions.SendFriendsTo(request);
            }
            else if(request.Type == Packet.PacketKinds.GetListOfChatsRequest)
            {
                ServerActions.SendListOfChatsTo(request);
            }
            else if (request.Type == Packet.PacketKinds.StartChatRequest)
            {
                ServerActions.StartChat(request);
            }
            else if (request.Type == Packet.PacketKinds.GetMessagesRequest)
            {
                ServerActions.GetMessages(request);
            }
            else if (request.Type == Packet.PacketKinds.MessageRequest)
            {
                ServerActions.SendMessage(request);
            }



        }

        public static void SendMessage(Packet packet)
        {
            var data = packet as MessageRequestPacket;
            DataBaseActions.AddMessage(data.FromUserID, data.FromNumber, data.ToUserId, data.ToNumber, data.Content,data.TimeSent);

            Server.NetManager.SendPacket(Server.FindSocketByName(data.ToUserId), packet);
        }

        public static void GetMessages(Packet packet)
        {
            var data = packet as GetMessagesRequest;
            var msgs = DataBaseActions.GetMessages(data.FromUserID, data.FromNumber, data.ToUserId, data.ToNumber);

            var messageList = new List<TransferMiniClasses.Message>();

            foreach(var item in msgs.ToList())
            {
                messageList.Add(new Message()
                {
                    Content = item.Content,
                    FromNumber = item.FromNumber,
                    FromUserID = item.FromUserID,
                    TimeSent = item.TimeSent,
                    ToNumber = item.ToNumber,
                    ToUserID = item.ToUserID
                });
            }

            var response = new ServerMessagesList()
            {
                Messages = messageList
            };

            Server.NetManager.SendPacket(Server.FindSocketByName(data.FromUserID), response);
        }

        public static void StartChat(Packet packet)
        {
            var data = packet as StartChatRequest;
            DataBaseActions.StartChatWith(data.UserName, data.Number, data.WithUser, data.WithNumber);
        }

        public static void SendListOfChatsTo(Packet packet)
        {
            var data = packet as GetListOfChatsRequest;

            var usersAndPhones = DataBaseActions.GetListOfChats(data.UserName, data.Number);

            var response = new ServerChatsList();
            response.Items = usersAndPhones;

            Server.NetManager.SendPacket(Server.FindSocketByName(data.UserName), response);
        }

        public static void SendFriendsTo(Packet packet)
        {
            var data = packet as GetFriendsRequest;

            var friends = DataBaseActions.GetFriendsOfUser(data.UserName);

            var userAndPhones = new List<UserAndPhones>();

            foreach (var friend in friends) userAndPhones.Add(new UserAndPhones(friend, DataBaseActions.GetNumbersOfUser(friend)));

            var response = new ServerFriendList();
            response.Friends = userAndPhones;

            Server.NetManager.SendPacket(Server.FindSocketByName(data.UserName), response);
        }

        public static void FriendAcceptOrDecline(Packet packet)
        {
            var data = packet as FriendAcceptDeclineRequest;
            DataBaseActions.DeleteFriendRequest(data.RequestedUserID, data.UserName);

            if (data.Accepted)
            {
                DataBaseActions.SubmitFriendship(data.UserName, data.RequestedUserID);
            }
        }

        public static void CheckUserLoginAttemp(Packet request)
        {
            var data = request as LoginRequestPacket;
            var username = data.UserName;
            var password = data.Password;
            var number = data.PhoneNumber;
            bool attempValid = DataBaseActions.UserValidiation(username, password, number);

            var responsePacket = new ServerStatusPacket();
            responsePacket.ResponseTo = ServerStatusPacket.ResponseTos.Login;

            if (attempValid)
            {
                responsePacket.ServerResonseStatus = ServerStatusPacket.Status.Successful;
                Server.NetManager.SendPacket(Server.FindSocketByName(username), responsePacket);
            }
            else
            {
                responsePacket.ServerResonseStatus = ServerStatusPacket.Status.Failed;
                Server.NetManager.SendPacket(Server.FindSocketByName(username), responsePacket);
            }
        }

        public static void RegisterUser(Packet request)
        {
            var data = request as RegisterRequestPacket;
            var username = data.UserName;
            var password = data.Password;
            var phoneNumbers = data.PhoneNumbers;
            bool registerValid = DataBaseActions.RegisterUser(username, password, phoneNumbers);

            var responsePacket = new ServerStatusPacket();
            responsePacket.ResponseTo = ServerStatusPacket.ResponseTos.Register;


            if (registerValid)
            {
                responsePacket.ServerResonseStatus = ServerStatusPacket.Status.Successful;
                Server.NetManager.SendPacket(Server.FindSocketByName(username), responsePacket);
            }
            else
            {
                responsePacket.ServerResonseStatus = ServerStatusPacket.Status.Failed;
                Server.NetManager.SendPacket(Server.FindSocketByName(username), responsePacket);
            }



        }
        public static void AddByNumberRequest(Packet request)
        {
            var data = request as AddFriendByNumberRequest;
            bool successful = DataBaseActions.AddFriendRequest(data.UserName, data.PhoneNumber, false);

            var responsePacket = new ServerStatusPacket();
            responsePacket.ResponseTo = ServerStatusPacket.ResponseTos.AddFriend;


            if (successful)
            {
                responsePacket.ServerResonseStatus = ServerStatusPacket.Status.Successful;
                Server.NetManager.SendPacket(Server.FindSocketByName(data.UserName), responsePacket);
            }
            else
            {
                responsePacket.ServerResonseStatus = ServerStatusPacket.Status.Failed;
                Server.NetManager.SendPacket(Server.FindSocketByName(data.UserName), responsePacket);
            }
        }

        public static void AddByIDRequest(Packet request)
        {
            var data = request as AddFriendByIDRequest;
            bool successful = DataBaseActions.AddFriendRequest(data.UserName, data.ToUserID);

            var responsePacket = new ServerStatusPacket();
            responsePacket.ResponseTo = ServerStatusPacket.ResponseTos.AddFriend;


            if (successful)
            {
                responsePacket.ServerResonseStatus = ServerStatusPacket.Status.Successful;
                Server.NetManager.SendPacket(Server.FindSocketByName(data.UserName), responsePacket);
            }
            else
            {
                responsePacket.ServerResonseStatus = ServerStatusPacket.Status.Failed;
                Server.NetManager.SendPacket(Server.FindSocketByName(data.UserName), responsePacket);
            }
        }

        public static void GiveMeMyFriendRequests(Packet packet)
        {
            var data = packet as GetFriendRequestsToMeRequest;
            var requests = DataBaseActions.GetFriendRequestsOfUser(data.UserName);

            var response = new ServerFriendRequestList();
            response.Requests = requests;

            Server.NetManager.SendPacket(Server.FindSocketByName(data.UserName), response);
        }


    }
}
