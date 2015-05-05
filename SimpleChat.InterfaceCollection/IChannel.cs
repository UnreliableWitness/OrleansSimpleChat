using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Orleans;

namespace SimpleChat.InterfaceCollection
{
    public interface IChannel : IGrainWithStringKey
    {
        /// <summary>
        /// retrieves all users in the channel
        /// </summary>
        /// <returns></returns>
        Task<List<string>>  GetUsers();

        /// <summary>
        /// adds a message to the channel
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task AddMessage(Message message);

        /// <summary>
        /// gets all messages sent in the channel
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetMessages();

        /// <summary>
        /// make a user join this channel
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task Join(string user);

        /// <summary>
        /// add a subscriber to the grain's subscriber list
        /// </summary>
        /// <param name="viewer"></param>
        /// <returns></returns>
        Task SubscribeForUpdates(IChatViewer viewer);

    }
}
