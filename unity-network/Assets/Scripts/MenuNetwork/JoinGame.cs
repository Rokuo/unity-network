using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinGame : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_hostAddress = null;

    public void OnClickJoinGame()
    {
        MyNetworkManager.singleton.networkAddress = m_hostAddress.text;

        MyNetworkManager.singleton.StartClient();
    }
}
