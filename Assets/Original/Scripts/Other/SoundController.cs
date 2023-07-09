using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    List<AudioSource> ambientSources;

    [SerializeField]
    AudioSource chestAmbient;

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

    void Awake()
    {
        GameDelegatesContainer.Start += OnStart;
        GameDelegatesContainer.TimeStep += OnTimeStep;

        GameDelegatesContainer.CloseToChest += OnCloseToChest;
        GameDelegatesContainer.FarFromChest += OnFarFromChest;

        GameDelegatesContainer.Win += OnWin;
        GameDelegatesContainer.Lose += OnLose;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.Start -= OnStart;
        GameDelegatesContainer.TimeStep -= OnTimeStep;

        GameDelegatesContainer.CloseToChest -= OnCloseToChest;
        GameDelegatesContainer.FarFromChest -= OnFarFromChest;

        GameDelegatesContainer.Win -= OnWin;
        GameDelegatesContainer.Lose -= OnLose;
    }

    void OnCloseToChest()
    {
        if (!isCloseToChest)
        {
            isCloseToChest = true;
            chestAmbient.Play();
        }
    }

    void OnFarFromChest()
    {
        if (isCloseToChest)
        {
            isCloseToChest = false;
            chestAmbient.Stop();
        }
    }

    void OnTimeStep()
    {
        timeStepSource.clip = timeStep;
        timeStepSource.Play();
    }

    void OnStart()
    {
        for (int i = 0; i < ambientSources.Count; i++)
        {
            ambientSources[i].Play();
        }
    }

    void OnWin()
    {
        foreach (var source in ambientSources)
        {
            source.Stop();
        }
        endSource.clip = win;
        endSource.Play();
    }

    void OnLose()
    {
        foreach (var source in ambientSources)
        {
            source.Stop();
        }

        endSource.clip = lose;
        endSource.Play();
    }
}
