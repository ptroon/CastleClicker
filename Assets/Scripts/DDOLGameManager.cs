using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DDOLGameManager : MonoBehaviour
{

    // current difficulty of the game from the meny option.
    public struct Choices
    {
        public int optionValue;
        public string optionName;

        public Choices (int _optionValue, string _optionName)
        {
            this.optionValue = _optionValue;
            this.optionName = _optionName;
        }
    }

    public enum EGameComplexity { Easy=1, Medium, Hard };

    public List<Choices> gameDifficulty;

    public int sessionBestScore = 0;     // best score this session
    public int currentScore = 0;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        gameDifficulty = new List<Choices>();

        Debug.Log("Initialising _Preload..");
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("GameScene"); // load the menu scene as the first scene
    }


    // Update is called once per frame
    void Update()
    {
        
    }


}
