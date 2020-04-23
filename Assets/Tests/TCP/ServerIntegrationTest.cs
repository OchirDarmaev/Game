using NUnit.Framework;
using ServerTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Tests
{
    public class ServerIntegrationTest
    {
        [Test]
        public void WhenClientSendMsg_ServerShoultGetIt()
        {
            // Arrange
            var expMsg = Encoding.UTF8.GetBytes("test data");
            const string IpString = "127.0.0.1";
            const int Port = 8080;

            var server = new Server(Port, Debug.unityLogger);
            var ctsTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            ctsTimeout.Token.Register(() => Assert.Fail("timeout"));

            Task.Run(async () => await server.ListenForIncommingRequests(ctsTimeout.Token));

            // Act
            Task.Run(async () =>
             {
                 using (var client = new Client(IPAddress.Parse(IpString), Port, Debug.unityLogger))
                 {
                     await client.ConnectAsync();
                     await client.SendAsync(expMsg);
                 }
             }, ctsTimeout.Token)
                 .GetAwaiter()
                 .GetResult();

            // Assert
            server.ReplySubject.Subscribe(res =>
             {
                 Assert.AreEqual(expMsg, res);
             });
        }

        [Test]
        public void WhenClientSend5Msg_ServerShoultGetIt()
        {
            // Arrange
            var count = 10;
            var expMsgs = Enumerable
                .Repeat(1, count)
                .Select(x => Encoding.UTF8.GetBytes("test data " + x));

            const string IpString = "127.0.0.1";
            const int Port = 8080;

            var server = new Server(Port, Debug.unityLogger);
            var ctsTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            ctsTimeout.Token.Register(() => Assert.Fail("timeout"));

            Task.Run(async () => await server.ListenForIncommingRequests(ctsTimeout.Token));

            Task.Run(async () =>
            {
                foreach (var msg in expMsgs)
                    using (var client = new Client(IPAddress.Parse(IpString), Port, Debug.unityLogger))
                    {
                        await client.ConnectAsync();
                        await client.SendAsync(msg);
                    }

            }, ctsTimeout.Token)
                 .GetAwaiter()
                 .GetResult();

            // Assert
            List<byte[]> recieveMsgs = new List<byte[]>();
            server.ReplySubject.Subscribe(msg =>
            {
                recieveMsgs.Add(msg);

                if (recieveMsgs.Count >= count)
                {
                    CollectionAssert.AreEquivalent(expMsgs, recieveMsgs);
                }
            });
        }
    }
}