using SignalR.EasyUse.Interface;

namespace ChatServerCS
{
    public class BroadcastPictureMessage : IClientMethod
    {
        public string Sender { get; set; }
        public byte[] Img { get; set; }
    }
}