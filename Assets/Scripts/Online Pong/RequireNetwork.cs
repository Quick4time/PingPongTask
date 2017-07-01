using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequireNetwork : MonoBehaviour {
    private void Awake()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
            Network.InitializeServer(1, 25005, true);
    }
}
