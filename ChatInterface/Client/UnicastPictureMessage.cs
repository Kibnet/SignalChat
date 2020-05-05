using SignalR.EasyUse.Interface;

namespace ChatServerCS
{
    public class UnicastPictureMessage : IClientMethod
    {
        public string Sender { get; set; }
        public byte[] Img { get; set; }
    }
}