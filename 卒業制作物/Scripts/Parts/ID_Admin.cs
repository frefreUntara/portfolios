using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// パーツのID管理クラス
public class ID_Admin
{
    public enum PartsID
    {
        ENONE = -1,
        ECORE = 0,
        ESEALD = 1,
        ECANNON = 2,
        ECANNON_ADV = 3,
        ERAIL_GUN=4,
        EEMP=5,
        EDOUBLESHOT=6,
        ECOSTOPENER=7,
        ESIZE
    }

    // データ保存時のビットフラグ取得
    static public int GetBitFlag(PartsID id)
    {
        return 1 << (int)id;
    } 

}
