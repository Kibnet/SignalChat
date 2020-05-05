using SignalR.EasyUse.Interface;

namespace ChatServerCS
{
    public class ParticipantDisconnection : IClientMethod
    {
        public string Name { get; set; }
    }
}