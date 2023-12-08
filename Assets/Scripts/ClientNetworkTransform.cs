using Unity.Netcode.Components;
using UnityEngine;

// This network transform is based on the client's information
// This means the client tells the server where the client is

namespace Players 
{
    public enum AuthorityMode
    {
        Server,
        Client
    }

    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        public AuthorityMode authorityMode = AuthorityMode.Client;
        protected override bool OnIsServerAuthoritative() => authorityMode == AuthorityMode.Server;
    }
}
