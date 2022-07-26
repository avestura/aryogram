using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MirogramServer.Network
{
    [Serializable]
    public class Packet
    {


        public enum PacketKinds
        {
            LoginRequest,
            RegisterRequest,
            MessageRequest,
            UpdateRequest,
            SearchRequest,
            NullPacket,
            GetMessagesRequest,
            AddFriendByNumberRequest,
            AddFriendByIDRequest,
            GetFriendsRequest,
            FriendAcceptDeclineRequest,
            GetFriendRequestsToMeRequest,
            GetListOfChatsRequest,
            StartChatRequest,

            StatusServerResonse,
            ServerFriendRequestList,
            ServerFriendList,
            ServerChatsList,
            ServerMessagesList,

        }
        public Packet() { }
        public Packet(PacketKinds Kind) { Type = Kind; }
        public string UserName { get; set; }
        public PacketKinds Type { get; set; }
        public const int BufferSize = 8192;


        public byte[] SerializedPacket()
        {
            using (var memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, this);
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Flush();
                return bytes;
            }
        }

        public static Packet DeserilizedPacket(byte[] serial)
        {
            using (var memoryStream = new MemoryStream(serial))
            {
                memoryStream.Position = 0;
                try
                {
                    return (Packet)(new BinaryFormatter()).Deserialize(memoryStream);
                }
                catch (Exception e)
                {
#if LOG_FAIL_CALLBACK
                            Console.WriteLine($">> Failed to deserialize stream (A terminated connection? maybe!)...");
#endif
                    return null;
                }
            }
        }



    }
}
