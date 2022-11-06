using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum TalkUI_State
{
    WAIT,
    PLAYING,
    END
}

// 会話文の文字送り＋文字数に合わせたテキストボックスの幅可変
public class Talk_Event_UIController : MonoBehaviour
{
    // テキストプレファブ
    public GameObject TextAsset_Pref;
    // テキスト出力先
    private GameObject Text_Output;

    // テキストボックスプレファブ
    public GameObject TextBox_Pref;
    // テキストボックス
    private GameObject TextBox;

    // キャンバス
    private GameObject canvas;
    // 文字送りの速さ
    float addChar_timerCnt = 0;
    float wait_NextCurrent = 1;
    float wait_TimeCnt = 0;
    // 出力文字数
    int DeriverChar = 0;
    // 出力対象の文字列
    private string Playing_Text = "";

    public AudioClip TalkSE;
    private AudioSource Audio_player;

    Text Disptext;
    Image TextBoxImg;
    TalkUI_State ui_state = TalkUI_State.WAIT;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
        CreateUI();
        Playing_Text = "";
        Change_Talk_UiState(TalkUI_State.WAIT); 
        
        Audio_player = GetComponent<AudioSource>();

        // アタッチされていない場合
        if (Audio_player == null)
        {
            Audio_player = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ui_state != TalkUI_State.PLAYING) { return; }

        float addChar_frame = 3 / 60.0f;
        string DisprayText = "";

        addChar_timerCnt += Time.deltaTime;
        // 指定の文字数分、テキストコンポーネントに文字を追加
        for (int i = 0; i < DeriverChar; ++i)
        {
            DisprayText += Playing_Text[i];
            Disptext.text = DisprayText;
        }

        // 一定時間を超えたら一文字追加
        if (addChar_timerCnt >= addChar_frame)
        {
            // １行の終端まで出力中の場合
            if (!Is_stringTerminal())
            {
                // もう１文字追加
                DeriverChar++;
                Audio_player.PlayOneShot(TalkSE);
                addChar_timerCnt = 0;
            }
            else if (wait_TimeCnt >= wait_NextCurrent)
            {
                Change_Talk_UiState(TalkUI_State.END);
                wait_TimeCnt = 0;
            }
            else
            {
                wait_TimeCnt += Time.deltaTime;
            }
        }
    }
    public void Set_Text(string text)
    {
        if (text == null)
        {
            Change_Talk_UiState(TalkUI_State.WAIT);
        }
        else
        {
            Change_Talk_UiState(TalkUI_State.PLAYING);
            Playing_Text = text;
            Resize_TextBox(Playing_Text.Length);
        }

        Disptext.text = "";
        DeriverChar = 0;
        addChar_timerCnt = 0;
    }
    // １行の終端まで出力されたか判定
    public bool Is_stringTerminal()
    {
        if (ui_state == TalkUI_State.WAIT) { return false; }
        return DeriverChar >= Playing_Text.Length;
    }

    public bool Is_Playing()
    {
        return ui_state == TalkUI_State.PLAYING;
    }

    /// <summary>
    /// 表示に必要なUI系統の生成
    /// </summary>
    private void CreateUI()
    {
        // テキストプレファブが生成れていない場合
        if (Text_Output == null)
        {
            Text_Output = Instantiate(TextAsset_Pref, canvas.transform);
        }
        // テキストコンポーネントを所持していない場合
        if (Disptext == null)
        {
            Disptext = Text_Output.GetComponent<Text>();
        }
        // テキストボックスプレファブが生成されていない場合
        if (TextBox == null)
        {
            TextBox = Instantiate(TextBox_Pref, Text_Output.transform.position, Quaternion.identity, canvas.transform);
            TextBox.transform.SetSiblingIndex(0);
            TextBox.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // テキストボックス画像の保持チェック
        if (TextBoxImg == null)
        {
            TextBoxImg = TextBox.GetComponent<Image>();
        }

        Vector2 rectPos = new Vector2(0, 25);
        Text_Output.GetComponent<RectTransform>().anchoredPosition = rectPos;
        TextBox.GetComponent<RectTransform>().anchoredPosition = rectPos;

    }

    private void Swich_UIVisibility(bool isDiplay)
    {
        TextBox.SetActive(isDiplay);
        Text_Output.SetActive(isDiplay);   
    }

    private void Resize_TextBox(int text_length)
    {
        Vector2 TextBoxScale = new Vector2(0, 60)
        {
            x = text_length * 8
        };

        TextBoxImg.rectTransform.sizeDelta = TextBoxScale;
    }

    private void Change_Talk_UiState(TalkUI_State _State)
    {
        ui_state = _State;
        if (ui_state == TalkUI_State.PLAYING) 
        {
            Swich_UIVisibility(true);
        }
        else
        {
            Swich_UIVisibility(false);
        }
    }

}
