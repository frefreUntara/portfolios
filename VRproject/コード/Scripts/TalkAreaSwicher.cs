using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージごとに不要な会話イベントエリアを非アクティブ化する
/// </summary>

public class TalkAreaSwicher : MonoBehaviour
{
    [SerializeField] GameObject[] Talk_Events;
    [SerializeField] GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        int stage_Number = GameManager.GetComponent<Stage>().GetNumber();
        for(int i = 0; i < Talk_Events.Length; ++i)
        {
            if (i != stage_Number)
            {
                Talk_Events[i].SetActive(false);
            }
        }
    }
}
