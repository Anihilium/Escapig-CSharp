using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator animatorStart;
    [SerializeField] private Animator animatorQuit;

    [SerializeField] private FMODUnity.StudioEventEmitter StartSound;
    [SerializeField] private FMODUnity.StudioEventEmitter QuitSound;


    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    public void Quit()
    {
        StartCoroutine(QuitCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        StartSound.Play();
        animatorStart.SetTrigger("tClic");


        yield return new WaitForSeconds(1.5f);

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync(1);
    }

    IEnumerator QuitCoroutine()
    {
        QuitSound.Play();

        animatorQuit.SetTrigger("tClic");

        yield return new WaitForSeconds(0.3f);

        Debug.Log("Quit");

        Application.Quit();

    }
}
