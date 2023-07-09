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
    }
    
    public void OnWin()
    {
        winCanvas.gameObject.SetActive(true);
    }

    // button startGame on startCanvas
    public void OnStartGame()
    {
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}