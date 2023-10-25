using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadSceneAsync("game");
    }
    public void ExitGame()
    {
        SceneManager.LoadSceneAsync("startScene");
    }
}
