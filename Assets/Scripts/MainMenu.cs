using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        if (!PlayerPrefs.HasKey("level"))
            PlayerPrefs.SetInt("level", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 + PlayerPrefs.GetInt("level"));
    }

    public void ResetSave() {
        PlayerPrefs.DeleteKey("level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
