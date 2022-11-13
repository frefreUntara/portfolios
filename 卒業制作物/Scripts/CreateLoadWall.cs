using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// フェードイン、アウトシステム起動クラス
public class CreateLoadWall : MonoBehaviour
{
    [SerializeField]
    private GameObject Blackout_Prefab;

    [SerializeField]
    private GameObject Canvas;

    public void CreateBlakout()
    {
        Instantiate(Blackout_Prefab, Canvas.transform);
    }
}
