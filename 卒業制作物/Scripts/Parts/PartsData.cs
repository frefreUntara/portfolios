using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 編成にセットされたパーツが持つパラメータ
public class PartsData : MonoBehaviour
{
    // パーツID
    [SerializeField]
    private ID_Admin.PartsID ID = ID_Admin.PartsID.ENONE;

    // 自分１つにかかるコスト
    [SerializeField]
    private int cost = 0;

    // 編成された全パーツのコスト管理をするクラス
    private SetCost_Checker cost_manager;

    // パーツID取得
    public ID_Admin.PartsID GetPartsID() { return ID; }

    public int Get_Cost() { return cost; }

    // このコンポーネント削除＝デッキから削除される＝コストの減少
    private void OnDestroy()
    {
        cost_manager.RemoveCost(cost);
    }

    // コスト管理コンポーネントをセット(パーツをD&Dする時)
    public void SetCost_manager(SetCost_Checker manager)
    {
        cost_manager = manager;
    }
}
