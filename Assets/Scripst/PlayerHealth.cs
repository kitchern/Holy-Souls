using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update

    public int startingHealth = 100;

    public AudioClip deadSFX;

    int currentHealth;

    public Slider healthSlider;


    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
        } 
        if (currentHealth <= 0)
        {
            PlayerDies();
        }

        Debug.Log("player health " + currentHealth);
    }

    public void TakeHealth(int healthAmount)
    {
        if(currentHealth < 100)
        {
            currentHealth += healthAmount;
            healthSlider.value = Mathf.Clamp(currentHealth, 0, 100);
        }

        Debug.Log("CurrentHealth with loot: " + currentHealth);
    }

    void PlayerDies()
    {
        Debug.Log("You Died");
        AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        transform.Rotate(-90, 0, 0, Space.Self);
        FindObjectOfType<LevelManager>().LevelLost();
    }
}
