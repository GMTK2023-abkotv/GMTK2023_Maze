using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject _startCanvas;

    // button startGame on startCanvas
    public void OnStartGame()
    {
        GameDelegatesContainer.Start();
        _startCanvas.gameObject.SetActive(false);
    }
}