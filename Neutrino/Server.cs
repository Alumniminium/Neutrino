using System;
using System.Net;
using System.Net.Sockets;

namespace Neutrino
{
    public class Server
    {
        public Action<Connection> OnConnection;
        public bool IsOnline { get; private set; }
        public Socket Socket { get; private set; }
        public ushort Port { get; private set; }
        public ushort BufferSize { get; private set; }
        private SocketAsyncEventArgs AcceptArgs;

        public Server()
        {
            AcceptArgs = SaeaFactory.GetArgs();
            AcceptArgs.Completed += (s, e) => Accepted(e);
        }
        public Server SetPort(ushort port)
        {
            Port = port;
            return this;
        }
        public Server SetBufferSize(ushort bufferSize)
        {
            BufferSize = bufferSize;
            return this;
        }
        public Server Start()
        {
            if (!IsOnline)
            {
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.Bind(new IPEndPoint(IPAddress.Any, Port));
                Socket.Listen(1);
                Accept();
                IsOnline = true;
                return this;
            }
            throw new Exception("Already started.");
        }

        private void Accept()
        {
            if (Socket == null)
                return;

            if (!Socket.AcceptAsync(AcceptArgs))
                Accepted(AcceptArgs);
        }

        private void Accepted(SocketAsyncEventArgs acceptArgs)
        {
            try
            {
                if (acceptArgs.SocketError == SocketError.Success)
                {
                    var socket = acceptArgs.AcceptSocket;
                    OnConnection?.Invoke(new Connection(socket, new byte[2048]));
                }
                acceptArgs.AcceptSocket = null;
                Accept();
            }
            catch
            {
                Destroy();
            }
        }


        public override string ToString()
        {
            var output = "Server ";
            if (IsOnline)
                output += "Online!";
            else
                output += "Offline!";
            output += Environment.NewLine;
            output += "Port: " + Port + Environment.NewLine;
            output += "Buffer Size: " + BufferSize + Environment.NewLine;
            return output;
        }

        public void Destroy()
        {
            IsOnline = false;
            Socket?.Dispose();
        }
    }
}
