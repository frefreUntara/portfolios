using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 文字を自動で表示してくれます。
/// DOToween使用
/// </summary>

public class ScrambleTextDisp : MonoBehaviour
{
    // 表示させるテキスト場所
    [SerializeField]
    private Text text;
    // 表示する文字列
    [SerializeField]
    private string Disp_Char;

    // Start is called before the first frame update
    void Start()
    {
        // テキストコンポーネントの指定がない場合は自分のオブジェクトから取得する
        if (text == null)
        {
            text = GetComponent<Text>();
        }
    }


    public void Set_DispChar(string text)
    {
        Disp_Char = text;
    }

    public void LoadingText()
    {
        text.DOText(Disp_Char, 2, scrambleMode: ScrambleMode.All).SetEase(Ease.InOutCirc);
    }

}
