using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 編成されたパーツを取り外すオブジェクトクラス
public class Parts_Bin : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip dropSE;

    [SerializeField]
    private AudioClip EnterSE;

    private bool isDrop = false;

    private void Update()
    {
        if (!isDrop) { return; }
    
        if (Input.GetMouseButtonUp(0))
        {
            audio.PlayOneShot(dropSE);
            isDrop = false;
        }
    }

    // グリッドにパーツが当たったら破棄判定をセット
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsParts(collision)) { return; }
        isDrop = true;
        audio.PlayOneShot(EnterSE);
        collision.gameObject.GetComponent<PartsMove>().isSet = false;
    }

    // 当たった2Dオブジェクトがパーツか否かの判定
    private bool IsParts(Collider2D collision)
    {
        bool isParts = false;
        if (collision.name.Contains("Core")) { return isParts; }
        isParts = collision.gameObject.CompareTag("PlayerParts");
        return isParts;
    }
}
