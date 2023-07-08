using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject _startCanvas;

    // button startGame on startCanvas
    public void OnStartGame()
    {
        PlayerDelegatesContainer.EventPlayerAlive();
        _startCanvas.gameObject.SetActive(false);
    }
}