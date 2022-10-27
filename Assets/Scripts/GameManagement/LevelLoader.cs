using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public void FadeReloadLevel(float waitingTime)
    {
           StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex, waitingTime)); 
    }

    public void ReloadLevel(float waitingTime)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FadeLoadNextLevel(float waitingTime)
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1, waitingTime));
    }

    public void LoadNextLevel(float waitingTime)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    IEnumerator LoadLevel(int levelIndex, float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(levelIndex);
    }
}
