using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 編成したタンクのパーツ群が規定のフォーマット通りかチェックする
public class SetParstChecker : MonoBehaviour
{
    private List<GameObject> grids;
    // Start is called before the first frame update
    void Start()
    {
        grids=new List<GameObject>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            grids.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }
    // セットされた状態が正しい状態、フォーマットかチェック
    // 仕様
    // ・本体であるコアが1つである事
    // 0個、および2個以上で不適合と返される
    public bool  Check_setPartsFormat()
    {
        bool is_trueFormat = false;

        foreach(var partsID in grids)
        {
            // コアのチェック
            if (partsID.GetComponent<GridAdmin>().GetPartsID() == ID_Admin.PartsID.ECORE) 
            {
                // 2個目の重複を禁止
                if (is_trueFormat)
                {
                    return false;
                }
                is_trueFormat = true;
            }
        }

        return is_trueFormat;
    }

    // パーツをセットできる場所の配列情報を取得
    public List<GameObject> GetGrits()
    {
        return grids;
    }

}
