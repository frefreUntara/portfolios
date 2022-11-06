using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// 誰ぞ誰が為に災厄の座に着くのか。
/// </summary>
public class Affection : MonoBehaviour
{
    public GameObject TargetPlace;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NavMeshAgent>().SetDestination(TargetPlace.transform.position);
    }
}
