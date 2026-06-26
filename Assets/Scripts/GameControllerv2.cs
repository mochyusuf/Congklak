using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerv2 : MonoBehaviour
{
    public HighScoreManager highScoreManager;
    public _UI UI;
    public int[] ResultArray;

    public _Type Type;

    public enum _Type
    {
        _PVP,
        _PVC
    }

    public _BoardType BoardType;

    public enum _BoardType
    {
        _5,
        _6,
        _7
    }

    [System.Serializable]
    public class _UI
    {
        public Image PlayerBank;
        public Button[] PlayerButton;

        public Image ComputerBank;
        public Button[] ComputerButton;

        public GameObject Winner;
        public GameObject Lose;
        public GameObject Tie;
        public GameObject Name_GameObject;
        public Text Name;
        public GameObject Score;
        public GameObject CloseButton;

        public GameObject PlayerBound;
        public GameObject ComputerBound;
        public GameObject ComputerBoundButton;

        public GameObject BoundAll;

        public GameObject Record;
        public InputField InputName;

        public Color PlayerColor;
        public Color ComputerColor;

        public Color PlayerColorAnim;
        public Color ComputerColorAnim;

        public Color ColorAnimTake;

        public Text Turn;
        public GameObject Play;

        public GameObject Restart;
        public GameObject Back;

        public GameObject[] SeedTotal;

        public _DisplaySeedText DisplaySeedText;

        [System.Serializable]
        public class _DisplaySeedText
        {
            public Text PlayerMountain;
            public Text[] PlayerHole;

            public Text ComputerMountain;
            public Text[] ComputerHole;
        }

        public _DisplaySeed DisplaySeed;

        [System.Serializable]
        public class _DisplaySeed
        {
            public GameObject PlayerMountainParent;
            public GameObject[] PlayerHoleParent;

            public GameObject ComputerMountainParent;
            public GameObject[] ComputerHoleParent;
        }

        public GameObject AnnouncementParent;

        public GameObject AnnouncementPrefabs;

        public Text ScoreText;

        public Text Greedy;
    }

    public _Audio Audio;

    [System.Serializable]
    public class _Audio
    {
        public GameObject[] Sound;
        public bool ActiveSound = true;
        public AudioSource AudioSource;
    }

    public int DefaultSeed = 7;
    public int NowSeed;

    public _Turn Turn;

    public enum _Turn
    {
        Player,
        Computer
    }

    public int IndexPlayerMountain;
    public int IndexComputerMountain;
    public int TotalIndex;

    public int[] BoardCongklak;

    private void DisplaySeed(int Index)
    {
        if (Index == IndexComputerMountain)
        {
            //var temp = UI.DisplaySeed.ComputerMountainParent;
            var trans = UI.DisplaySeed.ComputerMountainParent.transform;
            DestroyChild(trans);
            var temp = Instantiate(UI.SeedTotal[BoardCongklak[IndexComputerMountain]], trans);
            temp.transform.localPosition = Vector3.zero;
            //UI.ComputerMountain.sprite = UI.SeedTotal[BoardCongklak[IndexComputerMountain]];
        }
        else if (Index == IndexPlayerMountain)
        {
            var trans = UI.DisplaySeed.PlayerMountainParent.transform;
            DestroyChild(trans);
            var temp = Instantiate(UI.SeedTotal[BoardCongklak[IndexPlayerMountain]], trans);
            temp.transform.localPosition = Vector3.zero;
            //UI.DisplaySeed.PlayerMountain.sprite = UI.SeedTotal[BoardCongklak[IndexPlayerMountain]];
        }
        else if (Index > IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            var trans = UI.DisplaySeed.PlayerHoleParent[Index - (IndexComputerMountain + 1)].transform;
            DestroyChild(trans);
            var temp = Instantiate(UI.SeedTotal[BoardCongklak[Index]], trans);
            temp.transform.localPosition = Vector3.zero;
            //UI.DisplaySeed.PlayerHole[Index - (IndexComputerMountain + 1)].sprite = UI.SeedTotal[BoardCongklak[Index]];
        }
        else if (Index < IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            var trans = UI.DisplaySeed.ComputerHoleParent[Index].transform;
            DestroyChild(trans);
            var temp = Instantiate(UI.SeedTotal[BoardCongklak[Index]], trans);
            temp.transform.localPosition = Vector3.zero;
            //UI.DisplaySeed.ComputerHole[Index].sprite = UI.SeedTotal[BoardCongklak[Index]];
        }
    }

    private void DestroyChild(Transform trans)
    {
        var temp = trans.GetComponentsInChildren<Transform>();
        for (int i = 1; i < temp.Length; i++)
        {
            //print("Destroy:" + temp[i].name);
            Destroy(temp[i].gameObject);
        }
    }

    private void DisplaySeedText(int Index)
    {
        if (Index == IndexComputerMountain)
        {
            UI.DisplaySeedText.ComputerMountain.text = BoardCongklak[IndexComputerMountain].ToString();
        }
        else if (Index == IndexPlayerMountain)
        {
            UI.DisplaySeedText.PlayerMountain.text = BoardCongklak[IndexPlayerMountain].ToString();
        }
        else if (Index > IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            UI.DisplaySeedText.PlayerHole[Index - (IndexComputerMountain + 1)].text = BoardCongklak[Index].ToString();
        }
        else if (Index < IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            UI.DisplaySeedText.ComputerHole[Index].text = BoardCongklak[Index].ToString();
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Back();
        }
    }

    private void Awake()
    {
        highScoreManager = FindAnyObjectByType<HighScoreManager>();
        if (BoardType == _BoardType._7)
        {
            BoardCongklak = new int[16];
            ResultArray = new int[7];
            IndexPlayerMountain = 15;
            IndexComputerMountain = 7;
            TotalIndex = 16;
        }
        else if (BoardType == _BoardType._6)
        {
            BoardCongklak = new int[14];
            ResultArray = new int[6];
            IndexPlayerMountain = 13;
            IndexComputerMountain = 6;
            TotalIndex = 14;
        }
        else if (BoardType == _BoardType._5)
        {
            BoardCongklak = new int[12];
            ResultArray = new int[5];
            IndexPlayerMountain = 11;
            IndexComputerMountain = 5;
            TotalIndex = 12;
        }
    }

    // Use this for initialization
    private void Start()
    {
        InitData();
        UpdateUIAll();
        Turn = _Turn.Player;
        //ChangeTurn();
        ChangeComputerBoundButton();
        PlaySound(true);
    }

    private IEnumerator Announcement(string Announ)
    {
        var temp = Instantiate(UI.AnnouncementPrefabs);
        temp.GetComponentInChildren<Text>().text = Announ;
        temp.transform.parent = UI.AnnouncementParent.transform;
        temp.transform.localPosition = new Vector3(0, 0, 0);
        print(Announ);
        Destroy(temp, 1.5f);
        yield return null;
    }

    public IEnumerator PlayerAnim(int Index)
    {
        if (Index == IndexComputerMountain)
        {
            UI.ComputerBank.GetComponent<Image>().color = UI.PlayerColorAnim;
            yield return new WaitForSecondsRealtime(0.5f);
            UI.ComputerBank.GetComponent<Image>().color = UI.PlayerColor;
        }
        else if (Index == IndexPlayerMountain)
        {
            UI.PlayerBank.GetComponent<Image>().color = UI.PlayerColorAnim;
            yield return new WaitForSecondsRealtime(0.5f);
            UI.PlayerBank.GetComponent<Image>().color = UI.PlayerColor;
        }
        else if (Index > IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            UI.PlayerButton[Index - (IndexComputerMountain + 1)].GetComponent<Image>().color = UI.PlayerColorAnim;
            yield return new WaitForSecondsRealtime(0.5f);
            UI.PlayerButton[Index - (IndexComputerMountain + 1)].GetComponent<Image>().color = UI.PlayerColor;
        }
        else if (Index < IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            UI.ComputerButton[Index].GetComponent<Image>().color = UI.PlayerColorAnim;
            yield return new WaitForSecondsRealtime(0.5f);
            UI.ComputerButton[Index].GetComponent<Image>().color = UI.PlayerColor;
        }
    }

    public IEnumerator AnimTake(int Index)
    {
        if (Index == IndexComputerMountain)
        {
            UI.ComputerBank.GetComponent<Image>().color = UI.ColorAnimTake;
            yield return new WaitForSecondsRealtime(0.5f);
            if (Turn == _Turn.Computer)
            {
                UI.ComputerBank.GetComponent<Image>().color = UI.ComputerColor;
            }
            else if (Turn == _Turn.Player)
            {
                UI.ComputerBank.GetComponent<Image>().color = UI.PlayerColor;
            }
        }
        else if (Index == IndexPlayerMountain)
        {
            UI.PlayerBank.GetComponent<Image>().color = UI.ColorAnimTake;
            yield return new WaitForSecondsRealtime(0.5f);
            if (Turn == _Turn.Computer)
            {
                UI.PlayerBank.GetComponent<Image>().color = UI.ComputerColor;
            }
            else if (Turn == _Turn.Player)
            {
                UI.PlayerBank.GetComponent<Image>().color = UI.PlayerColor;
            }
        }
        else if (Index > IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            UI.PlayerButton[Index - (IndexComputerMountain + 1)].GetComponent<Image>().color = UI.ColorAnimTake;
            yield return new WaitForSecondsRealtime(0.5f);
            if (Turn == _Turn.Computer)
            {
                UI.PlayerButton[Index - (IndexComputerMountain + 1)].GetComponent<Image>().color = UI.ComputerColor;
            }
            else if (Turn == _Turn.Player)
            {
                UI.PlayerButton[Index - (IndexComputerMountain + 1)].GetComponent<Image>().color = UI.PlayerColor;
            }
        }
        else if (Index < IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            UI.ComputerButton[Index].GetComponent<Image>().color = UI.ColorAnimTake;
            yield return new WaitForSecondsRealtime(0.5f);
            if (Turn == _Turn.Computer)
            {
                UI.ComputerButton[Index].GetComponent<Image>().color = UI.ComputerColor;
            }
            else if (Turn == _Turn.Player)
            {
                UI.ComputerButton[Index].GetComponent<Image>().color = UI.PlayerColor;
            }
        }
    }

    public IEnumerator ComputerAnim(int Index)
    {
        if (Index == IndexComputerMountain)
        {
            UI.ComputerBank.GetComponent<Image>().color = UI.ComputerColorAnim;
            yield return new WaitForSecondsRealtime(0.5f);
            UI.ComputerBank.GetComponent<Image>().color = UI.ComputerColor;
        }
        else if (Index == IndexPlayerMountain)
        {
            UI.PlayerBank.GetComponent<Image>().color = UI.ComputerColorAnim;
            yield return new WaitForSecondsRealtime(0.5f);
            UI.PlayerBank.GetComponent<Image>().color = UI.ComputerColor;
        }
        else if (Index > IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            UI.PlayerButton[Index - (IndexComputerMountain + 1)].GetComponent<Image>().color = UI.ComputerColorAnim;
            yield return new WaitForSecondsRealtime(0.5f);
            UI.PlayerButton[Index - (IndexComputerMountain + 1)].GetComponent<Image>().color = UI.ComputerColor;
        }
        else if (Index < IndexComputerMountain && Index != IndexComputerMountain && Index != IndexPlayerMountain)
        {
            UI.ComputerButton[Index].GetComponent<Image>().color = UI.ComputerColorAnim;
            yield return new WaitForSecondsRealtime(0.5f);
            UI.ComputerButton[Index].GetComponent<Image>().color = UI.ComputerColor;
        }
    }

    public void Sound()
    {
        Audio.ActiveSound = !Audio.ActiveSound;
        if (Audio.ActiveSound == true)
        {
            Audio.Sound[0].SetActive(true);
            Audio.Sound[1].SetActive(false);
        }
        else
        {
            Audio.Sound[0].SetActive(false);
            Audio.Sound[1].SetActive(true);
        }
        PlaySound(Audio.ActiveSound);
    }

    private void PlaySound(bool Sound)
    {
        if (Sound == true)
        {
            Audio.AudioSource.Play();
        }
        else
        {
            Audio.AudioSource.Pause();
        }
    }

    public void Play()
    {
        UI.PlayerBound.SetActive(false);
        UI.ComputerBound.SetActive(true);
        UI.Play.SetActive(false);
        Turn = _Turn.Player;
        var temp = ChangeTurn();
    }

    public void NewRecord()
    {
        if (UI.InputName.text != string.Empty)
        {
            string temp_1;
            if (Type == _Type._PVP)
            {
                temp_1 = "PVP";
            }
            else
            {
                temp_1 = "PVC";
            }
            int temp_2 = 0;
            if (BoardType == _BoardType._5)
            {
                temp_2 = 5;
            }
            else if (BoardType == _BoardType._6)
            {
                temp_2 = 6;
            }
            else if (BoardType == _BoardType._7)
            {
                temp_2 = 7;
            }
            int score = 0;
            if (Type == _Type._PVC)
            {
                score = BoardCongklak[IndexPlayerMountain];
            }
            else
            {
                if (BoardCongklak[IndexComputerMountain] > BoardCongklak[IndexPlayerMountain])
                {
                    score = BoardCongklak[IndexComputerMountain];
                }
                else if (BoardCongklak[IndexComputerMountain] < BoardCongklak[IndexPlayerMountain])
                {
                    score = BoardCongklak[IndexPlayerMountain];
                }
            }
            highScoreManager.InsertScore(UI.InputName.text, temp_2, temp_1, score);
            print(UI.InputName.text + "||" + score);
            highScoreManager.DeleteExtraScore(temp_2, temp_1);

            SceneManager.LoadScene("Main.Menu");
        }
    }

    public bool ChangeTurn()
    {
        if (BoardType == _BoardType._7)
        {
            if (BoardCongklak[IndexComputerMountain] + BoardCongklak[IndexPlayerMountain] == (7 * 2 * 7))
            {
                CheckWinner();
                return false;
            }
            else if (BoardCongklak[0] == 0 && BoardCongklak[1] == 0 && BoardCongklak[2] == 0 && BoardCongklak[3] == 0 && BoardCongklak[4] == 0 && BoardCongklak[5] == 0 && BoardCongklak[6] == 0)
            {
                CheckWinner();
                return false;
            }
            else if (BoardCongklak[8] == 0 && BoardCongklak[9] == 0 && BoardCongklak[10] == 0 && BoardCongklak[11] == 0 && BoardCongklak[12] == 0 && BoardCongklak[13] == 0 && BoardCongklak[14] == 0)
            {
                CheckWinner();
                return false;
            }
        }
        else if (BoardType == _BoardType._6)
        {
            if (BoardCongklak[IndexComputerMountain] + BoardCongklak[IndexPlayerMountain] == (6 * 2 * 7))
            {
                CheckWinner();
                return false;
            }
            else if (BoardCongklak[0] == 0 && BoardCongklak[1] == 0 && BoardCongklak[2] == 0 && BoardCongklak[3] == 0 && BoardCongklak[4] == 0 && BoardCongklak[5] == 0)
            {
                CheckWinner();
                return false;
            }
            else if (BoardCongklak[7] == 0 && BoardCongklak[8] == 0 && BoardCongklak[9] == 0 && BoardCongklak[10] == 0 && BoardCongklak[11] == 0 && BoardCongklak[12] == 0)
            {
                CheckWinner();
                return false;
            }
        }
        else if (BoardType == _BoardType._5)
        {
            if (BoardCongklak[IndexComputerMountain] + BoardCongklak[IndexPlayerMountain] == (5 * 2 * 7))
            {
                CheckWinner();
                return false;
            }
            else if (BoardCongklak[0] == 0 && BoardCongklak[1] == 0 && BoardCongklak[2] == 0 && BoardCongklak[3] == 0 && BoardCongklak[4] == 0)
            {
                CheckWinner();
                return false;
            }
            else if (BoardCongklak[6] == 0 && BoardCongklak[7] == 0 && BoardCongklak[8] == 0 && BoardCongklak[9] == 0 && BoardCongklak[10] == 0)
            {
                CheckWinner();
                return false;
            }
        }

        if (Turn == _Turn.Player)
        {
            UI.PlayerBound.SetActive(false);
            UI.ComputerBound.SetActive(true);
            if (Type == _Type._PVP)
            {
                UI.Turn.text = "Pemain Ke 1";
            }
            else
            {
                UI.Turn.text = "Pemain";
            }
            ChangeComputerBoundButton();
        }
        else if (Turn == _Turn.Computer)
        {
            UI.PlayerBound.SetActive(true);
            UI.ComputerBound.SetActive(false);
            if (Type == _Type._PVP)
            {
                UI.ComputerBound.SetActive(false);
                UI.Turn.text = "Pemain Ke 2";
            }
            else
            {
                UI.ComputerBound.SetActive(true);
                UI.Turn.text = "Komputer";
            }
            ChangeComputerBoundButton();
        }
        return true;
    }

    private void CheckWinner()
    {
        if (BoardCongklak[IndexComputerMountain] > BoardCongklak[IndexPlayerMountain])
        {
            if (Type == _Type._PVC)
            {
                UI.Lose.SetActive(true);
                UI.Name.text = "Komputer";
                Debug.Log("KOMPUTER MENANG");
                UI.Score.SetActive(true);
            }
            else
            {
                UI.Winner.SetActive(true);
                UI.Name.text = "Pemain 2";
                Debug.Log("KOMPUTER MENANG");
                UI.Score.SetActive(true);
            }
        }
        else if (BoardCongklak[IndexComputerMountain] == BoardCongklak[IndexPlayerMountain])
        {
            UI.Tie.SetActive(true);
            UI.Name.text = "Tidak Ada Pemenang";
            Debug.Log("Seri");
        }
        else if (BoardCongklak[IndexComputerMountain] < BoardCongklak[IndexPlayerMountain])
        {
            if (Type == _Type._PVC)
            {
                UI.Winner.SetActive(true);
                UI.Name.text = "Pemain";
                UI.Score.SetActive(true);
                Debug.Log("AKU MENANG");
            }
            else
            {
                UI.Winner.SetActive(true);
                UI.Name.text = "Pemain 1";
                UI.Score.SetActive(true);
                Debug.Log("AKU MENANG");
            }
        }
        UI.Name_GameObject.SetActive(true);
        UI.BoundAll.SetActive(true);
        if (Type == _Type._PVC)
        {
            UI.ScoreText.text = BoardCongklak[IndexPlayerMountain].ToString();
        }
        else
        {
            if (BoardCongklak[IndexComputerMountain] > BoardCongklak[IndexPlayerMountain])
            {
                UI.ScoreText.text = BoardCongklak[IndexComputerMountain].ToString();
            }
            else if (BoardCongklak[IndexComputerMountain] < BoardCongklak[IndexPlayerMountain])
            {
                UI.ScoreText.text = BoardCongklak[IndexPlayerMountain].ToString();
            }
        }
        var temp = CheckRecord();
        if (temp == true)
        {
            UI.Record.SetActive(true);
        }
        else
        {
            UI.CloseButton.SetActive(true);
        }
    }

    public bool CheckRecord()
    {
        bool temp = false;
        string temp_1;
        if (Type == _Type._PVP)
        {
            temp_1 = "PVP";
        }
        else
        {
            temp_1 = "PVC";
        }
        int temp_2 = 0;
        if (BoardType == _BoardType._5)
        {
            temp_2 = 5;
        }
        else if (BoardType == _BoardType._6)
        {
            temp_2 = 6;
        }
        else if (BoardType == _BoardType._7)
        {
            temp_2 = 7;
        }
        int temp_min = highScoreManager.GetMinimalScore(temp_2, temp_1);
        print(temp_min);
        if (highScoreManager.GetCountScore(temp_2, temp_1) < highScoreManager.saveScore)
        {
            return true;
        }
        else
        {
            if (Type == _Type._PVC)
            {
                if (BoardCongklak[IndexPlayerMountain] > temp_min)
                {
                    return true;
                }
            }
            else
            {
                if (BoardCongklak[IndexComputerMountain] > BoardCongklak[IndexPlayerMountain])
                {
                    if (BoardCongklak[IndexComputerMountain] > temp_min)
                    {
                        return true;
                    }
                }
                else if (BoardCongklak[IndexComputerMountain] < BoardCongklak[IndexPlayerMountain])
                {
                    if (BoardCongklak[IndexPlayerMountain] > temp_min)
                    {
                        return true;
                    }
                }
            }
        }
        if (BoardCongklak[IndexComputerMountain] == BoardCongklak[IndexPlayerMountain])
        {
            return false;
        }
        return temp;
    }

    public void Restart()
    {
        UI.BoundAll.SetActive(true);
        UI.Restart.SetActive(true);
    }

    public void Yes_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void No_Restart()
    {
        UI.BoundAll.SetActive(false);
        UI.Restart.SetActive(false);
    }

    public void Back()
    {
        UI.BoundAll.SetActive(true);
        UI.Back.SetActive(true);
    }

    public void Yes_Back()
    {
        SceneManager.LoadScene("Main.Menu");
    }

    public void No_Back()
    {
        UI.BoundAll.SetActive(false);
        UI.Back.SetActive(false);
    }

    private void ChangeComputerBoundButton()
    {
        if (Type == _Type._PVC && Turn == _Turn.Computer)
        {
            UI.ComputerBoundButton.SetActive(false);
        }
        else if (Type == _Type._PVC && Turn == _Turn.Player)
        {
            UI.ComputerBoundButton.SetActive(true);
        }
        else if (Type == _Type._PVP)
        {
            UI.ComputerBoundButton.SetActive(true);
        }
    }

    public IEnumerator ProcessPlayer(int Index)
    {
        UI.PlayerBound.SetActive(true);
        int TempSeed;
        int Counter;
        while (BoardCongklak[Index] != 0)
        {
            TempSeed = BoardCongklak[Index];

            BoardCongklak[Index] = 0;
            Counter = 0;
            yield return StartCoroutine(AnimTake(Index));
            while (Counter < TempSeed)
            {
                Counter++;

                //Periksa apakah telah sampai ke gunung pemain
                if ((Index + Counter) % TotalIndex == IndexComputerMountain)
                {
                    BoardCongklak[(Index + Counter) % TotalIndex + 1]++;
                    UpdateUIAll();
                    yield return StartCoroutine(PlayerAnim((Index + Counter) % TotalIndex + 1));
                    Index++;
                }
                else
                {
                    BoardCongklak[(Index + Counter) % TotalIndex]++;
                    UpdateUIAll();
                    int TempIndex = ((Index + Counter) % TotalIndex);
                    if (TempIndex > IndexComputerMountain && TempIndex < IndexPlayerMountain && BoardCongklak[TempIndex] != 0 && BoardCongklak[(TempIndex - ((TempIndex - IndexComputerMountain) * 2))] != 0 && Counter == (TempSeed))
                    {
                        yield return StartCoroutine(AnimTake((Index + Counter) % TotalIndex));
                    }
                    else
                    {
                        yield return StartCoroutine(PlayerAnim((Index + Counter) % TotalIndex));
                    }
                }
            }
            Index = (Index + Counter) % TotalIndex;

            if (Index == IndexPlayerMountain)
            {
                break;
            }
            else if (BoardCongklak[Index] == 1)
            {
                break;
            }
        }

        if (Index <= IndexComputerMountain - 1)
        {
            //User = _User.Player;
            Turn = _Turn.Computer;
            var temp = ChangeTurn();
            UpdateUIAll();
        }
        else if (Index > IndexComputerMountain && Index < IndexPlayerMountain && BoardCongklak[Index] != 0 && BoardCongklak[(Index - ((Index - IndexComputerMountain) * 2))] != 0)
        { //nembak
            //print("Tembak" + Index + "||" + (TotalIndex - (Index + 2)));
            if (Type == _Type._PVP)
            {
                StartCoroutine(Announcement("Pemain ke 1 Menembak"));
            }
            else
            {
                StartCoroutine(Announcement("Pemain Menembak"));
            }
            BoardCongklak[IndexPlayerMountain] += BoardCongklak[TotalIndex - (Index + 2)] + 1;
            UpdateUIAll();
            yield return StartCoroutine(PlayerAnim(IndexPlayerMountain));
            BoardCongklak[Index] = 0;
            UpdateUIAll();
            yield return StartCoroutine(AnimTake(Index));
            BoardCongklak[TotalIndex - (Index + 2)] = 0;
            UpdateUIAll();
            yield return StartCoroutine(AnimTake(TotalIndex - (Index + 2)));
            //User = _User.Player;
            Turn = _Turn.Computer;
            var temp = ChangeTurn();
            UpdateUIAll();
        }
        else if (Index == IndexPlayerMountain)
        {
            Turn = _Turn.Player;
            if (ChangeTurn() == true)
            {
                if (Type == _Type._PVP)
                {
                    StartCoroutine(Announcement("Giliran Pemain ke 1 berlanjut"));
                }
                else
                {
                    StartCoroutine(Announcement("Giliran Pemain berlanjut"));
                }
            }
            UpdateUIAll();
        }
        else if (Index > IndexComputerMountain && Index < IndexPlayerMountain && BoardCongklak[Index] == 0)
        { //Lubang Kosong
            Debug.Log("Pilih lagi " + Index);
            UI.PlayerBound.SetActive(false);
        }
        else
        {
            Turn = _Turn.Computer;
            ChangeTurn();
        }
        UpdateUIAll();
        yield return null;
    }

    public IEnumerator ProcessComputer(int Index)
    {
        UI.ComputerBoundButton.SetActive(true);
        UI.ComputerBound.SetActive(true);
        int[] BoardSave = new int[TotalIndex];
        int Result = 0;
        for (int x = 0; x < TotalIndex; x++)
        {
            BoardSave[x] = BoardCongklak[x];
        }

        int Choice = 0;
        int TempSeed;
        int Counter;
        int temp = 0;

        if (Type == _Type._PVC)
        {
            //Cari lubang pilihan
            int Greedy = GreedyAlgorithm(BoardSave);                                              //Untuk scanning jadi tau langkah mana yang harus diambil
            temp = Greedy;
        }
        else
        {
            temp = Index;
        }

        Choice = temp;
        //print("pilihan musuh = " + (Choice + 1) +);
        while (BoardCongklak[Choice] != 0)
        {
            TempSeed = BoardCongklak[Choice];
            BoardCongklak[Choice] = 0;
            Counter = 0;
            yield return StartCoroutine(AnimTake(Choice));
            while (Counter < TempSeed)
            {
                Counter++;
                //Periksa apakah telah sampai ke gunung AI
                if ((Choice + Counter) % TotalIndex == IndexPlayerMountain)
                {
                    //counter++;
                    BoardCongklak[(Choice + Counter + 1) % TotalIndex]++;
                    UpdateUIAll();
                    yield return StartCoroutine(ComputerAnim((Choice + Counter + 1) % TotalIndex));
                    Choice++;
                }
                else
                {
                    BoardCongklak[(Choice + Counter) % TotalIndex]++;
                    UpdateUIAll();
                    int TempChoice = (Choice + Counter) % TotalIndex;
                    if (TempChoice < IndexComputerMountain && BoardCongklak[TempChoice] != 0 && BoardCongklak[(TempChoice + ((IndexComputerMountain - TempChoice) * 2))] != 0 && Counter == (TempSeed))
                    {
                        yield return StartCoroutine(AnimTake((Choice + Counter) % TotalIndex));
                    }
                    else
                    {
                        yield return StartCoroutine(ComputerAnim((Choice + Counter) % TotalIndex));
                    }
                }
            }
            Choice = (Choice + Counter) % TotalIndex;
            if (Choice == IndexComputerMountain)
            {
                break;
            }
            else if (BoardCongklak[Choice] == 1)
            {
                break;
            }
        }
        Result = BoardCongklak[IndexComputerMountain];
        ResultArray[temp] = Result;

        if (Choice > IndexComputerMountain)
        {
            Turn = _Turn.Player;
            var temp1 = ChangeTurn();
            UpdateUIAll();
        }
        else if (Choice < IndexComputerMountain && BoardCongklak[Choice] != 0 && BoardCongklak[(Choice + ((IndexComputerMountain - Choice) * 2))] != 0)
        { //nembak
            if (Type == _Type._PVP)
            {
                StartCoroutine(Announcement("Pemain ke 2 Menembak"));
            }
            else
            {
                StartCoroutine(Announcement("Komputer Menembak"));
            }
            BoardCongklak[IndexComputerMountain] += BoardCongklak[TotalIndex - (Choice + 2)] + 1;
            UpdateUIAll();
            yield return StartCoroutine(ComputerAnim((IndexComputerMountain)));
            BoardCongklak[Choice] = 0;
            UpdateUIAll();
            yield return StartCoroutine(AnimTake(Choice));
            BoardCongklak[TotalIndex - (Choice + 2)] = 0;
            UpdateUIAll();
            yield return StartCoroutine(AnimTake(TotalIndex - (Choice + 2)));
            Turn = _Turn.Player;
            var temp1 = ChangeTurn();
            UpdateUIAll();
        }
        else if (Choice == IndexComputerMountain)
        {
            Turn = _Turn.Computer;
            if (ChangeTurn() == true)
            {
                if (Type == _Type._PVP)
                {
                    StartCoroutine(Announcement("Giliran Pemain ke 2 berlanjut"));
                }
                else
                {
                    StartCoroutine(Announcement("Giliran Komputer berlanjut"));
                }
                UpdateUIAll();
                if (Type == _Type._PVC)
                {
                    ClickComputer(-1);
                }
            }
            UpdateUIAll();
        }
        else if (Choice < IndexComputerMountain && BoardCongklak[Choice] == 0)
        {
            Debug.Log("Coba Lagi");
            UI.ComputerBound.SetActive(false);
            //Turn = _Turn.Player;
            //ChangeTurn();
        }
        else
        {
            Turn = _Turn.Player;
            ChangeTurn();
        }

        UpdateUIAll();
        yield return null;
    }

    public void ClickPlayer(int Index)
    {
        StartCoroutine(ProcessPlayer(Index));
    }

    public void ClickPlayer2(int Index)
    {
        if (Type == _Type._PVP)
        {
            ClickComputer(Index);
        }
        else if (Type == _Type._PVC)
        {
            //ClickComputer(-1);
        }
    }

    public void ClickComputer(int Index)
    {
        StartCoroutine(ProcessComputer(Index));
    }

    private void InitData()
    {
        BoardCongklak[IndexComputerMountain] = 0;
        BoardCongklak[IndexPlayerMountain] = 0;
        for (int x = 0; x < TotalIndex; x++)
        {
            if ((x != IndexComputerMountain) && (x != IndexPlayerMountain))
            {
                BoardCongklak[x] = DefaultSeed;
            }
        }
    }

    public void OkComplete()
    {
        SceneManager.LoadScene("Main.Menu");
    }

    private void UpdateUIAll()
    {
        for (int i = 0; i < BoardCongklak.Length; i++)
        {
            DisplaySeed(i);
            DisplaySeedText(i);
        }
    }

    public int GreedyAlgorithm(int[] BoardSave)
    {
        int Choice;
        int Location;
        int TempSeed;
        int Counter;
        int Result;
        int Hole = 0;
        int MaxSeed;
        for (int y = 0; y < IndexComputerMountain; y++)
        {
            for (int x = 0; x < TotalIndex; x++)
            {
                BoardCongklak[x] = BoardSave[x];
            }

            Choice = y;
            Location = Choice;
            while (BoardCongklak[Location] != 0)
            {
                TempSeed = BoardCongklak[Location];
                BoardCongklak[Location] = 0;
                Counter = 0;
                while (Counter < TempSeed)
                {
                    Counter++;
                    if ((Location + Counter) % TotalIndex == IndexPlayerMountain)
                    {
                        BoardCongklak[(Location + Counter + 1) % TotalIndex]++;
                        Location++;
                    }
                    else
                    {
                        BoardCongklak[(Location + Counter) % TotalIndex]++;
                    }
                }
                Location = (Location + Counter) % TotalIndex;
                if (Location == IndexComputerMountain)
                    break;
                else if (BoardCongklak[Location] == 1)
                    break;
            }

            Result = BoardCongklak[IndexComputerMountain];
            if (BoardSave[y] == 0)
                Result = 0;

            print("Lubang ke :" + (Choice + 1) + "||Hasil :" + Result);
            ResultArray[y] = Result;

            if (Location < IndexComputerMountain && BoardCongklak[Location] != 0 && BoardCongklak[(Location + ((IndexComputerMountain - Location) * 2))] != 0)
            {
                BoardCongklak[IndexComputerMountain] += BoardCongklak[TotalIndex - (Location + 2)] + 1;
                BoardCongklak[Location] = 0;
                BoardCongklak[TotalIndex - (Location + 2)] = 0;
                Debug.Log("Lubang ke :" + (Choice + 1) + " ||prediksi isi lumbung setelah nembak :" + BoardCongklak[IndexComputerMountain]);
                ResultArray[y] = BoardCongklak[IndexComputerMountain];
            }
        }

        MaxSeed = Max(ResultArray, out Hole);
        Debug.Log("Max Biji : " + MaxSeed);
        for (int x = 0; x < TotalIndex; x++)
            BoardCongklak[x] = BoardSave[x];
        Debug.Log("Hasil terbaik Lubang ke : " + (Hole + 1));
        ViewGreedy(ResultArray, Hole);
        return Hole;
    }

    public void ViewGreedy(int[] Result, int Max)
    {
        string temp = "Algoritma Greedy";
        string temp1 = null;
        for (int i = 0; i < Result.Length; i++)
        {
            temp1 += "Lubang " + (i + 1) + " || Hasil  " + Result[i] + Environment.NewLine;
        }

        UI.Greedy.text = temp + Environment.NewLine + temp1 + Environment.NewLine + "Hasil terbaik Lubang " + (Max + 1);
    }

    public static int Max(int[] arr, out int index)
    {
        index = -1;
        int max = Int32.MinValue;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > max)
            {
                max = arr[i];
                index = i;
            }
        }
        return max;
    }
}