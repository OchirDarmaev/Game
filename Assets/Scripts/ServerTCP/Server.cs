using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace ServerTCP
{
    public class Server
    {
        private readonly int _port;
        private readonly ILogger _logger;

        public readonly ReplaySubject<byte[]> ReplySubject;

        public Server(int port, ILogger logger)
        {
            _port = port;
            _logger = logger;
            ReplySubject = new ReplaySubject<byte[]>();
        }

        public async Task ListenForIncommingRequests(CancellationToken cancellationToken)
        {
            var tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), _port);
            try
            {
                tcpListener.Start();
                _logger.Log(nameof(Server), "Server is listening");
                while (true)
                {
                    var client = await Task.Run(() => tcpListener.AcceptTcpClientAsync(), cancellationToken);
                    _ = Task.Run(async () =>
                      {
                          using (NetworkStream netstream = client.GetStream())
                          using (var ms = new MemoryStream())
                          {
                              await netstream.CopyToAsync(ms);
                              ReplySubject.OnNext(ms.ToArray());
                          }
                      }, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                ReplySubject.OnError(ex);
            }
            finally
            {
                tcpListener.Stop();
                ReplySubject.OnCompleted();
            }
        }
    }
}