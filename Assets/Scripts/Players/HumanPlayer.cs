using UnityEngine;

public class HumanPlayer : Player
{
    private Castle humanCastle;

    public override void InitPlayer(int startGold, Castle castle, GameManager.PlayerInitData initData)
    {
        base.InitPlayer(startGold, castle, initData);
        humanCastle = castle as Castle;
    }

    public override Castle GetSelectedCastle()
    {
        return humanCastle;
    }
}
