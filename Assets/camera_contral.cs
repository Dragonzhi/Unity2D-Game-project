using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_contral : MonoBehaviour
{
    public Transform followPlayer;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followPlayer.position.x, 0, -10f);
        //followPlayer.position.y
    }
}
