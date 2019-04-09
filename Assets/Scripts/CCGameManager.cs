using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCGameManager : MonoBehaviour
{

    // Components available through inspector
    public Button btn1, btn2, btn3, btn4, btn5, btn6, btn7, castleButton;
    public Scrollbar scrollbar;
    public GameObject explosionPE, firePE;
    public TMP_Text score;
    public GameObject gathered, holder;

    // The dataset for the COST and GOLD PER SECOND/CLICK
    private int[,] Option1Array = new int[,] { { 100, 1 }, { 200, 2 }, { 300, 3 }, { 400, 4 }, { 500, 5 } };    // Infantry
    //private int[,] Option1Array = new int[,] { { 10, 100 }, { 10, 200 }, { 10, 300 }, { 10, 400 }, { 10, 500 } };         // Infantry TESTING
    private int[,] Option2Array = new int[,] { { 1000, 6 }, { 1200, 7 }, { 1400, 8 }, { 1500, 9 } };            // Archers
    private int[,] Option3Array = new int[,] { { 2000, 10 }, { 2500, 12 }, { 3000, 14 }, { 3500, 16 } };        // Knights
    private int[,] Option4Array = new int[,] { { 5000, 17 }, { 6000, 18 }, { 7000, 19 }, { 8000, 20 } };        // Trebuchet
    private int[,] Option5Array = new int[,] { { 10000, 22 }, { 12000, 24 }, { 14000, 26 }, { 16000, 28 } };    // Battering Ram
    private int[,] Option6Array = new int[,] { { 20000, 32 }, { 22000, 34 }, { 24000, 36 }, { 26000, 38 } };    // Ballista
    private int[,] Option7Array = new int[,] { { 50000, 52 }, { 52000, 54 }, { 54000, 56 }, { 56000, 58 } };    // Seige Tower

    int[] currentOptions = new int[] { 0, 0, 0, 0, 0, 0, 0 };    // used to track the current option for each button

    private SoundManager sound;

    private float nextActionTime = 0.0f;
    private float nextActionTimeS = 0f;
    private float period = 1.0f;
    private float savePeriod = 30.0f;
    private float timeLastPlayed = 0f;

    private GameObject explosion_tmp;

    // the amount of gold we are automatically generating per second
    private int goldPerSecond = 0;

    // the amount of gold we collect per click of the castle
    private int goldPerClick = 1;

    // amount of gold we have collected
    private int goldCollected = 0;

    private List<GameObject> textList;
    private UnityEngine.Random random;

    // Start is called before the first frame update
    void Start()
    {
        scrollbar.value = 1f;
        sound = UnityEngine.Object.FindObjectOfType<SoundManager>();

        score.text = goldCollected.ToString();

        // use this to initialise the buttons to their first level of values
        InitialiseButtons();

        textList = new List<GameObject>();      // create a list for holding the text animations.
        random = new UnityEngine.Random();
    }

    private void InitialiseButtons ()
    {
        int cost = 0;
        int psec = 0;
        string tmpTxt = "";

        cost = Option1Array[currentOptions[0], 0];
        psec = Option1Array[currentOptions[0], 1];
        tmpTxt = "Infantry\n" + cost + " Gold\n+" + psec + " Gold p/sec";
        ChangeButtonText(btn1, tmpTxt);

        cost = Option2Array[currentOptions[1], 0];
        psec = Option2Array[currentOptions[1], 1];
        tmpTxt = "Archers\n" + cost + " Gold\n+" + psec + " Gold p/sec";
        ChangeButtonText(btn2, tmpTxt);

        cost = Option3Array[currentOptions[2], 0];
        psec = Option3Array[currentOptions[2], 1];
        tmpTxt = "Knights\n" + cost + " Gold\n+" + psec + " Gold p/sec";
        ChangeButtonText(btn3, tmpTxt);

        cost = Option4Array[currentOptions[3], 0];
        psec = Option4Array[currentOptions[3], 1];
        tmpTxt = "Trebuchet\n" + cost + " Gold\n+" + psec + " Gold p/sec";
        ChangeButtonText(btn4, tmpTxt);

        cost = Option5Array[currentOptions[4], 0];
        psec = Option5Array[currentOptions[4], 1];
        tmpTxt = "Battering Ram\n" + cost + " Gold\n+" + psec + " Gold p/sec";
        ChangeButtonText(btn5, tmpTxt);

        cost = Option6Array[currentOptions[5], 0];
        psec = Option6Array[currentOptions[5], 1];
        tmpTxt = "Ballista\n" + cost + " Gold\n+" + psec + " Gold p/sec";
        ChangeButtonText(btn6, tmpTxt);

        cost = Option7Array[currentOptions[6], 0];
        psec = Option7Array[currentOptions[6], 1];
        tmpTxt = "Siege Tower\n" + cost + " Gold\n+" + psec + " Gold p/sec";
        ChangeButtonText(btn7, tmpTxt);
    }


    public void OnClickButton (string btnName)
    {
        string tmpTxt;
        int cost, psec;
        GameObject textAnimation, explosion;
        float yy, xx;

        yy = UnityEngine.Random.Range(-10, 10);
        xx = UnityEngine.Random.Range(-20, 20);

        switch (btnName)
        {
            case "CastleButton":
                sound.PlayButtonClick();
                UpdateScore();
                textAnimation = Instantiate(holder, new Vector3(xx, yy), transform.rotation);
                textAnimation.SetActive(true);
                textAnimation.GetComponentInChildren<TMP_Text>().fontSize = 4;
                textAnimation.GetComponentInChildren<TMP_Text>().text = goldPerClick + " Gold";
                textAnimation.transform.SetParent(castleButton.transform, false);
                textList.Add(textAnimation);
                break;

            case "OptionButton1":
                if (currentOptions[0] < Option1Array.GetLength(0))
                {
                    if (goldCollected >= Option1Array[currentOptions[0], 0])
                    {
                        goldCollected -= Option1Array[currentOptions[0], 0];    // remove the cost of the item from the total gold collected.
                        goldPerSecond += Option1Array[currentOptions[0], 1];
                        goldPerClick  += Option1Array[currentOptions[0], 1];
                        currentOptions[0]++;
                        if (currentOptions[0] < Option1Array.GetLength(0))      // have we now gone over the length of the array?
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            cost = Option1Array[currentOptions[0], 0];
                            psec = Option1Array[currentOptions[0], 1];
                            tmpTxt = "Infantry\n" + cost + " Gold\n+" + psec + " Gold p/sec";
                            ChangeButtonText(btn1, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        else
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            tmpTxt = "Infantry\nAll done!";
                            ChangeButtonText(btn1, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        SaveValues();
                    }
                }
                break;

            case "OptionButton2":
                if (currentOptions[1] < Option2Array.GetLength(0))
                {
                    if (goldCollected >= Option2Array[currentOptions[1], 0])
                    {
                        goldCollected -= Option2Array[currentOptions[1], 0];    // remove the cost of the item from the total gold collected.
                        goldPerSecond += Option2Array[currentOptions[1], 1];
                        goldPerClick += Option2Array[currentOptions[1], 1];
                        currentOptions[1]++;
                        if (currentOptions[1] < Option2Array.GetLength(0))      // have we now gone over the length of the array?
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            cost = Option2Array[currentOptions[1], 0];
                            psec = Option2Array[currentOptions[1], 1];
                            tmpTxt = "Archers\n" + cost + " Gold\n" + psec + " Gold p/sec";
                            ChangeButtonText(btn2, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        else
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            tmpTxt = "Archers\nAll done!";
                            ChangeButtonText(btn2, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        SaveValues();
                    }
                }
                break;

            case "OptionButton3":
                if (currentOptions[2] < Option3Array.GetLength(0))
                {
                    if (goldCollected >= Option3Array[currentOptions[2], 0])
                    {
                        goldCollected -= Option3Array[currentOptions[2], 0];    // remove the cost of the item from the total gold collected.
                        goldPerSecond += Option3Array[currentOptions[2], 1];
                        goldPerClick += Option3Array[currentOptions[2], 1];
                        currentOptions[2]++;
                        if (currentOptions[2] < Option3Array.GetLength(0))      // have we now gone over the length of the array?
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            cost = Option3Array[currentOptions[2], 0];
                            psec = Option3Array[currentOptions[2], 1];
                            tmpTxt = "Knights\n" + cost + " Gold\n" + psec + " Gold p/sec";
                            ChangeButtonText(btn3, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        else
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            tmpTxt = "Knights\nAll done!";
                            ChangeButtonText(btn3, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        SaveValues();
                    }
                }
                break;

            case "OptionButton4":
                if (currentOptions[3] < Option4Array.GetLength(0))
                {
                    if (goldCollected >= Option4Array[currentOptions[3], 0])
                    {
                        goldCollected -= Option4Array[currentOptions[3], 0];    // remove the cost of the item from the total gold collected.
                        goldPerSecond += Option4Array[currentOptions[3], 1];
                        goldPerClick += Option4Array[currentOptions[3], 1];
                        currentOptions[3]++;
                        if (currentOptions[3] < Option4Array.GetLength(0))      // have we now gone over the length of the array?
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            cost = Option4Array[currentOptions[3], 0];
                            psec = Option4Array[currentOptions[3], 1];
                            tmpTxt = "Trebuchet\n" + cost + " Gold\n" + psec + " Gold p/sec";
                            ChangeButtonText(btn4, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        else
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            tmpTxt = "Trebuchet\nAll done!";
                            ChangeButtonText(btn4, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        SaveValues();
                    }
                }
                break;

            case "OptionButton5":
                if (currentOptions[4] < Option5Array.GetLength(0))
                {
                    if (goldCollected >= Option5Array[currentOptions[4], 0])
                    {
                        goldCollected -= Option5Array[currentOptions[4], 0];    // remove the cost of the item from the total gold collected.
                        goldPerSecond += Option5Array[currentOptions[4], 1];
                        goldPerClick += Option5Array[currentOptions[4], 1];
                        currentOptions[4]++;
                        if (currentOptions[4] < Option5Array.GetLength(0))      // have we now gone over the length of the array?
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            cost = Option5Array[currentOptions[4], 0];
                            psec = Option5Array[currentOptions[4], 1];
                            tmpTxt = "Battering Ram\n" + cost + " Gold\n" + psec + " Gold p/sec";
                            ChangeButtonText(btn5, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        else
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            tmpTxt = "Battering Ram\nAll done!";
                            ChangeButtonText(btn5, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        SaveValues();
                    }
                }
                break;

            case "OptionButton6":
                if (currentOptions[5] < Option6Array.GetLength(0))
                {
                    if (goldCollected >= Option6Array[currentOptions[5], 0])
                    {
                        goldCollected -= Option6Array[currentOptions[5], 0];    // remove the cost of the item from the total gold collected.
                        goldPerSecond += Option6Array[currentOptions[5], 1];
                        goldPerClick += Option6Array[currentOptions[5], 1];
                        currentOptions[5]++;
                        if (currentOptions[5] < Option6Array.GetLength(0))      // have we now gone over the length of the array?
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            cost = Option6Array[currentOptions[5], 0];
                            psec = Option6Array[currentOptions[5], 1];
                            tmpTxt = "Ballista\n" + cost + " Gold\n" + psec + " Gold p/sec";
                            ChangeButtonText(btn6, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        else
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            tmpTxt = "Ballista\nAll done!";
                            ChangeButtonText(btn6, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        SaveValues();
                    }
                }
                break;

            case "OptionButton7":
                if (currentOptions[6] < Option7Array.GetLength(0))
                {
                    if (goldCollected >= Option7Array[currentOptions[6], 0])
                    {

                        goldCollected -= Option7Array[currentOptions[6], 0];    // remove the cost of the item from the total gold collected.
                        goldPerSecond += Option7Array[currentOptions[6], 1];
                        goldPerClick += Option7Array[currentOptions[6], 1];
                        currentOptions[6]++;
                        if (currentOptions[6] < Option7Array.GetLength(0))      // have we now gone over the length of the array?
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            cost = Option7Array[currentOptions[6], 0];
                            psec = Option7Array[currentOptions[6], 1];
                            tmpTxt = "Siege Tower\n" + cost + " Gold\n" + psec + " Gold p/sec";
                            ChangeButtonText(btn7, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        else
                        {
                            sound.GetComponent<SoundManager>().PlayBuySound();
                            tmpTxt = "Seige Tower\nAll done!";
                            ChangeButtonText(btn7, tmpTxt);
                            explosion = Instantiate(explosionPE, new Vector3(xx, 0), transform.rotation);
                            explosion.transform.SetParent(castleButton.transform, false);
                        }
                        SaveValues();
                    }
                }
                break;

            default:
                tmpTxt = "";
                break;
        }
        // Save settings
        PlayerPrefs.Save();
    }

    IEnumerator CleanUp ()
    {
        foreach (GameObject go in textList)
        {
            if (go)
            {
                if (!go.GetComponentInChildren<Animation>().isPlaying)
                {
                    Debug.Log("Destroy in CleanUp");
                    Destroy(go);
                }
            }
        }
        yield return null;
    }

    void ChangeButtonText (Button btn, string btnText)
    {
        btn.GetComponentInChildren<TMPro.TMP_Text>().text = btnText;
    }

    void UpdateScore ()
    {
        goldCollected += goldPerClick;              // update the total amount collected
        score.text = goldCollected.ToString();      // update the score
    }

    void UpdateScorePerSec()
    {
        goldCollected += goldPerSecond;             // update the total amount collected
        score.text = goldCollected.ToString();      // update the score
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            UpdateScorePerSec();
        }

        if (Time.time > nextActionTimeS)
        {
            nextActionTimeS += savePeriod;
            SaveValues();
        }
        StartCoroutine(CleanUp());              // start the CleanUp which checks the objects created.

    }

    void SaveValues ()
    {
        PlayerPrefs.SetInt("GoldCollected", goldCollected);
        PlayerPrefs.SetInt("GoldPerClick", goldPerClick);
        PlayerPrefs.SetInt("GoldPerSecond", goldPerSecond);
        PlayerPrefs.SetString("TimeLastPlayed", DateTime.Now.ToString());
        PlayerPrefs.Save();
        Debug.Log("Saved at " + DateTime.Now.ToString());
    }
}
