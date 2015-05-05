using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;
using SimpleChat.InterfaceCollection;

namespace SimpleChat.Grains
{
    public class User : Grain, IUser
    {
        private List<Message> _messagesIveSent;

        public override Task OnActivateAsync()
        {
            _messagesIveSent = new List<Message>();
            return base.OnActivateAsync();
        }

        public async Task SendMessage(string channel, Message message)
        {
            var channelGrain = GrainFactory.GetGrain<IChannel>(channel);
            _messagesIveSent.Add(message);

            await channelGrain.AddMessage(message);
        }
    }
}
