using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 所持金追加クラス
// ステージクリア時に起動
[DefaultExecutionOrder(100)]
public class ClearMoneyManager : MonoBehaviour
{
    // 獲得最低値
    [SerializeField]
    int Defalt_getMoney = 1000;

    // 乱数上限
    [SerializeField]
    [Range(0,50)]
    private int RandomMoney = 50;

    // 追加金額表示
    [SerializeField]
    ScrambleTextDisp scramble;

    // JSON書き込みシステム
    [SerializeField]
    private Parts_Info_JSON JSON;

    // Start is called before the first frame update
    void Start()
    {
        Defalt_getMoney += Random.Range(0, RandomMoney);
        scramble.Set_DispChar("$" + Defalt_getMoney.ToString());
        scramble.LoadingText();
        JSON.AddMoney(Defalt_getMoney);
        JSON.OnSave();
    }
}
