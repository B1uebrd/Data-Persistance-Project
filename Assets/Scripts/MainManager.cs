using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.IO;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager mainM;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text gameBestScore;
    public int newScore;
    public string newName;
    public int noScore;
    public int oldScore;

    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        LoadName();
        if (MenuManager.MM.username == newName)
        {
        LoadScore();
        oldScore = newScore;
        SaveOldScore();
        gameBestScore.text = "Best Score : " + MenuManager.MM.username + " : " + newScore;
        }
        else 
        {
            oldScore = noScore;
            SaveOldScore();
        gameBestScore.text = "Best Score : " + MenuManager.MM.username + " : " + noScore;
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        newName = MenuManager.MM.username;

        LoadOldScore();
        if (m_Points > oldScore)
        {
            newScore = m_Points;
            gameBestScore.text = "Best Score : " + MenuManager.MM.username + " : " + newScore;
            SaveScore();
            SaveName();
        }
        else
        {
            newScore = oldScore;
            gameBestScore.text = "Best Score : " + MenuManager.MM.username + " : " + newScore;
            SaveScore();
            SaveName();
        }

    }
    
    [System.Serializable]
    public class SaveData
    {
        public int newScore;
        public string newName;
        public int oldScore;
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.newScore = newScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void SaveName()
    {
        SaveData data = new SaveData();
        data.newName = newName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savegile.json", json);
    }

    public void SaveOldScore()
    {
        SaveData data = new SaveData();
        data.oldScore = oldScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savesile.json", json);
    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            newScore = data.newScore;
        }
    }

    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savegile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            newName = data.newName;
        }
    }

    public void LoadOldScore()
    {
        string path = Application.persistentDataPath + "/savesile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            oldScore = data.oldScore;
        }
    }
}
