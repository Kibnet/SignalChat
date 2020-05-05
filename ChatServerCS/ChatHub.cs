using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using ChatInterface.Server;
using Microsoft.AspNetCore.SignalR;
using SignalR.EasyUse.Server;

namespace ChatServerCS
{
    public class ChatHub : Hub, IChatHub
    {
        private static ConcurrentDictionary<string, User> ChatClients = new ConcurrentDictionary<string, User>();

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userName = ChatClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if (userName != null)
            {
                Clients.Others.SendAsync(new ParticipantDisconnection { Name = userName });
                Console.WriteLine($"<> {userName} disconnected");
            }
            return base.OnDisconnectedAsync(exception);
        }

        public override Task OnConnectedAsync()
        {
            var userName = ChatClients.SingleOrDefault((c) => c.Value.ID == Context.ConnectionId).Key;
            if (userName != null)
            {
                Clients.Others.SendAsync(new ParticipantReconnection { Name = userName });
                Console.WriteLine($"== {userName} reconnected");
            }
            return base.OnConnectedAsync();
        }

        public async Task<List<User>> Login(string name, byte[] photo)
        {
            if (!ChatClients.ContainsKey(name))
            {
                Console.WriteLine($"++ {name} logged in");
                List<User> users = new List<User>(ChatClients.Values);
                User newUser = new User { Name = name, ID = Context.ConnectionId, Photo = photo };
                Context.Items["Name"] = name;
                var added = ChatClients.TryAdd(name, newUser);
                if (!added) return null;
                await Clients.Others.SendAsync(new ParticipantLogin { Client = newUser });
                return users;
            }
            else
            {
                Console.WriteLine($"++ {name} logged in");
                List<User> users = new List<User>(ChatClients.Values);
                User newUser = new User { Name = name, ID = Context.ConnectionId, Photo = photo };
                Context.Items["Name"] = name;
                var added = ChatClients[name] = newUser;
                await Clients.Others.SendAsync(new ParticipantReconnection { Name = newUser.Name });
                return users;
            }
            return null;
        }

        public async Task Logout()
        {
            var name = Context.Items["Name"] as string;
            if (!string.IsNullOrEmpty(name))
            {
                User client = new User();
                ChatClients.TryRemove(name, out client);
                await Clients.Others.SendAsync(new ParticipantLogout { Name = name });
                Console.WriteLine($"-- {name} logged out");
            }
        }

        public async Task BroadcastTextMessage(string message)
        {
            var name = Context.Items["Name"] as string;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(message))
            {
                await Clients.Others.SendAsync(new BroadcastTextMessage { Sender = name, Message = message });
            }
        }

        public async Task BroadcastImageMessage(byte[] img)
        {
            var name = Context.Items["Name"] as string;
            if (img != null)
            {
                await Clients.Others.SendAsync(new BroadcastPictureMessage { Sender = name, Img = img });
            }
        }

        public async Task UnicastTextMessage(string recepient, string message)
        {
            var sender = Context.Items["Name"] as string;
            if (!string.IsNullOrEmpty(sender) && recepient != sender &&
                !string.IsNullOrEmpty(message) && ChatClients.ContainsKey(recepient))
            {
                User client = new User();
                ChatClients.TryGetValue(recepient, out client);
                await Clients.Client(client.ID).SendAsync(new UnicastTextMessage { Sender = sender, Message = message });
            }
        }

        public async Task UnicastImageMessage(string recepient, byte[] img)
        {
            var sender = Context.Items["Name"] as string;
            if (!string.IsNullOrEmpty(sender) && recepient != sender &&
                img != null && ChatClients.ContainsKey(recepient))
            {
                User client = new User();
                ChatClients.TryGetValue(recepient, out client);
                await Clients.Client(client.ID).SendAsync(new UnicastPictureMessage { Sender = sender, Img = img });
            }
        }

        public async Task Typing(string recepient)
        {
            if (string.IsNullOrEmpty(recepient)) return;
            var sender = Context.Items["Name"] as string;
            User client = new User();
            ChatClients.TryGetValue(recepient, out client);
            await Clients.Client(client.ID).SendAsync(new ParticipantTyping { Sender = sender });
        }
    }
}