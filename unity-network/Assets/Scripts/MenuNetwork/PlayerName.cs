using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_nameInput = null;

    public void SavePlayerName()
    {
        PlayerPrefs.SetString("DisplayName", m_nameInput.text);
    }
}
