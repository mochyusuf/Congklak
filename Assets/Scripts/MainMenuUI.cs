using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject[] UI;
    
    public Text Title_Score;

    public GameControllerv2._Type Type_X;

    public GameObject scorePrefab;
    public Transform ParentScore;

    public HighScoreManager highScoreManager;

    public List<HighScore> High;

    private void Awake()
    {
        highScoreManager = FindAnyObjectByType<HighScoreManager>();
    }

    // Use this for initialization
    private void Start()
    {
        Debug.developerConsoleVisible = true;
        Disable();
        UI[0].SetActive(true);
    }

    private void SetHighScore()
    {
        for (int i = 0; i < High.Count; i++)
        {
            if (i <= High.Count - 1)
            {
                GameObject tempObject = Instantiate(scorePrefab);

                HighScore tempScore = High[i];
                tempObject.transform.parent = ParentScore;
                tempObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                tempObject.GetComponent<HighScoreScript>().SetScore(tempScore.Name, tempScore.Score.ToString(), (i + 1).ToString());
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (UI[0].activeInHierarchy == true)
            {
                Exit();
            }
            else if (UI[0].activeInHierarchy == false)
            {
                Back();
            }
        }
    }

    public void Back_Score()
    {
        Disable();
        UI[6].SetActive(true);
        Clear();
    }

    public void Clear()
    {
        High.Clear();
        foreach (Transform child in ParentScore)
        {
            Destroy(child.gameObject);
        }
    }

    public void PVC()
    {
        Type_X = GameControllerv2._Type._PVC;
        Disable();
        UI[4].SetActive(true);
    }

    public void PVP()
    {
        Type_X = GameControllerv2._Type._PVP;
        Disable();
        UI[4].SetActive(true);
    }

    public void Type_7()
    {
        if (Type_X == GameControllerv2._Type._PVC)
        {
            SceneManager.LoadScene("Game.PVC");
        }
        else
        {
            SceneManager.LoadScene("Game.PVP");
        }
    }

    public void Type_6()
    {
        if (Type_X == GameControllerv2._Type._PVC)
        {
            SceneManager.LoadScene("Game.PVC.6");
        }
        else
        {
            SceneManager.LoadScene("Game.PVP.6");
        }
    }

    public void Type_5()
    {
        if (Type_X == GameControllerv2._Type._PVC)
        {
            SceneManager.LoadScene("Game.PVC.5");
        }
        else
        {
            SceneManager.LoadScene("Game.PVP.5");
        }
    }

    public void Score()
    {
        Disable();
        UI[6].SetActive(true);
    }

    public void Help()
    {
        Disable();
        UI[2].SetActive(true);
    }

    public void About()
    {
        Disable();
        UI[3].SetActive(true);
    }

    public void Back()
    {
        Disable();
        UI[0].SetActive(true);
    }

    public void Back_PVP_PVC()
    {
        Disable();
        UI[5].SetActive(true);
    }

    private void Disable()
    {
        for (int i = 0; i < UI.Length; i++)
        {
            UI[i].SetActive(false);
        }
    }

    public void Main()
    {
        Disable();
        UI[5].SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Score_Select(string Input)
    {
        var temp = Input.Split('|');
        string temp1 = temp[0] + " Lubang " + temp[1];
        Title_Score.text = temp1;

        Disable();
        UI[1].SetActive(true);
        High = highScoreManager.getHighScore(int.Parse(temp[0]), temp[1]);
        SetHighScore();
    }
}