using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    AudioSource ambient;

    [SerializeField]
    AudioSource chestProximity;

    [SerializeField]
    AudioSource warriorProximity;

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
            chestProximity.Play();
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
            chestProximity.Stop();
        }
    }

    void OnCloseToHero()
    {
        if (!isCloseToHero)
        {
            isCloseToHero = true;
            warriorProximity.Play();
        }
    }

    void OnCoinTake()
    {
        isCoinTaken = true;
        if (isCloseToChest)
        {
            chestProximity.Stop();
        }
    }

    void OnFarFromHero()
    {
        if (isCloseToHero)
        {
            isCloseToHero = false;
            warriorProximity.Stop();
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
