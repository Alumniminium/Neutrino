using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Xunit;

namespace Neutrino.Tests
{
    public class IntegrationTests : IClassFixture<SetupServer>
    {
        private SetupServer Setup;

        public IntegrationTests(SetupServer setup)
        {
            Setup = setup;
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
            var tcpCli = CreateClient();

            tcpCli.ConnectAsync(IPAddress.Loopback, 65000);
            Thread.Sleep(1000);

            Assert.True(tcpCli.Connected, "Client thinks its connected");
            Assert.True(Setup.GotConnection, "Server got a connection");
        }
        
        [Fact]
        public void ServerAcceptsPacket()
        {
            var tcpCli = CreateClient();
            tcpCli.Connect(IPAddress.Loopback, 65000);
            var stream = tcpCli.GetStream();

            var msg = Encoding.ASCII.GetBytes("Hello AluSocket!");
            var size = msg.Length + 4;
            stream.Write(BitConverter.GetBytes(size));
            stream.Write(msg);
            Assert.True(true);
        }
        [Fact]
        public void ServerRespondsToPacket()
        {
            var tcpCli = CreateClient();
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