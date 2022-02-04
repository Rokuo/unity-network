using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class HostGame : MonoBehaviour
{
    public void OnClickHostServer()
    {
        MyNetworkManager.singleton.StartHost();
    }
}
