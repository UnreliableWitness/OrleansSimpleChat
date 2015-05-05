using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Orleans;
using SimpleChat.InterfaceCollection;

namespace SimpleChat.Grains
{
    public class Channel : Grain, IChannel
    {
        private List<string> _users;
        private List<Message> _messages;

        [NonSerialized]
        private ObserverSubscriptionManager<IChatViewer> _viewers;

        public override Task OnActivateAsync()
        {
            //this is sort of a list of all subscribers 
            _viewers = new ObserverSubscriptionManager<IChatViewer>();

            _users = new List<string>();
            _messages = new List<Message>();

            return base.OnActivateAsync();
        }

        public Task<List<string>> GetUsers()
        {
            return Task.FromResult(_users);
        }

        public Task AddMessage(Message message)
        {
            _messages.Add(message);

            //notify will call the "update" on all subscribers
            Notify(message);
            return TaskDone.Done;
        }

        public Task<List<Message>> GetMessages()
        {
            return Task.FromResult(_messages);
        }


        public Task Join(string user)
        {
            _users.Add(user);

            //send a general message in the channel when a user joined
            var message = 
                new Message
                {
                    Content = user+" joined channel " + this.GetPrimaryKeyString(), Created = DateTime.UtcNow
                };
            _messages.Add(message);

            Notify(message);

            return TaskDone.Done;
        }

        public Task SubscribeForUpdates(IChatViewer viewer)
        {
            _viewers.Subscribe(viewer);
            return TaskDone.Done;
        }

        public Task Notify(Message message)
        {
            _viewers.Notify(v => v.Update(message, this.GetPrimaryKeyString()));
            return TaskDone.Done;
        }
    }
}
