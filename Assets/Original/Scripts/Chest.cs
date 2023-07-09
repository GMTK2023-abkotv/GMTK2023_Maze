using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]
    Sprite emptyChestSprite;

    void Awake()
    {
        GameDelegatesContainer.CoinTake += OnCoinTake;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.CoinTake += OnCoinTake;
    }

    void OnCoinTake()
    {
        GetComponent<SpriteRenderer>().sprite = emptyChestSprite;
    }
}