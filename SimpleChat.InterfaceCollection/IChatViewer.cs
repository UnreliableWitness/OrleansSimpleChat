using Orleans;

namespace SimpleChat.InterfaceCollection
{
    public interface IChatViewer : IGrainObserver
    {
        /// <summary>
        /// will be triggered in the channel manually when we want the subscribers to update
        /// </summary>
        /// <param name="latestMessage"></param>
        /// <param name="channel"></param>
        void Update(Message latestMessage, string channel);
    }
}
