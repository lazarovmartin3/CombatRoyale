using UnityEngine;

public class AI_Player : MonoBehaviour
{
    private Player ai_player;

    private void Start()
    {
        ai_player = GameManager.Instance.GetAllPlayers()[1];
        if (ai_player == null) print("No AI player found !!!");
        Invoke("CreateSwordman", 2);
    }

    private void CreateSwordman()
    {
        ai_player.GetSelectedCastle().CreateUnit(UnitCreator.UnitType.swordman);
    }
}
