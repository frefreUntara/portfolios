using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// テキストを文字送り形式で出力する。
/// テキストの表示、文字送り、テキストボックスの表示非表示、テキストの更新
/// スクリプトはテキストが付いているオブジェクトにアタッチする事。
/// 制作者：日本電子専門学校　ゲーム制作科２年　20CI0205 伊東佳汰　
/// </summary>

// 既に終了した会話イベントを保持するログクラス
struct Talk_Event_Logger
{
    [SerializeField] private List<string> LogEvent;
    // 書き込みストッパー
    bool isAdded;

    // スタック用の連想配列作成
    public void Create_LogEventArray()
    {
        LogEvent = new List<string>();
    }

    // 終了した会話イベントをスタック
    public void Add(string EventName)
    {
        if (isAdded) { return; }
        LogEvent.Add(EventName);
        isAdded = true;
    }

    // ログへの書き込み制限のストッパー解除
    public void AddFlag_Reset()
    {
        isAdded = false;
    }

    // 指定の会話イベントが追加されているかチェック
    public bool FindEventLog(string Sarch_EventName)
    {
        return LogEvent.IndexOf(Sarch_EventName) != -1;
    }

}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioClip))]


public class TalkEvent_Scr : MonoBehaviour
{

    [SerializeField] string[,] ProcesserData;

    // ファイル読み込みクラス
    TextFileLoadScr textFile;
    // 待機中の会話イベント
    [SerializeField] private List<string> WaitEvent;
    // 終了した会話イベントログ
    Talk_Event_Logger _Event_Logger;
    public string Playing_Event
    {
        get;
        set;
    }
    // NPCの名前
    public string CharaName = "";

    // 表情スクリプト
    public TextFaceControl faceControl;
    private Talk_Event_UIController ui_Controller;
    private Talk_Event_ComandProcesser event_processer;
   
    // Start is called before the first frame update
    void Start()
    {
        ui_Controller = GetComponent<Talk_Event_UIController>();
        event_processer = GetComponent<Talk_Event_ComandProcesser>();
        _Event_Logger = new Talk_Event_Logger();
        _Event_Logger.Create_LogEventArray();

         WaitEvent = new List<string>();
        textFile = new TextFileLoadScr();
    }

    // Update is called once per frame
    void Update()
    {
        // 全行出力されたか判定
        if (!Is_TextEnd())
        {
            if (textFile.loadState == LoadState.NONE) { return; }

            // １行出し切ったら
            if (!ui_Controller.Is_Playing())
            {
                textFile.NextRow();
                switch (textFile.GetCurrent_MainComand())
                {
                    case "Text":
                        event_processer.Set_SubComands(textFile.GetCurrent_SubComand());
                        ui_Controller.Set_Text(textFile.GetCurrentRowText());
                        break;

                    case "Scene":
                        GameObject scenechanger = new GameObject("SceneChanger");
                        SceneChange changer = scenechanger.AddComponent<SceneChange>();
                        changer.SceneChangeFunc(textFile.GetCurrentRowText());

                        break;
                }
            }
        }
        else
        {
            _Event_Logger.Add(Playing_Event);
            if (WaitEvent.Count > 0)
            {
                Debug.Log("次のイベント：" + WaitEvent[0]);
                TalkEvent_Play(WaitEvent[0]);
                DeleteEvent(WaitEvent[0]);
                _Event_Logger.AddFlag_Reset();
            }
            else
            {
                textFile.loadState = LoadState.NONE;
            }
        }
    }
    // 全文表示されたか判定
    public bool Is_TextEnd() => textFile.IsRowOver();

    /// <summary>
    /// 会話イベント開始
    /// </summary>
    /// <param _filename="_filename"></param>
    private void TalkEvent_Play(string _filename)
    {
        Playing_Event = _filename;
        textFile.CreateText("Text/" + CharaName + "/" + Playing_Event);
        ui_Controller.Set_Text(textFile.GetCurrentRowText());
    }
    /// <summary>
    /// 会話イベント追加
    /// </summary>
    public void AddEvent(string EventName)
    {
        WaitEvent.Add(EventName);
    }
    /// <summary>
    /// 終了した会話イベントをキューから削除
    /// </summary>
    private void DeleteEvent(string EventName)
    {
        WaitEvent.Remove(EventName);
    }


    /// <summary>
    /// 指定の会話イベントが終了しているか判定
    /// </summary>
    /// <param name="TalkEv_Target"></param>
    /// <returns></returns>
    public bool Is_TargetEvent_End(string TalkEv_Target)
    {
        return _Event_Logger.FindEventLog(TalkEv_Target);
    }
}
