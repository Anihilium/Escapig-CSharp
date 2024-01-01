using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;




public class PauseMenu : MonoBehaviour
{
    static public bool bOnPause;
    private bool stopPause;
    private bool openMenu;
    private bool quit;


    private GameObject Canva;
    [SerializeField] private GameObject firstSeletedButton;
    [SerializeField] private FMODUnity.StudioEventEmitter snapshotPause;
    [SerializeField] private FMODUnity.StudioEventEmitter returnSound;
    [SerializeField] private FMODUnity.StudioEventEmitter menuSound;
    [SerializeField] private FMODUnity.StudioEventEmitter quitSound;
    [SerializeField] private GameObject UICanva;




    // Start is called before the first frame update
    void Start()
    {
        Canva = transform.GetChild(0).gameObject;
        Canva.SetActive(false);

    }

    private void LateUpdate()
    {
        if(stopPause)
        {
            bOnPause = false;
            stopPause = false;
        }
        else if(openMenu)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if(quit)
        {
            Application.Quit();
        }
    }

    public void BeginPause(InputAction.CallbackContext value)
    {
        if(value.started && !bOnPause)
        {
            if(UICanva)
                UICanva.SetActive(false);
            snapshotPause.Play();
            bOnPause = true;
            Canva.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSeletedButton);
            FindObjectOfType<PlayerManager>().StopCurrentPlayer();
            Time.timeScale = 0f;
        }
    }

    public void ReturnGame()
    {
        Time.timeScale = 1f;

        StartCoroutine(ReturnCoroutine());
        if (UICanva)
            UICanva.SetActive(true);

    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;

        StartCoroutine(MenuCoroutine());
    }

    public void Quit()
    {
        Time.timeScale = 1f;

        StartCoroutine(QuitCoroutine());
    }

    IEnumerator ReturnCoroutine()
    {
        returnSound.Play();
        snapshotPause.Stop();

        yield return new WaitForSeconds(0.5f);

        Canva.SetActive(false);
        stopPause = true;
    }

    IEnumerator MenuCoroutine()
    {
        menuSound.Play();
        snapshotPause.Stop();
        bOnPause = false;

        yield return new WaitForSeconds(0.5f);

        openMenu = true;
    }

    IEnumerator QuitCoroutine()
    {
        quitSound.Play();
        //animator.SetTrigger("tClic");

        yield return new WaitForSeconds(0.3f);

        quit = true;
    }
}
