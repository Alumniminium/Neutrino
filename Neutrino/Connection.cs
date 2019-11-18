using System;
using System.Net.Sockets;

namespace Neutrino
{
    public class Connection
    {
        public Socket Socket;
        public byte[] Buffer;
        private Span<byte> GetSendBuffer() => Buffer.AsSpan().Slice(Buffer.Length / 2);
        private Span<byte> GetReceiveBuffer() => Buffer.AsSpan().Slice(0, Buffer.Length / 2);
        public Connection(Socket socket, byte[] buffer)
        {
            Socket = socket;
            Buffer = buffer;
        }
    }
}