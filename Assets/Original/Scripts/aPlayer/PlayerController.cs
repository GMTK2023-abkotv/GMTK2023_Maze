using UnityEngine;

public enum TileTypes {
    Player,
    Walkable,
    Wall,
    Warrior,
    Chest,
    NotATile // just for a default
}

public class PlayerController : MotionController
{
    bool _isActivated;

    private SpriteRenderer _SpriteRenderer;
    Animator animator;

    int Animation_idle;
    int Animation_move;
    int Animation_attack;
    // int Animation_hit;
    // int Animation_die;

    protected override void Awake()
    {
        base.Awake();

        PlayerDelegatesContainer.IsMoving += IsMoving;
        PlayerDelegatesContainer.NewMove += OnNewMove;

        _SpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Hashing animation for better performance
        Animation_idle = Animator.StringToHash("minotaur_idle");
        Animation_move = Animator.StringToHash("minotaur_walk");
        Animation_attack = Animator.StringToHash("minotaur_smash");
        // Animation_hit = Animator.StringToHash("Player_hit");
        // Animation_die = Animator.StringToHash("Player_die");
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.IsMoving -= IsMoving;
        PlayerDelegatesContainer.NewMove -= OnNewMove;
    }

    void OnNewMove(Vector2 position, TileTypes tileType)
    {
        switch (tileType)
        {
            case TileTypes.Walkable:
                // Flip the Sprite in the correct direction
                Vector2 dir = position - (Vector2)transform.position;

                // by defauly minotaur faces left so flip it if it wants to go right
                if (dir.x != 0) _SpriteRenderer.flipX = (dir.x == 1) ? true : false;
                // Play Walk animation once ! NO_REPEAT
                // no need to use animator variables.
                animator.Play(Animation_move);

                start = transform.position;
                end = position;
                lerp = 0;
                break;

            case TileTypes.Wall:
                break;
                
            case TileTypes.NotATile:
                break;

            default:
                Debug.LogError("HOW DID WE GET HERE !!!");
                break;
        }
    }

    Transform GetTransform()
    {
        return transform;
    }
}