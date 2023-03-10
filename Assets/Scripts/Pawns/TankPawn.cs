using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    #region Variables
    public Shooter shooter;
    public GameObject shellPrefab;
    public float fireForce;
    public float damageDone;
    public float lifespan;
    public float fireCooldown = 1.5f;
    #endregion Variables

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        shooter = GetComponent<Shooter>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void MoveForward()
    {
        if (mover != null) 
        { 
        mover.Move(transform.forward, moveSpeed);
        } 
        else
        {
        Debug.LogWarning("Warning: No Mover in TankPawn.MoveForward()!");
        }
    }

    public override void MoveBackward()
    {
        if (mover != null)
        {
        mover.Move(transform.forward, -moveSpeed);
        }
        else
        {
        Debug.LogWarning("Warning: No Mover in TankPawn.MoveBackward()!");
        }
    }

    public override void RotateClockwise()
    {
        if (mover != null)
        {
        mover.Rotate(turnSpeed);
        }
        else
        {
        Debug.LogWarning("Warning: No Mover in TankPawn.RotateClockwise()!");
        }
    }

    public override void RotateCounterClockwise()
    {
        if (mover != null)
        {
        mover.Rotate(-turnSpeed);
        }
        else
        {
        Debug.LogWarning("Warning: No Mover in TankPawn.RotateCounterClockwise()!");
        }
    }

    public override void RotateTowards(Vector3 targetPosition)
    {
        // Vector is location
        // Gets the vector from the pawn to the target
        Vector3 vectorToTarget = targetPosition - transform.position;

        // Quaternion is rotation
        // Gets the rotation from the pawn to the target
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);

        // Sets the rotation of the pawn to the target using the turnSpeed multiplied by delta time
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public override void Shoot()
    {
        shooter.Shoot(shellPrefab, fireForce, damageDone, lifespan);
    }
}
