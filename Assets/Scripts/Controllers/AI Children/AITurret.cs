using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurret : AIController
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
            // If AIState is idle
            case AIState.Idle:
                // Do work
                DoIdleState();
                // Check for transitions
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (IsDistanceLessThan(target, maxViewDistance))
                {
                    if (CanSee(target))
                    {
                        ChangeState(AIState.Attack);
                    }
                }
                if (CanHear(target))
                {
                    ChangeState(AIState.Scan);
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
                    ChangeState(AIState.Idle);
                }
                break;

            case AIState.Scan:
                // Do work
                DoScanState();
                // Check for transition
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (CanSee(target))
                {
                    ChangeState(AIState.Attack);
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

    #region States
    public override void Seek(GameObject target)
    {
        pawn.RotateTowards(target.transform.position);
    }
    #endregion

    #region Behavior

    #endregion
}
