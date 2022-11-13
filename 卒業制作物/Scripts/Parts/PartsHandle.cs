using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// パーツ一覧から設置可能なパーツを生成するクラス
public class PartsHandle : MonoBehaviour
{
    // 
    [SerializeField]
    GameObject CreateParts;

    [SerializeField]
    GameObject Canvas;

    [SerializeField]
    PartsSetAble[] setter;
 
    // 設置用パーツプレファブ作成
    public void PopCreateParts()
    {
        GameObject parts = Instantiate(CreateParts, Input.mousePosition,
                                    Quaternion.identity, Canvas.transform);
        Move_to_Pointer partsScr = parts.GetComponent<Move_to_Pointer>();
      
        partsScr.SetGenerateSource(this.gameObject);
        partsScr.SetMass(setter);
    }
}
