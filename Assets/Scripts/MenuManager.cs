using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager MM;
    public InputField nameInput;
    public string username;
    public TextMeshProUGUI bestScore;
    public float score;

    // Start is called before the first frame update
    void Awake()
    {
        if (MM != null)
        {
            GameObject.Destroy(MM);
        }
        else 
        {
            MM = this;
        }
        DontDestroyOnLoad(this);

        LoadInput();
        if (username != null)
        {
            nameInput.text = username;
        }

        bestScore.text = "Best Score: " + username + " : " + score;
    }

    // Update is called once per frame
    void Update()
    {
        username = nameInput.text;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
    #else
        Application.Quit();
    #endif
    }
    
    [System.Serializable]
    public class SaveData
    {
        public string username;
    }

    public void SaveInput()
    {
        SaveData data = new SaveData();
        data.username = username;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savebile.json", json);
    }

    public void LoadInput()
    {
        string path = Application.persistentDataPath + "/savebile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            username = data.username;
        }
    }
}
