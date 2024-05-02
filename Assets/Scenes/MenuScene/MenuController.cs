using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI titleText;
    [SerializeField]
    TMP_InputField inputField;

    private const string dificultyLevelKey = "dificulty";
    private const string nameKey = "userName";


    void Start()
    {
        CheckPlayerPrefs();
    }

    public void RegisterName()
    {
        string name = inputField.text.ToString();
        PlayerPrefs.SetString(nameKey, name);
        CheckPlayerPrefs();
    }

    public void OpenGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsScene", LoadSceneMode.Single);
    }
    public void OpenMenu()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    void CheckPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(nameKey))
        {
            string name = PlayerPrefs.GetString(nameKey);

            if (inputField != null)
            {
                inputField.text = name;
                titleText.text = "Welcome, " + name;
            }
        }
    }

}
