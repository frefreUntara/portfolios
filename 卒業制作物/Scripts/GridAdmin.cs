using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// グリッド1つのステート管理クラス
public class GridAdmin : MonoBehaviour
{
    // 自分のマスにセットされているパーツ
    // csv吐き出しに使います
    public GameObject setParts;

    // マスの状況（設置可能）を色で表示
    [SerializeField]
    private PartsColorChanger GridColor;
    // 設置されたパーツが持っているID
    [SerializeField]
    private ID_Admin.PartsID have_PartsID = ID_Admin.PartsID.ENONE;
    // マスのY座標
    [SerializeField]
    [Range(0,6)]
    private int index_Y;
    // マスのX座標
    [SerializeField]
    [Range(0,8)]
    private int index_X;
    // AudioSource
    [SerializeField]
    private AudioSource audios;
    // マスの上にパーツが来た時のSE
    [SerializeField]
    private AudioClip partsput_SE;

    // Start is called before the first frame update
    void Start()
    {
        setParts = null;
        if (GridColor == null)
        {
            GridColor = GetComponent<PartsColorChanger>();
        }

        if (audios == null) 
        {
            audios = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // パーツがセットされていない場合はマスの色を「設置可能」の色（コメント記入時は緑色）に変更
        if (setParts == null)
        {
            GridColor.ColorChange_SetAble();
        }
    }
    // csv吐き出し用：現在配置されているパーツのIDを返す
   public ID_Admin.PartsID GetPartsID()
    {
        return have_PartsID;
    }
    // 自分のグリッド座標を返す
    public Vector2 GetGridIndex()
    {
        Vector2 myindex = Vector2.zero;
        myindex.x = index_X;
        myindex.y = index_Y;
        return myindex;
    }

    // パーツが取り外された時
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != setParts) { return; }
        audios.PlayOneShot(partsput_SE);
        GridColor.ColorChange_SetAble();
        setParts = null;
        have_PartsID = ID_Admin.PartsID.ENONE;
    }

    // パーツがセットされている時
    private void OnTriggerStay2D(Collider2D collision)
    {
        GridColor.ColorChange_SetDisAble();
    }

    // パーツがセットされた時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (setParts != null) { return; }

        if (collision.gameObject.CompareTag("PlayerParts"))
        {
            setParts = collision.gameObject;
            have_PartsID = collision.gameObject.GetComponent<PartsData>().GetPartsID();
        }

        if (collision.gameObject.CompareTag("DammyCollider"))
        {
            setParts = collision.gameObject;
        }

    }
}
