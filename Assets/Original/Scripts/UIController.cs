using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    GameObject _startCanvas;

    void Awake()
    {
        PlayerDelegatesContainer.EventPlayerDead += OnDeath;
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.EventPlayerDead -= OnDeath;
    }

    void OnDeath()
    {
        _startCanvas.gameObject.SetActive(true);
    }

    // button startGame on startCanvas
    public void OnStartGame()
    {
        PlayerDelegatesContainer.EventPlayerAlive();
        _startCanvas.gameObject.SetActive(false);
    }
}