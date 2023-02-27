using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISniper : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called on ce per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Idle:
                // Do Nothing
                DoIdleState();
                // Check for transition
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (CanSee(target))
                {
                    ChangeState(AIState.Attack);
                }
                break;

            case AIState.Attack:
                // Do work
                DoAttackState();
                // Check for transition
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (!CanSee(target))
                {
                    ChangeState(AIState.Idle);
                }
                break;

            case AIState.ChooseTarget:
                // Do work
                DoChooseTargetState();
                // Check for transitions
                if (target != null)
                {
                    ChangeState(AIState.Idle);
                }
                break;
        }
    }

    // No movement for attacking
    protected override void DoAttackState()
    {
        pawn.RotateTowards(target.transform.position);
        Shoot();
    }
 
}
