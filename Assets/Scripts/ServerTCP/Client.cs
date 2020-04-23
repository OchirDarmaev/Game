using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace ServerTCP
{
    public class Client
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
        }

        public async Task<IDisposable> ConnectAsync()
        {
            _client = new TcpClient();
            await _client.ConnectAsync(_ipAddress, _port);
            _logger.Log("ConnectAsync");
            return _client;
        }

        public async Task SendAsync(byte[] data)
        {
            using (var stream = _client.GetStream())
            {
                await stream.WriteAsync(data, 0, data.Length);
                _logger.Log("SendAsync");
            }
        }
    }
}