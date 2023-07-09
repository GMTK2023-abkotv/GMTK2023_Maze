using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject _startCanvas;

    [SerializeField]
    GameObject _endCanvas;

    void Awake()
    {
        GameDelegatesContainer.End += OnEnd;
        _endCanvas.gameObject.SetActive(false);
        _startCanvas.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        GameDelegatesContainer.End -= OnEnd;
    }

    void OnEnd()
    {
        _endCanvas.gameObject.SetActive(true);
    }

    void Start()
    {
        GameDelegatesContainer.Start();
    }

    // button startGame on startCanvas
    public void OnStartGame()
    {
        
    }

    public void OnEndGame()
    {
        SceneManager.LoadScene(0);
    }
}