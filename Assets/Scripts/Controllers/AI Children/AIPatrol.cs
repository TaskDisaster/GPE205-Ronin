using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrol : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Idle:
                // Do nothing
                DoIdleState();
                // Check for transition
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                ChangeState(AIState.Patrol);
                break;

            case AIState.Patrol:
                // Do work
                DoPatrolState();
                // Check for transitions
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (CanHear(target))
                {
                    ChangeState(AIState.Scan);
                }
                if (CanSee(target))
                {
                    ChangeState(AIState.Attack);
                }
                break;

            case AIState.Attack:
                // Do work
                DoAttackState();
                // Check for transitions
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (!CanSee(target))
                {
                    ChangeState(AIState.Patrol);
                }
                break;

            case AIState.Scan:
                // Do work
                DoScanState();
                // Check for transitions
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (CanSee(target))
                {
                    ChangeState(AIState.Attack);
                }
                if (!CanHear(target))
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
}
