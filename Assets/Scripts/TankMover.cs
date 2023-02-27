using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{
    #region Variables
    private Rigidbody rb;
    private NoiseMaker noiseMaker;
    #endregion
    // Start is called before the first frame update
    public override void Start()
    {
        rb = GetComponent<Rigidbody>();     // Get the rigid body
        noiseMaker = GetComponent<NoiseMaker>();    // Get the NoiseMaker
    }

    public override void Move(Vector3 direction, float speed)
    {
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVector);
        if (noiseMaker != null)
        {
            noiseMaker.volumeDistance = 10;
        }
    }

    public override void Rotate(float turnSpeed)
    {
        transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
        if (noiseMaker != null)
        {
            noiseMaker.volumeDistance = 5;
        }

    }
}
