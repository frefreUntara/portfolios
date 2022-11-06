using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlay_AtOnce : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("END")) 
        {
            Destroy(gameObject);
        }
    }
}
