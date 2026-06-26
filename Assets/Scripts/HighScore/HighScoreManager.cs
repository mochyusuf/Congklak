using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using System.Linq;

public class HighScoreManager : MonoBehaviour
{
    public string connectionString;

    public List<HighScore> highScores = new List<HighScore>();

    public int saveScore;

    private string connection;
    private IDbConnection dbcon;
    private IDbCommand dbcmd;
    private IDataReader reader;
    private StringBuilder builder;

    // Table Names
    private string Table_Name = "HighScore";

    private string Player_ID = "PLAYER_ID";
    private string NAME = "NAME";
    private string SCORE = "SCORE";
    private string DATE = "DATE";
    private string Type = "Type";
    private string Board = "Board";

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    //private void Start()
    //{
    //    OpenDB("HighScoreDB");
    //    Create_Table();
    //    GetScores();
    //}

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        OpenDB("HighScoreDB");
        Create_Table();
        GetScores();
    }

    public void OpenDB(string p)
    {
        Debug.Log("Call to OpenDB:" + p);
        // check if file exists in Application.persistentDataPath
        string filepath = Application.persistentDataPath + "/" + p;
        if (!File.Exists(filepath))
        {
            Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +
                             Application.dataPath + "!/assets/" + p + ".sqlite");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p + ".sqlite");
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
        }

        //open db connection
        connectionString = "URI=file:" + filepath + ".sqlite";
        Debug.Log("Stablishing connection to: " + connectionString);
        //dbcon = new SqliteConnection(connectionString);
        //dbcon.Open();
    }

    public void Create_Table()
    {
        string CREATE_TABLE_TODO = "CREATE TABLE if not exists "
            + Table_Name + "(" + Player_ID + " INTEGER PRIMARY KEY AUTOINCREMENT," +
            NAME + " TEXT," +
            SCORE + " INTEGER," +
            DATE + " DATETIME DEFAULT CURRENT_DATE," +
            Type + " TEXT," +
            Board + " INTEGER" + ")";
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = CREATE_TABLE_TODO;

                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    //public void EnterName()
    //{
    //    if (enterName.text != string.Empty)
    //    {
    //        int score = UnityEngine.Random.Range(4000, 5000);
    //        print(enterName.text + "||" + score);
    //        InsertScore(enterName.text, int.Parse(enterBoard.text), enterType.text, score);
    //        enterName.text = string.Empty;

    //        DeleteExtraScore();
    //        //Showscores();
    //    }
    //}

    //// Update is called once per frame
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        nameDialog.SetActive(!nameDialog.activeSelf);
    //    }
    //}

    public List<HighScore> getHighScore(int Board, string Type)
    {
        List<HighScore> High = new List<HighScore>();
        for (int i = 0; i < highScores.Count; i++)
        {
            if (highScores[i].Board == Board && highScores[i].Type == Type)
            {
                High.Add(highScores[i]);
            }
        }
        return High;
    }

    public void InsertScore(string name, int _Board, string _Type, int newScore)
    {
        GetScores();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("INSERT INTO " + Table_Name + "(" + NAME + "," + Board + "," + Type + "," + SCORE + ") VALUES (\"{0}\",\"{1}\",\"{2}\",\"{3}\")", name, _Board, _Type, newScore);

                dbCmd.CommandText = sqlQuery;

                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }

    public int GetMinimalScore(int Board, string Type)
    {
        List<HighScore> High = new List<HighScore>();
        for (int i = 0; i < highScores.Count; i++)
        {
            if (highScores[i].Board == Board && highScores[i].Type == Type)
            {
                High.Add(highScores[i]);
            }
        }
        int minVal = int.MaxValue;

        for (int i = 1; i < High.Count; i++)
        {
            if (High[i].Score < minVal)
            {
                minVal = High[i].Score;
            }
        }
        return minVal;
    }

    public int GetCountScore(int Board, string Type)
    {
        int Temp = 0;
        List<HighScore> High = new List<HighScore>();
        for (int i = 0; i < highScores.Count; i++)
        {
            if (highScores[i].Board == Board && highScores[i].Type == Type)
            {
                High.Add(highScores[i]);
                Temp++;
            }
        }
        return Temp;
    }

    public List<HighScore> GetScoreSpecific(int Board, string Type)
    {
        List<HighScore> High = new List<HighScore>();
        for (int i = 0; i < highScores.Count; i++)
        {
            if (highScores[i].Board == Board && highScores[i].Type == Type)
            {
                High.Add(highScores[i]);
            }
        }
        return High;
    }

    private void GetScores()
    {
        highScores.Clear();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT " + Player_ID + ", " + SCORE + ", " + Board + "," + Type + "," + NAME + "," + DATE + " From HighScore";

                dbCmd.CommandText = sqlQuery;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        highScores.Add(new HighScore(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5)));
                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

        highScores.Sort();
    }

    public void DeleteExtraScore(int Board, string Type)
    {
        GetScores();
        var temp = GetScoreSpecific(Board, Type);

        if (temp.Count > saveScore)
        {
            int deleteCount = (temp.Count) - saveScore;
            print(deleteCount);
            //highScores.Reverse();
            List<HighScore> SortedList = temp.OrderBy(o => o.Score).ToList();

            using (IDbConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    for (int i = 0; i < deleteCount; i++)
                    {
                        string sqlQuery = String.Format("DELETE FROM " + Table_Name + " WHERE " + Player_ID + " = (\"{0}\")", SortedList[i].ID);

                        dbCmd.CommandText = sqlQuery;

                        dbCmd.ExecuteScalar();
                    }
                    dbConnection.Close();
                }
            }
        }
    }
}