using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LeaveServer : MonoBehaviour
{
    public void OnClickLeaveServer()
    {
        Debug.Log($"is host ? : {NetworkClient.isHostClient}");
        if (!NetworkClient.isHostClient)
            MyNetworkManager.singleton.StopClient();
        else
            MyNetworkManager.singleton.StopHost();
    }
}
