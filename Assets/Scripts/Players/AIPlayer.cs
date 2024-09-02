using UnityEngine;

public class AIPlayer : Player
{
    private Castle aiCastle;

    public override void InitPlayer(int startGold, Castle castle, GameManager.PlayerInitData initData)
    {
        base.InitPlayer(startGold, castle, initData);
        aiCastle = castle as Castle;
    }

    public override Castle GetSelectedCastle()
    {
        return aiCastle;
    }

    private void Start()
    {
        Invoke("CreateSwordman", 2);
    }

    private void CreateSwordman()
    {
        aiCastle.CreateUnit(UnitCreator.UnitType.swordman);
    }
}
