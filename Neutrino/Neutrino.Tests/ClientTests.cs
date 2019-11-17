using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Xunit;

namespace Neutrino.Tests
{
    public class ClientTests : IClassFixture<SetupServer>
    {
        private Server Server;
        public ClientTests(SetupServer setup)
        {
            Server = setup.S;
        }
        private static TcpClient CreateClient()
        {
            var tcpCli = new TcpClient();
            tcpCli.ReceiveTimeout = 10;
            tcpCli.SendTimeout = 10;
            return tcpCli;
        }

        [Fact]
        public void CanConnect()
        {
            TcpClient tcpCli = CreateClient();

            var serverGotConnection = false;
            Server.OnConnection += connection =>
            {
                serverGotConnection = true;
            };

            tcpCli.ConnectAsync(IPAddress.Loopback, 65000);
            Thread.Sleep(1000);

            Assert.True(tcpCli.Connected, "Client thinks its connected");
            Assert.True(serverGotConnection, "Server got a connection");
        }

        [Fact]
        public void CanSend()
        {
            TcpClient tcpCli = CreateClient();
            tcpCli.Connect(IPAddress.Loopback, 65000);
            var stream = tcpCli.GetStream();

            var msg = Encoding.ASCII.GetBytes("Hello AluSocket!");
            var size = msg.Length + 4;
            stream.Write(BitConverter.GetBytes(size));
            stream.Write(msg);
            Assert.True(true);
        }
        [Fact]
        public void CanReceive()
        {
            TcpClient tcpCli = CreateClient();
            tcpCli.Connect(IPAddress.Loopback, 65000);
            var stream = tcpCli.GetStream();
            var msg = Encoding.ASCII.GetBytes("Hello AluSocket!");
            var size = msg.Length + 4;
            stream.Write(BitConverter.GetBytes(size));
            stream.Write(msg);

            var buffer = new byte[1024];
            stream.Read(buffer);

            Assert.True(true);
        }
    }
}