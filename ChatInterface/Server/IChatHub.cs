using System.Collections.Generic;
using System.Threading.Tasks;
using SignalR.EasyUse.Interface;

namespace ChatInterface.Server
{
    public interface IChatHub :IServerMethods
    {
        Task<List<User>> Login(string name, byte[] photo);
        Task Logout();
        Task BroadcastTextMessage(string message);
        Task BroadcastImageMessage(byte[] img);
        Task UnicastTextMessage(string recepient, string message);
        Task UnicastImageMessage(string recepient, byte[] img);
        Task Typing(string recepient);
    }
}