using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private List<Player> players = new List<Player>();
    [SerializeField] private int currentSceneLevel = 1;
    private int maxSceneLevel = 3;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void AddNewPlayer(Player player)
    {
        player.OnDeath += CheckDeathPlayer;
        ColorPlayer colorPlayer = player.GetComponent<ColorPlayer>();
        Color randomColor = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );
        colorPlayer.SetPlayerColor(randomColor);
        Debug.Log(randomColor);

        players.Add(player);
        Debug.Log("Number of player: " + players.Count);
    }

    private string GetNextSceneName()
    {
        currentSceneLevel += 1;
        Debug.Log("Next Level: " + currentSceneLevel);
        if (currentSceneLevel > maxSceneLevel)
            currentSceneLevel = 1;
        return ("GameScene" + currentSceneLevel);
    }

    private void CheckDeathPlayer()
    {
        int numberPlayerAlive = 0;
        int indexWinner = 0;

        for (int i = 0; i < players.Count; i++) {
            if (players[i].isDead == false) {
                numberPlayerAlive += 1;
                indexWinner = i;
            }
        }

        Debug.Log("Number of players alive: " + numberPlayerAlive);
        if (numberPlayerAlive == 1) {
            // update score of the winner
            players[indexWinner].SetPlayerScore();
            // trigger change scene
            GameObject.FindWithTag("NetworkManager").GetComponent<MyNetworkManager>().ChangeScene(GetNextSceneName());

            Transform spawnPoints = GameObject.FindGameObjectWithTag("SpawnPoints").GetComponent<Transform>();
            List<Vector3> positions = new List<Vector3>();
            foreach (Transform child in spawnPoints) {
                positions.Add(child.position);
            }
            // reset all players to default state
            for (int i = 0; i < players.Count; i++) {
                Debug.Log("Spawn pos: " + positions[i]);
                players[i].SetDefaults(positions[i]);
            }
        }

    }

}
