using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject _startCanvas;

    void Start()
    {
        GameDelegatesContainer.Start();
        _startCanvas.gameObject.SetActive(false);
    }

    // button startGame on startCanvas
    public void OnStartGame()
    {
        
        
    }
}