using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Renderer unitMaterial;

    private UnitCreator.UnitType unitType;
    private float maxHP;
    private float movingSpeed;
    private float attackTimeElapsed;

    private List<Node> path;
    private int pathIndex = 0;
    private Vector2Int currentPos;

    private enum States { idle, goToTarget, wondering, attack };
    private States currentState;
    private float maxIdleTime = 2;
    private float idleTimeElapsed = 0;

    private float attackRange; // The range of the sword swing
    private float attackDamage; // Damage per hit
    private float attackCooldown = 1.5f; // Time between attacks

    private Transform target;
    private bool canAttack = true;
    private Player owner;

    public void SetType(UnitCreator.UnitType unitType)
    {
        this.unitType = unitType;

        switch (unitType) 
        {
            case UnitCreator.UnitType.swordman:
                maxHP = 100;
                GetComponent<Health>().SetHealth(maxHP);
                movingSpeed = 5;
                attackDamage = 15;
                attackRange = 2f;
                attackCooldown = 1.5f;
                break;
        }
        currentState = States.wondering;
    }

    public void SetPosition(Vector2Int pos)
    {
        currentPos = pos;
    }

    public Vector2Int GetPositionXY()
    {
        return currentPos;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void ChangeMaterial(Material material)
    {
        unitMaterial.material = material;
    }

    public void SetOwner(Player player)
    {
        owner = player;
    }

    public void GoTo(Vector2Int newPos)
    {
        path = Map.Instance.FindPath(transform.position, Map.Instance.GetPosition(newPos.x, newPos.y));
        pathIndex = 0;
        currentState = States.goToTarget;
    }

    private void Update()
    {
        StateMachine();
    }

    private void StateMachine()
    {
        switch (currentState)
        {
            case States.idle:

                HandleIdleState();

                break;
            case States.goToTarget:

                HandleGoToTargetState();
                currentPos = Map.Instance.GetXY_fromPosition(transform.position);
                
                break;
            case States.wondering:

                HandleWonderingState();

                break;
            case States.attack:

                HandleAttackState();

                break;
        }
    }

    private void HandleIdleState()
    {
        idleTimeElapsed += Time.deltaTime;
        if (idleTimeElapsed >= maxIdleTime)
        {
            idleTimeElapsed = 0;
            currentState = States.wondering;
        }
    }

    private void HandleGoToTargetState()
    {
        if (target == null || path == null || pathIndex >= path.Count)
        {
            // No valid target or path, return to idle
            TransitionToState(States.idle);
            return;
        }

        // Check if target moved from its last known position
        Vector2Int targetCurrentPos = Map.Instance.GetXY_fromPosition(target.position);
        Vector2Int targetLastPos = new Vector2Int(path[path.Count - 1].x, path[path.Count - 1].y);

        if (targetCurrentPos != targetLastPos)
        {
            // Recalculate path to the updated target position
            GoTo(targetCurrentPos);
        }

        MoveAlongPath();

        // If the path is completed or the target is within attack range, transition to attack
        if (pathIndex >= path.Count && Vector3.Distance(transform.position, target.position) < attackRange)
        {
            TransitionToState(States.attack);
        }
    }

    private void HandleWonderingState()
    {
        target = FindClosestTarget();

        if (target != null)
        {
            Vector2Int targetPos = GetTargetPosition(target);

            if (targetPos != Vector2Int.zero)
            {
                GoTo(targetPos);
            }
        }
        else
        {
            TransitionToState(States.idle);
        }
    }

    private void HandleAttackState()
    {
        if (canAttack && target != null)
        {
            PerformAttack();
        }
        else
        {
            TransitionToState(States.idle);
        }
    }

    private void TransitionToState(States newState)
    {
        currentState = newState;
    }

    private Transform FindClosestTarget()
    {
        Transform closestTarget = UnitsManager.Instance.GetClosestUnit(owner, GetPosition())?.transform;

        if (closestTarget == null)
        {
            // Logic to find the closest building or another target
        }

        return closestTarget;
    }

    private Vector2Int GetTargetPosition(Transform target)
    {
        if (target == null) return Vector2Int.zero;

        Unit unit = target.GetComponent<Unit>();
        if (unit != null)
        {
            return unit.GetPositionXY();
        }

        // Logic for getting positions of other types of targets like buildings

        return Vector2Int.zero;
    }


    private void MoveAlongPath()
    {
        if (path == null || pathIndex >= path.Count)
        {
            currentState = States.idle; // Path completed or invalid, switch to idle
            return;
        }

        Node targetNode = path[pathIndex];
        Vector3 targetPosition = targetNode.worldPosition;
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * movingSpeed * Time.deltaTime;
        transform.LookAt(target);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            pathIndex++;
        }

        // Check if reached the end of the path
        if (pathIndex >= path.Count)
        {
            currentState = States.idle;
        }
    }

    private void PerformAttack()
    {
        if (target == null) return;

        // Check if the target is within attack range
        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            // Deal damage to the target
            target.GetComponent<Health>().TakeDamage(attackDamage);
            Debug.Log("Attacked and dealt " + attackDamage + " damage");

            // Start cooldown
            canAttack = false;
            attackTimeElapsed = 0f;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
        else
        {
            currentState = States.goToTarget; // If target is out of range, move towards it
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
        currentState = States.attack; // Reset to idle state or any other logic
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        currentState = States.goToTarget; // Start moving towards the new target
    }

    public void Dead()
    {
        owner.RemoveUnit(this);
        Destroy(this.gameObject);
    }
}
