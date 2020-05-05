using ChatInterface.Server;
using SignalR.EasyUse.Interface;

namespace ChatServerCS
{
    public class ParticipantLogin : IClientMethod
    {
        public User Client { get; set; }
    }
}