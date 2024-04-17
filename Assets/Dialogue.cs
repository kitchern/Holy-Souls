using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Dialogue : MonoBehaviour
{
    public static bool isDialogue = false;
    public GameObject DialogueBox;
    public bool isPlayerClose = false;
    GameObject player;
    GameObject npc;
    public float dialogueDistance = 3;
    public float distanceToPlayer;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        npc = GameObject.FindWithTag("NPC");
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(npc.transform.position, player.transform.position);
        if(distanceToPlayer <= dialogueDistance)
        {
            isPlayerClose = true;
        }

        if(distanceToPlayer >= dialogueDistance)
        {
            isPlayerClose = false;
        }

        if(Input.GetKeyDown(KeyCode.F) && isPlayerClose)
        {
            Debug.Log("Paused");
            if(isDialogue)
            {
                EndTalk();
            }
            else
            {
                Talk();
            }
        }
    }

    public void Talk()
    {
        isDialogue = true;
        DialogueBox.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EndTalk()
    {
        isDialogue = false;
        DialogueBox.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
