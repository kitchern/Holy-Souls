using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpellBehavior : MonoBehaviour
{
    PlayerHealth playerHealth;
    ShieldBehaviour shieldBehaviour;
    public int spellDamage = 10;
    GameObject player;
    GameObject shield;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        shield = GameObject.FindWithTag("Shield");
        playerHealth = player.GetComponent<PlayerHealth>();
        if (shield != null)
        {
            shieldBehaviour = shield.GetComponent<ShieldBehaviour>();
        }
        
        transform.LookAt(player.transform);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            if (shieldBehaviour != null)
            {
                shieldBehaviour.TakeDamage(spellDamage);
            }
            Destroy(gameObject); // Destroy the spell immediately upon hitting the shield.
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(spellDamage);
            }
            Destroy(gameObject); // Optionally, destroy the spell on collision with the player too.
        }
    }

    
}
