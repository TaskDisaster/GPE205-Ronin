using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AIController : Controller
{
    #region Variables
    public enum AIState { Idle, Guard, Chase, Flee, Patrol, Attack, Scan, BackToPost, ChooseTarget };
    public bool isLooping;
    public bool isSeeing = true;
    public bool isHearing = true;
    public AIState currentState;
    private float lastStateChangeTime;
    public float fleeDistance;
    public Transform[] waypoints;
    public float waypointStopDistance;
    private int currentWaypoint = 0;
    public int hearingDistance;
    public float fieldOfView;
    public float maxViewDistance;
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

    // Commands that do eveything
    public virtual void MakeDecisions()
    {

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

    protected void TargetPlayerOne()
    {
        // If the GameManager exists
        if (GameManager.Instance != null)
        {
            // And there are players in it
            if (GameManager.Instance.players.Count > 0)
            {
                if (GameManager.Instance.players[0].pawn.gameObject != null)
                // Then target the gameObject of the pawn of the first player controller in the list
                target = GameManager.Instance.players[0].pawn.gameObject;
            }
        }
    }
    public bool CanHear(GameObject target)
    {
        // If isHearing false, return false
        if (isHearing == false)
        {
            return false;
        }
        // Get the target's NoiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();

        // If they don't have onem they can't make noise, so return false
        if (noiseMaker == false)
        {
            return false;
        }
        // If they are making 0 noise then they also can't be heard
        if (noiseMaker.volumeDistance <= 0)
        {
            return false;
        }
        // If they are making noise, add the volumeDistance in the nopisemaker to the hearingDistance of this Ai
        float totalDistance = noiseMaker.volumeDistance + hearingDistance;
        // If the distnace between our pawn and target is closer than this...
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            // ... then we can hear the target
            //UnityEngine.Debug.LogWarning("I HEAR YOU");
            return true;
        }
        else
        {
            //UnityEngine.Debug.LogWarning("I CANNOT HEAR YOU");
            return false;
        }
    }

    public bool CanSee(GameObject target)
    {
        // If isSeeing false, return false
        if (isSeeing == false)
        {
            return false;
        }

        // Find the vector from the agent to the target
        Vector3 agentToTargetVector = target.transform.position - transform.position;
        agentToTargetVector.Normalize();
        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);

        // If that angle is out of our field of view
        if (angleToTarget >= fieldOfView)
        {
            return false;

        }
        
        // If the target is further than our maxViewDistance
        if (!IsDistanceLessThan(target, maxViewDistance))
        {
            return false;
        }

        // Checking for target collider
        Collider targetCollider = target.GetComponent<Collider>();

        // Raycast to make sure nothing is blocking view
        Ray ray = new Ray(transform.position, agentToTargetVector);
        RaycastHit hitInfo;

        // If the raycast doesn't hit anything return nothing
        if (!Physics.Raycast(ray, out hitInfo, maxViewDistance))
        {
            UnityEngine.Debug.LogWarning("NOTHING TO SEE HERE");
            return false;
        }

        // if our raycast hits nothing, we can't see them
        if (!Physics.Raycast(transform.position, agentToTargetVector, maxViewDistance))
        {
            UnityEngine.Debug.LogWarning("NOTHING NOTHING TO SEE HERE");
            return false;
        }

        // If the target collider is hit, then we are seen
        if (hitInfo.collider == targetCollider)
        {
        // I SEE YOU
        UnityEngine.Debug.LogWarning("I SEE YOU");
        return true;
        }

        // If I cannot see you, then something is blocking me
        UnityEngine.Debug.LogWarning("BLOCKED");
        return false;
        
    }

    protected bool HasTarget()
    {
        // return true if we hava target, false if we don't
        return (target != null);
    }

    protected void TargetNearestTank()
    {
        // Get a list of all the tanks (pawns)
        Pawn[] allTanks = FindObjectsOfType<Pawn>();

        // Assume that the first tank is the closest
        Pawn closestTank = allTanks[0];
        float closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);

        // Iterate through them one at a time
        foreach (Pawn tank in allTanks)
        {
            // Check if it's a player tankPrefab
            if (tank == GameManager.Instance.tankPawnPrefab)
            { 
                // If this one is closer than the closest
                if (Vector3.Distance(pawn.transform.position, tank.transform.position) < closestTankDistance) 
                {
                    // It is the closest tank
                    closestTank = tank;
                    closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);
                }
            }    
        }

        // Target the closest tank
        target = closestTank.gameObject;
    }

    #region States
    protected virtual void DoChooseTargetState()
    {
        ChooseTarget();
    }

    protected virtual void DoPatrolState()
    {
        Patrol();
    }

    protected virtual void DoScanState()
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

    protected virtual void DoFleeState()
    {
        Flee();
    }
    #endregion States

    #region Behavior
    public void ChooseTarget()
    {
        TargetNearestTank();
    }

    public virtual void Seek(GameObject target)
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

        // Find the distance the target is from the pawn
        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);
        // Get the percentage of our FleeDistance
        float percentOfFleeDistance = targetDistance / fleeDistance;
        // Clamp the distance of FleeDistance to between 0 and 1
        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);
        // Flip the percentage of percentofFleeDistance
        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;

        // Find the vector we could travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance * flippedPercentOfFleeDistance;

        // Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);
 
    }

    public void Patrol()
    {
        // IF we have enough waypoints in our list to move to a current waypoint
        if (waypoints.Length > currentWaypoint)
        {
            // Then seek that waypoint
            Seek(waypoints[currentWaypoint]);
            // If we are close enough, then increment to next waypoint
            if (Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) <= waypointStopDistance)
            {
                currentWaypoint++;
            }
        } 
        else if (isLooping == true)
        {
            RestartPatrol();
        }
    }

    public void RestartPatrol()
    {
        // Set the index to 0
        currentWaypoint = 0;
    }

    #endregion
}
