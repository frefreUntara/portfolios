using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Go_Straight : MonoBehaviour
{
    [SerializeField] float mMoveSpeed = 4;
    [SerializeField] float mLifeTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mLifeTime -= Time.deltaTime;
        transform.position += transform.up * (mMoveSpeed * Time.deltaTime);

        if (mLifeTime < 0)
        {
            Destroy(gameObject);
        }

    }
}
