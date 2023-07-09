using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*

Plant Traps -> 


PlayerController -> 

Animation Logic + Check next tile...



*/

public class WallTrapScript : MonoBehaviour
{

    public int health;

    // Start is called before the first frame update
    void Start()
    {
        // make a 'WallTrap' Tile on the logical TileMap.
    }

    // Update is called once per frame
    void Update()
    {
        // check for hits ..???
        // or maybe the Warrior can Interact with this somehow...
        // we need an animation of the Warrior Hitting the Wall and
        // the Wall Taking Damage turning white for a few frames maybe...

        if (health <= 0)
        {
            // This Trap Should be deleted
            // The TrapsManager.trapsPlantedCount should be decremented
        }
    }

    void onHit()
    {
        health -= 1;
    }
}
