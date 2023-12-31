using UnityEngine;

public class FeedbacksController : MonoBehaviour
{
    [SerializeField]
    Transform _particlesParent;

    [SerializeField]
    Transform _audioSourcesParent;

    [SerializeField]
    ParticleSystem _walkingStateParticles;

    [SerializeField]
    ParticleSystem _groundJumpParticles;

    [SerializeField]
    Transform _playerTransform;


    void Awake()
    {
    }

    void OnDestroy()
    {
    }

    void Start()
    { 
    }
}