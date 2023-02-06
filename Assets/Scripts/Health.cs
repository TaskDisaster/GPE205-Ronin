using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float currentHealth;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, Pawn source)
    {
        currentHealth = currentHealth - amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log(source.name + " did " + amount + " damage to " + gameObject.name);
        
        if (currentHealth <= 0 )
        {
            Die(source);
        }
    }

    public void Die(Pawn source)
    {
        Debug.Log(source.name + " destroyed " + gameObject.name);
        Destroy(gameObject);
    }
}
