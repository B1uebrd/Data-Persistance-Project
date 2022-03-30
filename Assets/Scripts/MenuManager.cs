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
    public InputField nameInput;
    public string username;
    public TextMeshProUGUI bestScore;
    public float score;

    // Start is called before the first frame update
    void Start()
    {
        LoadInput();
        if (username != null)
        {
            nameInput.text = username;
        }
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

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadInput()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            username = data.username;
        }
    }
}
