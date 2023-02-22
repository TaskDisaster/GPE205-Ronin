using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    #region Variables
    public enum AIState { Idle, Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost };
    public AIState currentState;
    private float lastStateChangeTime;
    public float fleeDistance;
    #endregion

    // Object of interest for certain states like Chase or Attack
    public GameObject target;

    // Start is called before the first frame update
    public override void Start()
    {
        // Set currentState to Idle
        currentState = AIState.Idle;

        // Run the parent (base) Start
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        // Make decisions
        MakeDecisions();
        // Run the parent (base) Update
        base.Update();
    }

    public void MakeDecisions()
    {
        // Command that runs decisions
        switch (currentState)
        {
            // If AIState is idle
            case AIState.Idle:
                // Do work
                DoIdleState();
                // Check for transitions
                if (IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Chase);
                }
                break;

            // If AISTate is chase
            case AIState.Chase:
                // Do work
                DoChaseState();
                // Check for transitions
                if (!IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Idle);
                }
                break;
        }
    }

    // Changes state of AIState
    public virtual void ChangeState(AIState newState)
    {
        // Change the current state
        currentState = newState;

        // Save the time when we changed states
        lastStateChangeTime = Time.time;
    }

    // Checks distance between target and AI to do stuff
    protected bool IsDistanceLessThan(GameObject target, float distance)
    {
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region States
    public void DoSeekState()
    {
        Seek(target);
    }

    protected virtual void DoChaseState()
    {
        Seek(target);
    }

    protected virtual void DoIdleState()
    {
        // Do nothing :)
    }

    protected virtual void DoAttackState()
    {
        // Chass
        Seek(target);
        // Shoot
        Shoot();
    }
    #endregion States

    #region Behavior
    public void Seek(GameObject target)
    {
        // RotateTowards the Function
        pawn.RotateTowards(target.transform.position);
        // Move Forward
        pawn.MoveForward();
    }

    public void Seek(Transform targetTransform)
    {
        // Seek the position of our target Transform
        pawn.RotateTowards(targetTransform.position);
        // Move Forward
        pawn.MoveForward();
    }

    public void Seek(Pawn targetPawn)
    {
        // Seek that pawn's transform!
        pawn.RotateTowards(targetPawn.transform.position);
    }

    public void Seek(Vector3 targetPosition)
    {
        // Rotate towards the position
        pawn.RotateTowards(targetPosition);
        // Move Forward
        pawn.MoveForward();
    }

    public void Shoot()
    {
        // Tell the pawn to shoot
        pawn.Shoot();
    }

    public void Flee()
    {
        // Find the Vector to our target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;
        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;
        // Find the vector we could travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance;
        // Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);
    }
    #endregion
}
