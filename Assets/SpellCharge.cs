using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCharge : MonoBehaviour
{

    public GameObject projectilePrefab;
    public GameObject chargedSpellPrefab; 
    public float projectileSpeed = 100f; 
    public AudioClip spellSFX;
    public float maxChargeTime = 5.0f;
    private float currentChargeTime = 0f;
    private bool isCharging = false;
    private Vector3 initialScale;
    public Vector3 maxScale = new Vector3(3f, 3f, 3f);

    void Start()
    {
        initialScale = projectilePrefab.transform.localScale; // Assuming initial scale from the default prefab
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCharging = true;
            currentChargeTime = 0f;
        }

        if (isCharging && currentChargeTime < maxChargeTime)
        {
            currentChargeTime += Time.deltaTime;
            currentChargeTime = Mathf.Min(currentChargeTime, maxChargeTime);
        }

        if (Input.GetKeyUp(KeyCode.C) && isCharging)
        {
            isCharging = false;
            ReleaseSpell();
        }
    }

    void ReleaseSpell()
    {
        GameObject projectile = Instantiate(
            projectilePrefab, 
            transform.position + transform.forward, 
            transform.rotation) as GameObject;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

        float scaleMultiplier = Mathf.Lerp(1f, maxScale.x / initialScale.x, currentChargeTime / maxChargeTime);
        projectile.transform.localScale = new Vector3(initialScale.x * scaleMultiplier, initialScale.y * scaleMultiplier, initialScale.z * scaleMultiplier);

        if (currentChargeTime >= maxChargeTime && projectile.GetComponent<Spell>() != null)
        {
            float chargedDamage = Mathf.Lerp(1.0f, 2.0f, currentChargeTime / maxChargeTime);
            projectile.GetComponent<Spell>().SetDamage(chargedDamage);
        }

        AudioSource.PlayClipAtPoint(spellSFX, transform.position);
        currentChargeTime = 0;
        Destroy(projectile, 0.5f);
    }
}