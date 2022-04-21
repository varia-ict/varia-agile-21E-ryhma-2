using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FollowPlayer : MonoBehaviour

 

{
    // Start is called before the first frame update
    Vector3 offset = new Vector3(0, 2, -10);
    public GameObject player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //follows the player on an offset
        transform.position = player.transform.position + offset;
}
}
