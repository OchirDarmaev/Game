using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace ServerTCP
{
    public class Client : IDisposable
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly ILogger _logger;
        private TcpClient _client;

        public Client(IPAddress ipAddress, int port, ILogger logger)
        {
            _ipAddress = ipAddress;
            _port = port;
            _logger = logger;
            _client = new TcpClient();
        }

        public async Task ConnectAsync()
        {
            await _client.ConnectAsync(_ipAddress, _port);
            _logger.Log("ConnectAsync");
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        public async Task SendAsync(byte[] data)
        {
            using (var stream = _client.GetStream())
            {
                await stream.WriteAsync(data, 0, data.Length);
                _logger.Log("SendAsync");
                stream.Close();
            }
        }
    }
}