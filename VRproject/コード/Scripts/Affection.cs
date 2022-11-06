using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// ’N‚¼’N‚ªˆ×‚ÉĞ–ï‚ÌÀ‚É’…‚­‚Ì‚©B
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
