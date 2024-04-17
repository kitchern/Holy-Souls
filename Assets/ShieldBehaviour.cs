using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBehaviour : MonoBehaviour
{
    public int startingHealth = 30;
    public int currentHealth;
    public Slider healthSlider;
    public bool isShieldBroken;
    public AudioClip shieldBreakSFX;
    public GameObject postProc;
    TakeDamagePost PPtakeDamage;

    // Start is called before the first frame update
    void Start()
    {
        postProc = GameObject.FindWithTag("PostProc");
        PPtakeDamage = postProc.GetComponent<TakeDamagePost>();
        currentHealth = startingHealth;
        isShieldBroken = false;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if(currentHealth > 0)
        {
            PPtakeDamage.TakeDamageNormal();
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
        }
        else if(currentHealth <= 0)
        {
            PPtakeDamage.TakeDamageShield();
            isShieldBroken = true;
            Destroy(gameObject, 1);
            AudioSource.PlayClipAtPoint(shieldBreakSFX, transform.position);
        }

        Debug.Log("shieldBroken: " + currentHealth);
    }

    public void TakeShield(int healthAmount)
    {
        if(currentHealth < 30)
        {
            currentHealth += healthAmount;
            healthSlider.value = Mathf.Clamp(currentHealth, 0, 30);
        }

        Debug.Log("Gained 10 Shield");
    }
}
