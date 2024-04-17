using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 10;

    public float minDistance = 2;

    public int damageAmount = 20;
    public float attackDistance = 5;
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        

        if(distance <= attackDistance)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                var enemyHealth = GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(10);
            }
        }
    }


}
