using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    #region variables
    public Transform firepointTransform; // Firing point for bullets
    private NoiseMaker noiseMaker;
    #endregion

    // Start is called before the first frame update
    public override void Start()
    {
        noiseMaker = GetComponent<NoiseMaker>();
    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifeSpan)
    {
        // Instantiate our projectile
        GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation);

        // Get the DamageOnHit
        DamageOnHit doh = newShell.GetComponent<DamageOnHit>();

        if (doh != null )
        {
            doh.damageDone = damageDone;
            doh.owner = GetComponent<Pawn>();
        }

        Rigidbody rb = newShell.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(firepointTransform.forward * fireForce);
        }

        // Make some noise
        if (noiseMaker != null)
        {
            noiseMaker.volumeDistance = 20;
        }

        Destroy(newShell, lifeSpan);
    }
}
