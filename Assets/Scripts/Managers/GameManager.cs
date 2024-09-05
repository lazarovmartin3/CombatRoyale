using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

        CreateNewPlayer(5, 0, new Vector3(-90, -180, 90), GetDataByColor(PlayerColors.red));
        CreateNewPlayer(5, 9, new Vector3(-90, 0, 90), GetDataByColor(PlayerColors.green), true);
    }

    private void CreateNewPlayer(int posX, int posY, Vector3 rotation, PlayerInitData data, bool isAI = false)
    {
        GameObject playerObject = new GameObject();
        playerObject.transform.SetParent(this.transform);
        playerObject.name = isAI ? "AI_Player_" + players.Count : "Player_" + players.Count;

        GameObject castle = Instantiate(castlePrefab, Map.Instance.GetPosition(posX, posY), Quaternion.Euler(rotation), playerObject.transform);

        if (isAI)
        {
            playerObject.AddComponent<AIPlayer>();
            castle.GetComponent<Castle>().SetOwner(playerObject.GetComponent<AIPlayer>());
            playerObject.GetComponent<AIPlayer>().InitPlayer(START_GOLD, castle.GetComponent<Castle>(), data);
            players.Add(playerObject.GetComponent<AIPlayer>());
        }
        else
        {
            playerObject.AddComponent<HumanPlayer>();
            castle.GetComponent<Castle>().SetOwner(playerObject.GetComponent<HumanPlayer>());
            playerObject.GetComponent<HumanPlayer>().InitPlayer(START_GOLD, castle.GetComponent<Castle>(), data);
            players.Add(playerObject.GetComponent<HumanPlayer>());
        }
        castle.GetComponent<Castle>().SetGridPosition(posX, posY);
        castle.GetComponent<Castle>().CreateSpawnableArea();
    }

    public HumanPlayer GetPlayer()
    {
        foreach (Player player in players)
        {
            if (player is HumanPlayer)
                return player as HumanPlayer;
        }
        return null;
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
