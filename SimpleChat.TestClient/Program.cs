/*
Project Orleans Cloud Service SDK ver. 1.0
 
Copyright (c) Microsoft Corporation
 
All rights reserved.
 
MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the ""Software""), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Threading.Tasks;
using Orleans;
using SimpleChat.InterfaceCollection;

namespace SimpleChat.TestClient
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            //we are letting the test silo run that's located in the orleans sdk folder, hence we need to wait untill it is loaded
            //you can tell when it is loaded when there's green text saying "x is your primary node"
            Console.WriteLine("wait till silo loaded");
            Console.ReadLine();

            GrainClient.Initialize("DevTestClientConfiguration.xml");

            //we make an instance of the observer, orleans will invoke these methods when "x" has changed in a grain
            var viewer = new ChatViewer();
            var viewerObj = ChatViewerFactory.CreateObjectReference(viewer).Result;

            //make a chat channel (irc style yeah!)
            var generalChat = ChannelFactory.GetGrain("#GeneralChat");
            //subscribe for changes in this channel
            generalChat.SubscribeForUpdates(viewerObj);

            //make another channel
            var aboutOrleansChat = ChannelFactory.GetGrain("#AboutOrleans");
            //also subscribe for changes
            aboutOrleansChat.SubscribeForUpdates(viewerObj);

            //get some user grains, note that for channels and users we made a primarykeystring -> just for demo purposes!
            var jane = UserFactory.GetGrain("Jane");
            var jack = UserFactory.GetGrain("Jack");

            JoinChannelAndSayHi(jane, generalChat);
            JoinChannelAndSayHi(jack,generalChat);

            JoinChannelAndSayHi(jane, aboutOrleansChat);
            

            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();

        }

        public static void JoinChannelAndSayHi(IUser user, IChannel channel)
        {
            //we add the user to the channel
            channel.Join(user.GetPrimaryKeyString());
            Task.Run(async () =>
            {
                //and say hi to everybody already present in the channel
                await user.SendMessage(channel.GetPrimaryKeyString(), new Message
                {
                    Content = "Hi there! My name is " + user.GetPrimaryKeyString() ,
                    Created = DateTime.UtcNow
                });
            });
        }
    }

    public class ChatViewer : IChatViewer
    {
        public void Update(Message latestMessage, string channel)
        {
            Console.WriteLine("observed in {0}: {1}", channel,latestMessage.Content);
        }
    }
}
