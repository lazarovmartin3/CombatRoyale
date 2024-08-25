using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum PlayerColors { black, blue, red, green, pink, white, yellow };
    [Serializable]
    public struct PlayerInitData
    {
        public Material unitMaterial;
        public Material buildingMaterial;
        public PlayerColors playerColor;
    }

    [SerializeField]
    private GameObject castlePrefab;
    [SerializeField]
    private PlayerInitData[] playerInitData;
    
    private List<Player> players = new List<Player>();

    private int START_GOLD = 300;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Map.Instance.GenerateMap();

        CreateNewPlayer(5, 0, new Vector3(-90, -180, 90), GetDataByColor(PlayerColors.blue));
        CreateNewPlayer(5, 9, new Vector3(-90, 0, 90), GetDataByColor(PlayerColors.green));
    }

    private void CreateNewPlayer(int posX, int posY, Vector3 rotation, PlayerInitData data)
    {
        GameObject playerObject = new GameObject();
        playerObject.transform.SetParent(this.transform);
        playerObject.name = "Player_" + players.Count;
        playerObject.AddComponent<Player>();
        GameObject castle = Instantiate(castlePrefab, Map.Instance.GetPosition(posX, posY), Quaternion.Euler(rotation), playerObject.transform);
        castle.GetComponent<Castle>().SetOwner(playerObject.GetComponent<Player>());
        castle.GetComponent<Castle>().SetGridPosition(posX, posY);
        playerObject.GetComponent<Player>().InitPlayer(START_GOLD, castle.GetComponent<Castle>(), data);
        players.Add(playerObject.GetComponent<Player>());
        if (players.Count > 1)
        {
            players[1].AddComponent<AI_Player>();
        }
    }

    public Player GetPlayer()
    {
        return players[0];
    }

    public List<Player> GetAllPlayers()
    {
        return players;
    }

    private PlayerInitData GetDataByColor(PlayerColors color)
    {
        foreach (var playerData in playerInitData)
        {
            if(playerData.playerColor == color)
                return playerData;
        }
        return playerInitData[0];
    }

}
