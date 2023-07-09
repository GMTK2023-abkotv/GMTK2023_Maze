using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject startCanvas;

    [SerializeField]
    GameObject loseCanvas;

    [SerializeField]
    GameObject winCanvas;

    void Awake()
    {
        startCanvas.gameObject.SetActive(false);
        loseCanvas.gameObject.SetActive(false);
        winCanvas.gameObject.SetActive(false);

        GameDelegatesContainer.Lose += OnLose;
        GameDelegatesContainer.Win += OnWin;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.Lose -= OnLose;
        GameDelegatesContainer.Win -= OnWin;
    }

    void OnLose()
    {
        loseCanvas.gameObject.SetActive(true);
        StartCoroutine(CheckForKeyPress());
    }
    
    public void OnWin()
    {
        winCanvas.gameObject.SetActive(true);
        StartCoroutine(CheckForKeyPress());
    }

    // button startGame on startCanvas
    public void OnStartGame()
    {
        
    }

    IEnumerator CheckForKeyPress()
    {
        yield return new WaitForSeconds(3);
        while (true)
        { 
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }

            yield return null;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}