using UnityEngine;

public class Timer : MonoBehaviour
{
    int currentTime;

    void Awake()
    {
        GameDelegatesContainer.TimeStep += OnTimeStep;
        GameDelegatesContainer.GetCurrentTime += GetCurrentTime;
    }

    void OnDestroy()
    {
        GameDelegatesContainer.TimeStep -= OnTimeStep;
        GameDelegatesContainer.GetCurrentTime -= GetCurrentTime;
    }

    void OnTimeStep()
    {
        currentTime++;
    }

    int GetCurrentTime()
    {
        return currentTime;
    }
}