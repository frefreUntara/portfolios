using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// グリッド１つ１つでの状態に合わせた色調変更クラス
// セット可能→緑
// セット不可→赤
public class PartsColorChanger : MonoBehaviour
{
    SpriteRenderer imageSplite;

    [SerializeField]
    Color SetAble;

    [SerializeField]
    Color SetDisAble;

    // Start is called before the first frame update
    void Start()
    {
        imageSplite = GetComponent<SpriteRenderer>();
        imageSplite.color = SetAble;
    }

    
    public void ColorChange_SetAble()
    {
        imageSplite.color = SetAble;
    }
    public void ColorChange_SetDisAble()
    {
        imageSplite.color = SetDisAble;
    }

}
