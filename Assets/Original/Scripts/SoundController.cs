using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    List<AudioSource> ambientSources;

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

    void Awake()
    {
        GameDelegatesContainer.Start += OnStart;
        GameDelegatesContainer.TimeStep += OnTimeStep;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.Start -= OnStart;
        GameDelegatesContainer.TimeStep -= OnTimeStep;
    }

    void OnTimeStep()
    {
        Debug.Log("Playing timestep");
        timeStepSource.clip = timeStep;
        timeStepSource.Play();
    }

    void OnStart()
    {
        for (int i = 0; i < ambientSources.Count; i++)
        {
            Debug.Log("Playing " + i);
            ambientSources[i].Play();
        }
    }

    void OnWin()
    {
        endSource.clip = win;
        endSource.Play();
    }

    void OnLose()
    {
        endSource.clip = lose;
        endSource.Play();
    }
}
