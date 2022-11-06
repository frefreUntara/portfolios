using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラのプレイヤートラッキング
/// </summary>
public class PlayerTrakcingCam : MonoBehaviour
{
    public GameObject player;
    public Vector3 OffestPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        OffestPos = new Vector3(0, 1.3f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + OffestPos;
    }
}
