using UnityEngine;

public enum TileTypes {
    Player,
    Walkable,
    Wall,
    Warrior,
    Trap,
    NotATile // just for a default
}

public class PlayerController : MotionController
{
    bool _isActivated;
    Animator animator;

    int Animation_idle;
    int Animation_move;
    int Animation_hit;
    int Animation_attack;
    int Animation_die;

    protected override void Awake()
    {
        base.Awake();
        PlayerDelegatesContainer.IsMoving += IsMoving;
        PlayerDelegatesContainer.NewMoveDestination += OnNewMove;
        animator = GetComponent<Animator>();
        
        // Hashing animation for better performance
        Animation_idle = Animator.StringToHash("Player_idle");
        Animation_move = Animator.StringToHash("Player_move");
        Animation_hit = Animator.StringToHash("Player_hit");
        Animation_attack = Animator.StringToHash("Player_attack");
        Animation_die = Animator.StringToHash("Player_die");
    }

    void OnDestroy()
    {
        PlayerDelegatesContainer.IsMoving -= IsMoving;
        PlayerDelegatesContainer.NewMoveDestination -= OnNewMove;
    }

    bool OnNewMove(Vector2 position, TileTypes tileType)
    {
        // Now we just need to handle animation here.
        Debug.Log(tileType);
        switch (tileType)
        {
            case TileTypes.Walkable:
                // Play Walk animation once ! NO_REPEAT
                // no need to use animator variables.
                animator.Play(Animation_move);

                start = transform.position;
                end = position;
                lerp = 0;
                return true;
                // break;

            case TileTypes.Wall:
                break;
                
            case TileTypes.NotATile:
                break;

            default:
                Debug.LogError("HOW DID WE GET HERE !!!");
                break;
        }
        return false;
    }

    Transform GetTransform()
    {
        return transform;
    }

    bool IsMoving()
    {
        return lerp <= 1;
    }
}