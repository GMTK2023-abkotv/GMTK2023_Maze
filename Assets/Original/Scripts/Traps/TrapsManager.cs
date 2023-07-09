using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsManager : MonoBehaviour
{
    public int MaxTrapsCount = 3;
    // should contain the Prefabs of all the types of Traps.
    // public GameObject[] AllTrapsTypes;      // these will only be used for initialisation
    // wee need to keep the limit and the counter in a dict for multiple Traps

    public GameObject TrapPrefab;
    public Transform playerTransform;


    // we can show the traps in an array and then allow the player to choose them
    // using 

    // State
    
    private int trapsPlantedCount = 0;

    private List<GameObject> trapsPlanted;

    void Awake()
    {

        // already making space for the Trap objects
        List<Trap> trapsPlanted = new List<Trap>(MaxTrapsCount);

        playerTransform = GetComponent<Transform>();
        // bind player
        Debug.Assert(playerTransform != null, "playerTransform NOT SET", this.gameObject);
    }

    void Update()
    {
        // if the player.isSelectingTrap we need to set the Trap Selection UI to Active !
        // Make the Selected Trap Change using Mouse Hover..
        // todo: show preview of the currently selected Trap.
    }

    void plantTrap()
    {
        if (trapsPlantedCount > MaxTrapsCount) return;

        trapsPlantedCount += 1;
        // Get the Prefab selected by the player to spawn;

        // Creating the Trap.
        // Trap planted = Instantiate<Trap>(toPlant, transform.position, transform.rotation);
        GameObject planted = Instantiate<GameObject>(TrapPrefab, playerTransform.position, playerTransform.rotation);
        
        trapsPlanted.Add(planted);


        // how do i use the methods i defined on the Trap class ??.. do i need to do .GetComponent<>(Trap);

    }

    void pickUpTrap()
    {
        trapsPlantedCount -= 1;
    }

    // we need to add a EventLister function to delete the trap
    // on interact button

    // For the Wall Trap we need to also add a Wall Sprite on the location spawned
    // Might do it inside the Awake/Start Method of the Trap....
}
