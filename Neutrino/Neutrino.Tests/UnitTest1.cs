using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Xunit;

namespace Neutrino.Tests
{
    public class ServerTests
    {
        [Fact]
        public void ServerStarts()
        {
            var server = new Server().Start();
            Assert.True(true);
        }
        [Theory]
        [InlineData(-30)]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(1024)]
        [InlineData(10000)]
        public void CanSetPort(ushort port)
        {
            var server = new Server().SetPort(port).Start();
            Assert.True(true);
        }
    }
    public class ClientTests
    {
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

            tcpCli.ConnectAsync(IPAddress.Loopback, 65000);
            Thread.Sleep(1000);

            Assert.True(tcpCli.Connected);
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
