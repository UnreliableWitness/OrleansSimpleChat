using System.Threading.Tasks;
using Orleans;

namespace SimpleChat.InterfaceCollection
{
    public interface IUser : IGrainWithStringKey
    {
        /// <summary>
        /// sends a message to the specified channel (grain)
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessage(string channel, Message message);
    }
}
