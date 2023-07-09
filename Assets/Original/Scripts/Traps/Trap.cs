using UnityEngine;
public enum TrapTypes {
  Spikes
};

public abstract class Trap : MonoBehaviour {
  TrapTypes type;
  bool isPlanted;
  Vector2 location;

  // should i add a OnPlayer Interact ..???? so that it get picked by the player when he tries to interact with it
};
