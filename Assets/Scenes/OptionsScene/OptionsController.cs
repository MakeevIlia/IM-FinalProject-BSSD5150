using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class OptionsController : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown dificulty_Choice;

    private const string dificultyLevelKey = "dificulty";
    private const string nameKey = "userName";
    private int currentDificulty = 1;
    private int defaultDificulty = 1;

    void Start()
    {
        defaultDificulty = 1;
        CheckPlayerPrefs();    
    }
    public void ClearPreferences()
    {
        PlayerPrefs.DeleteAll();
        dificulty_Choice.value = defaultDificulty;
    }
    public void OpenMenu()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void DificultyChanged(int choice)
    {
        currentDificulty = choice;
    }

    public void DificultySaved()
    {
        PlayerPrefs.SetInt(dificultyLevelKey, currentDificulty);
    }

    void CheckPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(nameKey))
        {
            int choice = PlayerPrefs.GetInt(dificultyLevelKey, 1);

            if (dificulty_Choice != null)
            {
                dificulty_Choice.value = choice;
            }
        }
    }

}
