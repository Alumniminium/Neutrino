using System;

namespace Neutrino.Tests
{
    public class SetupServer : IDisposable
    {
        public Server S;
        public SetupServer()
        {
            S = new Server().SetPort(65000).SetBufferSize(1024).Start();
            S.OnConnection += OnConnection;
        }

        private void OnConnection(Connection obj) => GotConnection = true;
        public bool GotConnection { get; set; }

        public void Dispose()
        {
            S.Destroy();
        }
    }
}