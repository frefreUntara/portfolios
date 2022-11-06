using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// スクリプト内部のコマンド区分け
enum OrderState
{
    MAINCOMAND, // Text、Sceneなど
    SUBCOMAND,  // キャラ表情や、シーン遷移など
    ORDERDATA   // 会話本文、遷移先のシーン名指定など
}

// 現在のスクリプトのステート一覧
public enum LoadState
{
    NONE,
    LOADING,
    COMPLETE,
}

/// <summary>
/// NPCキャラの会話イベントの感情とテキストを配列で管理
/// </summary>
public class TextFileLoadScr
{
    /// 現在の行
    private int CurrentRow = 0;
    public string[,] OrderDetails;
    public LoadState loadState = LoadState.NONE;

    /// <summary>
    /// テキストファイルを読み込み、感情とテキストを配列で保存
    /// </summary>
    /// <param name="filePath">Resourcesファイルをカレントとした読み込み対象のファイルパス</param>
    public void CreateText(string filePath)
    {
        Reset();

        // 改行で区切ったデータ格納
        string[] LoadTexts;

        // Resourcesファイルからテキストファイル読み込み
        var LoadData = Resources.Load<TextAsset>(filePath).text;

        // 改行ごとにテキストを切り分けて格納
        LoadTexts = LoadData.Split('\n');

        int neo_arraySize = 0;
        foreach (string text in LoadTexts)
        {
            // テキストデータで正しいテキスト分カウント
            if (FindComand(text))
            {
                neo_arraySize++;
            }
        }

        // スクリプトのサイズに合わせたstring配列構築
        OrderDetails = new string[neo_arraySize, 3];

        // 感情とテキスト配列のインデックス
        int Emotion_Index = 0;

        foreach (var CurrtneDeta in LoadTexts)
        {
            // 読み込んだ行が指定した書式として成立しているかチェック
            if (FindComand(CurrtneDeta))
            {
                string[] ComandData = CurrtneDeta.Split('/');
                OrderDetails[Emotion_Index, (int)OrderState.MAINCOMAND] = ComandData[0];
                OrderDetails[Emotion_Index, (int)OrderState.SUBCOMAND] = ComandData[1].Split('>')[0];
                OrderDetails[Emotion_Index, (int)OrderState.ORDERDATA] = ComandData[1].Split('>')[1];
                Emotion_Index++;
            }
        }
        loadState = LoadState.LOADING;
    }
    /// <summary>
    /// 行の構成が書式通りかチェック
    /// </summary>
    /// <param name="SeachTarget"></param>
    /// <returns></returns>
    private bool FindComand(string SeachTarget)
    {
        return -1 != SeachTarget.IndexOf('>');
    }
    /// <summary>
    /// 現在のスクリプトのメインコマンド取得
    /// </summary>
    public string GetCurrent_MainComand()
    {
        if (IsRowOver() == false)
        {
            return OrderDetails[CurrentRow, (int)OrderState.MAINCOMAND];
        }
        else
        {
            return null;
        }

    }
     /// <summary>
    /// 現在のスクリプトのサブコマンド取得
    /// </summary>
    public string GetCurrent_SubComand()
    {
        if (IsRowOver() == false)
        {
            return OrderDetails[CurrentRow, (int)OrderState.SUBCOMAND];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 表示対象の文字列を配列から取得
    /// </summary>
    /// <returns></returns>
    public string GetCurrentRowText()
    {
        if (IsRowOver() == false)
        {
            return OrderDetails[CurrentRow, (int)OrderState.ORDERDATA];
        }
        else
        {
            return null;
        }
    }
    
    /// <summary>
    /// カレント行を移動
    /// </summary>
    public void NextRow()
    {
        if (IsRowOver() == false)
        {
            CurrentRow++;
        }
    }
    /// <summary>
    /// 全行出力が完了したか判定
    /// </summary>
    /// <returns></returns>
    public bool IsRowOver()
    {
        if (OrderDetails == null) { return true; }
        // return CurrentRow >= Text.Length;
        return CurrentRow >= OrderDetails.GetLength(0);
    }

    /// <summary>
    /// 初期状態にする。
    /// 現在の読み込み対象を0行目にする
    /// </summary>
    private void Reset()
    {
        CurrentRow = 0;
    }
}

