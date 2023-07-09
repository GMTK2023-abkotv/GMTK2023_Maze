using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform ObjectToFollow;
    public Vector3 offset;
    void Start()
    {
        Debug.Assert(ObjectToFollow != null, "ObjectToFollow Not Set", this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.forward * -10 +  ObjectToFollow.position - offset;
        
    }
}
