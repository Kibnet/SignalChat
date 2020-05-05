using SignalR.EasyUse.Interface;

namespace ChatServerCS
{
    public class ParticipantTyping : IClientMethod
    {
        public string Sender { get; set; }
    }
}