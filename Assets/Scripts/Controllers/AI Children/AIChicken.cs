using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChicken : AIController
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
        // Command that runs decisions
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
                        ChangeState(AIState.Chase);
                    }
                }
                if (CanHear(target))
                {
                    ChangeState(AIState.Chase);
                }
                break;

            // If AISTate is chase
            case AIState.Chase:
                // Do work
                DoChaseState();
                // Check for transitions
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (IsDistanceLessThan(target, 2))
                {
                    ChangeState(AIState.Flee);
                }
                if (!IsDistanceLessThan(target, maxViewDistance))
                {
                    ChangeState(AIState.Idle);
                }
                break;

            // If AIState is flee
            case AIState.Flee:
                // Do work
                DoFleeState();
                // Check for transitions
                if (target == null)
                {
                    ChangeState(AIState.ChooseTarget);
                }
                if (!IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Idle);
                }
                break;

            // If AIState is choosetarget
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
