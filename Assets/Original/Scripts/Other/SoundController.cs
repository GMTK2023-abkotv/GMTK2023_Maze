using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    AudioSource ambient;

    [SerializeField]
    AudioClip timeStep;

    [SerializeField]
    AudioClip win;

    [SerializeField]
    AudioClip lose;

    [SerializeField]
    AudioSource timeStepSource;

    [SerializeField]
    AudioSource endSource;

    bool isCloseToChest;
    bool isCloseToHero;
    bool isCoinTaken;

    void Awake()
    {
        GameDelegatesContainer.Start += OnStart;
        GameDelegatesContainer.TimeStep += OnTimeStep;

        GameDelegatesContainer.CloseToChest += OnCloseToChest;
        GameDelegatesContainer.FarFromChest += OnFarFromChest;

        GameDelegatesContainer.CloseToHero += OnCloseToHero;
        GameDelegatesContainer.FarFromHero += OnFarFromHero;

        GameDelegatesContainer.CoinTake += OnCoinTake;

        GameDelegatesContainer.Win += OnWin;
        GameDelegatesContainer.Lose += OnLose;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.Start -= OnStart;
        GameDelegatesContainer.TimeStep -= OnTimeStep;

        GameDelegatesContainer.CloseToChest -= OnCloseToChest;
        GameDelegatesContainer.FarFromChest -= OnFarFromChest;

        GameDelegatesContainer.CloseToHero -= OnCloseToHero;
        GameDelegatesContainer.FarFromHero -= OnFarFromHero;

        GameDelegatesContainer.CoinTake -= OnCoinTake;

        GameDelegatesContainer.Win -= OnWin;
        GameDelegatesContainer.Lose -= OnLose;
    }

    void OnCloseToChest()
    {
        if (isCoinTaken)
        {
            return;
        }

        if (!isCloseToChest)
        {
            isCloseToChest = true;
            // StartCoroutine(IncreaseVolumeChest());
        }
    }

    void OnFarFromChest()
    {
        if (isCoinTaken)
        {
            return;
        }

        if (isCloseToChest)
        {
            isCloseToChest = false;
            // StartCoroutine(DecreaseVolumeChest());
        }
    }

    // IEnumerator IncreaseVolumeChest()
    // {
    //     chestProximity.Play();
    //     float time = 1.5f;
    //     float c = 0;
    //     chestProximity.volume = 0;
    //     while (c < time)
    //     {
    //         chestProximity.volume = Mathf.Lerp(0, 1, c / time);
    //         yield return null;
    //         c += Time.deltaTime;
    //     }
    // }

    // IEnumerator DecreaseVolumeChest()
    // {
    //     float time = 1.5f;
    //     float c = 0;
    //     chestProximity.volume = 1;
    //     while (c < time)
    //     {
    //         chestProximity.volume = Mathf.Lerp(1, 1, c / time);
    //         yield return null;
    //         c += Time.deltaTime;
    //     }
    //     chestProximity.Stop();
    // }

    void OnCloseToHero()
    {
        if (!isCloseToHero)
        {
            isCloseToHero = true;
            // StartCoroutine(IncreaseVolumeWarrior());
        }
    }

    // IEnumerator IncreaseVolumeWarrior()
    // {
    //     warriorProximity.Play();
    //     float time = 1.5f;
    //     float c = 0;
    //     warriorProximity.volume = 0;
    //     while (c < time)
    //     {
    //         warriorProximity.volume = Mathf.Lerp(0, 1, c / time);
    //         yield return null;
    //         c += Time.deltaTime;
    //     }
    // }

    // IEnumerator DecreaseVolumeWarrior()
    // {
    //     float time = 1.5f;
    //     float c = 0;
    //     warriorProximity.volume = 1;
    //     while (c < time)
    //     {
    //         warriorProximity.volume = Mathf.Lerp(1, 1, c / time);
    //         yield return null;
    //         c += Time.deltaTime;
    //     }
    //     warriorProximity.Stop();
    // }

    void OnCoinTake()
    {
        isCoinTaken = true;
        // if (isCloseToChest)
        // {
        //     chestProximity.Stop();
        // }
    }

    void OnFarFromHero()
    {
        if (isCloseToHero)
        {
            isCloseToHero = false;
            // StartCoroutine(DecreaseVolumeWarrior());
        }
    }

    void OnTimeStep()
    {
        timeStepSource.clip = timeStep;
        timeStepSource.Play();
    }

    void OnStart()
    {
        ambient.Play();
    }

    void OnWin()
    {
        ambient.Stop();
        endSource.clip = win;
        endSource.Play();
    }

    void OnLose()
    {
        ambient.Stop();

        endSource.clip = lose;
        endSource.Play();
    }
}
