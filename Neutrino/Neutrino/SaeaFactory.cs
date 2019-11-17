using System.Net.Sockets;

namespace Neutrino
{
    public static class SaeaFactory
    {
        public static SocketAsyncEventArgs GetArgs()
        {
            var args = new SocketAsyncEventArgs();
            return args;
        }
    }
}