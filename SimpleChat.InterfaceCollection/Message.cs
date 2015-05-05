using System;

namespace SimpleChat.InterfaceCollection
{
    [Serializable]
    public class Message
    {
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}
