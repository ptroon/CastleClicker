using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zebaroo;

public class MenuSceneManager : MonoBehaviour
{


    private MenuHandler menuH;
    private DDOLGameManager game;

    public GameObject canvas;
    public TMPro.TMP_Text bestScore;

    private void Awake()
    {
        // init objects
        menuH   = Object.FindObjectOfType<MenuHandler>();
        game    = Object.FindObjectOfType<DDOLGameManager>();

        // run functions to draw / build screen

        Time.timeScale = 1f;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("checking scores.." + game.currentScore);
        if (game.sessionBestScore < game.currentScore) { game.sessionBestScore = game.currentScore; }
        bestScore.text = "TOP SCORE: "+ game.sessionBestScore.ToString();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            menuH.QuitGame();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // go to game
            StartGame();
        }
    }

    public void StartGame ()
    {
        /*
        if (Object.FindObjectOfType<MainSceneManager>())
        {
            Destroy(Object.FindObjectOfType<MainSceneManager>());   // destroy MainSceneManager to ensure there is a new copy.
        }
        */
        menuH.LoadScene("MainScene");

    }

    // so we can call this from a button click event
    public void QuitGame ()
    {
        menuH.QuitGame();
    }

    public void ToggleCredits ()
    {
        menuH.ToggleScreen(canvas);
    }

}
