using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_lobbyCanva = null;
    [SerializeField] private GameObject m_homeCanva = null;
    [SerializeField] private GameObject m_startGameButton = null;

    private void Awake()
    {
        MyNetworkManager.onServerJoin += OnJoin_SwitchCanvas;
        MyNetworkManager.onServerLeave += OnLeave_SwitchCanvas;
    }

    private void OnDestroy()
    {
        MyNetworkManager.onServerJoin -= OnJoin_SwitchCanvas;
        MyNetworkManager.onServerLeave -= OnLeave_SwitchCanvas;
    }

    public void OnJoin_SwitchCanvas(bool isHost)
    {
        m_homeCanva.SetActive(false);
        m_lobbyCanva.SetActive(true);
        m_startGameButton.SetActive(isHost);
    }

    public void OnLeave_SwitchCanvas()
    {
        Debug.Log($"Canva switch on leave home is {m_homeCanva.activeSelf}");
        m_homeCanva.SetActive(true);
        m_lobbyCanva.SetActive(false);
        m_startGameButton.SetActive(false);
    }

}
