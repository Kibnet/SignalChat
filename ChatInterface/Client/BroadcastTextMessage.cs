using SignalR.EasyUse.Interface;

namespace ChatServerCS
{
    public class BroadcastTextMessage : IClientMethod
    {
        public string Sender { get; set; }
        public string Message { get; set; }
    }
}