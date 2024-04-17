using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestOpen : MonoBehaviour
{
    public static bool isOpen = false;
    public GameObject Chest2;
    public bool isPlayerClose = false;
    GameObject player;
    GameObject Chest;
    GameObject shield;
    public float dialogueDistance = 3;
    public float distanceToPlayer;
    public AudioClip chestOpenSFX;
    private Transform OGTransform;
    ShieldBehaviour shieldBehaviour;
    public Text chestText;
    PlayerHealth pHealth;
    public float chestCoolDown = 45f;
    bool isChestCoolDown;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pHealth = player.GetComponent<PlayerHealth>();
        shield = GameObject.FindWithTag("Shield");
        if (shield != null)
        {
            shieldBehaviour = shield.GetComponent<ShieldBehaviour>();
        }
        OGTransform = transform;
        isChestCoolDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distanceToPlayer <= dialogueDistance)
        {
            isPlayerClose = true;
        }

        if(distanceToPlayer >= dialogueDistance)
        {
            isPlayerClose = false;
        }

        if(Input.GetKeyDown(KeyCode.Z) && isPlayerClose && !isChestCoolDown)
        {
            isChestCoolDown = true;
            Debug.Log("Chest Opened");
            Open();
        }
    }

    public void Open()
    {
        AudioSource.PlayClipAtPoint(chestOpenSFX, transform.position);
        Chest2.SetActive(true);
        Reward();
        StartCoroutine(WaitAndClose(Chest2));
    }

    private IEnumerator WaitAndClose(GameObject openedChest)
    {
        yield return new WaitForSeconds(1f);
        Close(openedChest);
    }

    public void Close(GameObject openedChest)
    {
        Chest2.SetActive(false);
        Invoke("resetChest", chestCoolDown);
        chestText.text = "Wait 1 min then try open again";
        chestText.color = Color.red;
    }

    void Reward()
    {
        if (Random.Range(0, 2) == 0)
        {
            chestText.text = "You got 30 health!";
            chestText.color = Color.magenta;
            Debug.Log("You got 30 health!");
            pHealth.TakeHealth(20);
        }
        else
        {
            chestText.text = "You got 15 shield!";
            chestText.color = Color.magenta;
            Debug.Log("You got 15 shield!");
            if (shieldBehaviour != null)
            {
                shieldBehaviour.TakeShield(15);
            }
        }
    }

    public void resetChest()
    {
        isChestCoolDown = false;
    }
}
