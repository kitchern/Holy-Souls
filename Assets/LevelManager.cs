using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static GameObject[] pickups;
    public static GameObject[] enemies;
    public Text timerText;
    public Text gameText;
    public Text levelText;
    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;
    public static bool isGameOver = false;
    public string nextLevel;
    public string level1;
    public int enemyCount;
    float countUp;
    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        pickups = GameObject.FindGameObjectsWithTag("Pickup");
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("Enemies left: " + enemies.Length);
        if(enemies.Length <= 0)
        {
            LevelBeat();
        }
        enemyCount = enemies.Length;

        countUp += Time.deltaTime;

        timerText.text = countUp.ToString("f2");
        levelText.text = "Enemies Left: " + enemyCount.ToString();
        
    }


    public void LevelLost()
    {
        isGameOver = true;
        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().pitch = 1;
        AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);

        Invoke("LoadCurrentLevel", 2);
              
    }

    public void LevelBeat()
    {
        isGameOver = true;
        gameText.text = "Level Complete!";
        gameText.gameObject.SetActive(true);

        
        AudioSource.PlayClipAtPoint(gameWonSFX, transform.position);

        Invoke("LoadNextLevel", 2);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
    
    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(level1);
    }
    
}
