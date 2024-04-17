using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    EnemyHealth enemyHealth;
    public int spellDamage = 10;
    GameObject enemy;
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");
        enemyHealth = enemy.GetComponent<EnemyHealth>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyHealth.TakeDamage(spellDamage);
            Destroy(gameObject); 
        }
    }

    public void SetDamage(float damage)
    {
        spellDamage = Mathf.RoundToInt(damage);
    }
}
