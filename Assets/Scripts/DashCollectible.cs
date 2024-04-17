using UnityEngine;

public class DashCollectible : MonoBehaviour
{
    public AudioClip pickFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.EnableDash();
                AudioSource.PlayClipAtPoint(pickFX, transform.position);
                gameObject.GetComponent<Animator>().SetTrigger("PickUp");
                Destroy(gameObject, 0);
            }
        }
    }
}
