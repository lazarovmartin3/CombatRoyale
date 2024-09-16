using UnityEngine;

public class AIPlayer : Player
{
    private Castle aiCastle;
    public enum AIState
    {
        Idle,
        GatheringResources,
        BuildArmy,
        Attack,
        Defend
    }

    private AIState currentState;
    private float decisionInterval = 2f; // AI decides every few seconds
    private float nextDecisionTime = 0f;

    private int defensiveUnitThreshold = 5;
    private int offensiveUnitThreshold = 10;

    public override void InitPlayer(int startGold, Castle castle, GameManager.PlayerInitData initData)
    {
        base.InitPlayer(startGold, castle, initData);
        aiCastle = castle as Castle;
        aiCastle.CreateSpawnableArea();
        currentState = AIState.Idle;
    }

    public override Castle GetSelectedCastle()
    {
        return aiCastle;
    }

    private void Update()
    {
        // Check if it's time for the AI to make a new decision
        if (Time.time >= nextDecisionTime)
        {
            MakeDecision();
            nextDecisionTime = Time.time + decisionInterval; // Schedule the next decision
        }
        print("AI GOLD " + GetGold() + " c_s " + currentState);
    }

    private void MakeDecision()
    {
        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState();
                break;
            case AIState.BuildArmy:
                HandleBuildArmyState();
                break;
            case AIState.GatheringResources:
                HandleGatheringResourcesState();
            break;
            case AIState.Attack:
                HandleAttackState();
                break;
            case AIState.Defend:
                HandleDefendState();
                break;
        }
    }

    private void TransitionToState(AIState newState)
    {
        currentState = newState;
    }

    private void HandleIdleState()
    {
        // AI decides to gather resources or build army depending on the situation
        if (GetGold() >= aiCastle.GetComponent<UnitCreator>().GetUnitCost(UnitCreator.UnitType.swordman))
        {
            TransitionToState(AIState.BuildArmy);
        }
        else
        {
            TransitionToState(AIState.GatheringResources);
        }
    }

    private void HandleGatheringResourcesState()
    {
        //Wait
    }

    private void HandleBuildArmyState()
    {
        // Build units if AI has enough gold
        if (GetGold() > aiCastle.GetComponent<UnitCreator>().GetUnitCost(UnitCreator.UnitType.swordman) && GetUnits().Count < offensiveUnitThreshold)
        {
            CreateUnit(UnitCreator.UnitType.swordman); // Build an offensive unit
        }

        if (GetUnits().Count >= offensiveUnitThreshold)
        {
            TransitionToState(AIState.Attack);
        }

        aiCastle.GetComponent<UnitCreator>().SpawnUnit(aiCastle.GetSpawnableArea()[0].gameObject);
    }

    private void HandleAttackState()
    {
        // AI attacks when it has sufficient units
        if (GetUnits().Count >= offensiveUnitThreshold)
        {
            AttackHumanPlayer();
        }
        else
        {
            TransitionToState(AIState.BuildArmy); // If it doesn't have enough units, go back to building
        }
    }

    private void HandleDefendState()
    {
        // AI defends when the human player attacks
        if (GetUnits().Count < defensiveUnitThreshold)
        {
            //CreateUnit(UnitCreator.UnitType.archer); // Build a defensive unit
        }

        // If the AI has enough defensive units, it can return to idle or build more units
        if (GetUnits().Count >= defensiveUnitThreshold)
        {
            TransitionToState(AIState.Idle);
        }
    }

    private void AttackHumanPlayer()
    {
        // Find human player's castle and send units to attack
        Player humanPlayer = GameManager.Instance.GetPlayer();
        Castle humanCastle = humanPlayer.GetSelectedCastle();

        // Command all AI units to attack human castle
        foreach (var unit in GetUnits())
        {
            unit.GoTo(humanCastle.GetPosition()); // Move units to the human castle's position
        }

        // After the attack, return to Idle
        TransitionToState(AIState.Idle);
    }

    private void CreateUnit(UnitCreator.UnitType unitType)
    {
        aiCastle.CreateUnit(unitType);
    }
}
