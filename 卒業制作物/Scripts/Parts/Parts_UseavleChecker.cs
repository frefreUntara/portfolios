using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// 使用できるパーツデータを確認
public class Parts_UseavleChecker : MonoBehaviour
{
    [SerializeField]
    private Parts_Info_JSON JSONLeader;

    // 編成時に使えるパーツプレファブ
    [SerializeField]
    private GameObject[] PartsList;

    // コストチェックシステム
    [SerializeField]
    private SetCost_Checker _Checker;

    // 編成コスト解放パーツ１つで拡張できるコスト値
    [SerializeField]
    private int AddCoset_val = 25;

    // Start is called before the first frame update
    void Start()
    {
        int UseFlag = JSONLeader.OnLoad().Useavle_Flag;
        for (int i = 0; i < PartsList.Length; ++i)
        {
            int bit = 1 << i;
            bool debug = (UseFlag & bit) == bit;
            Debug.Log(debug + " " + Convert.ToString(UseFlag, 2) + " " + ((ID_Admin.PartsID)i).ToString());
            
            // コスト上限解放にフラグが経っている場合、定数値のコストを追加
            if (i == (int)ID_Admin.PartsID.ECOSTOPENER && (UseFlag & bit) == bit) 
            {
                _Checker.AddMaxCost(AddCoset_val);
            }
            PartsList[i].SetActive((UseFlag & bit) == bit);
        }
    }
}
