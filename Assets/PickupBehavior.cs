using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupBehavior : MonoBehaviour
{
    public Text ScoreText;
    public static int pickupCount;
    public static int scoreValue = 0;
    public AudioClip pickupSFX;
    public AudioClip pickup2SFX;
    public int healthAmount = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, 90 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) 
    {


            Camera.main.GetComponent<AudioSource>().pitch = 1;
            if(other.CompareTag("Player")) 
            {
                gameObject.SetActive(false);
                AudioSource.PlayClipAtPoint(pickup2SFX, Camera.main.transform.position);
                var playerHealth = other.GetComponent<PlayerHealth>();
                playerHealth.TakeHealth(healthAmount);
                Destroy(gameObject, 0.5f);
                
            }
        
    }

}
