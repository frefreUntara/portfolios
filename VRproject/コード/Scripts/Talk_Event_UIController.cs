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

// ��b���̕�������{�������ɍ��킹���e�L�X�g�{�b�N�X�̕���
public class Talk_Event_UIController : MonoBehaviour
{
    // �e�L�X�g�v���t�@�u
    public GameObject TextAsset_Pref;
    // �e�L�X�g�o�͐�
    private GameObject Text_Output;

    // �e�L�X�g�{�b�N�X�v���t�@�u
    public GameObject TextBox_Pref;
    // �e�L�X�g�{�b�N�X
    private GameObject TextBox;

    // �L�����o�X
    private GameObject canvas;
    // ��������̑���
    float addChar_timerCnt = 0;
    float wait_NextCurrent = 1;
    float wait_TimeCnt = 0;
    // �o�͕�����
    int DeriverChar = 0;
    // �o�͑Ώۂ̕�����
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

        // �A�^�b�`����Ă��Ȃ��ꍇ
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
        // �w��̕��������A�e�L�X�g�R���|�[�l���g�ɕ�����ǉ�
        for (int i = 0; i < DeriverChar; ++i)
        {
            DisprayText += Playing_Text[i];
            Disptext.text = DisprayText;
        }

        // ��莞�Ԃ𒴂�����ꕶ���ǉ�
        if (addChar_timerCnt >= addChar_frame)
        {
            // �P�s�̏I�[�܂ŏo�͒��̏ꍇ
            if (!Is_stringTerminal())
            {
                // �����P�����ǉ�
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
    // �P�s�̏I�[�܂ŏo�͂��ꂽ������
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
    /// �\���ɕK�v��UI�n���̐���
    /// </summary>
    private void CreateUI()
    {
        // �e�L�X�g�v���t�@�u��������Ă��Ȃ��ꍇ
        if (Text_Output == null)
        {
            Text_Output = Instantiate(TextAsset_Pref, canvas.transform);
        }
        // �e�L�X�g�R���|�[�l���g���������Ă��Ȃ��ꍇ
        if (Disptext == null)
        {
            Disptext = Text_Output.GetComponent<Text>();
        }
        // �e�L�X�g�{�b�N�X�v���t�@�u����������Ă��Ȃ��ꍇ
        if (TextBox == null)
        {
            TextBox = Instantiate(TextBox_Pref, Text_Output.transform.position, Quaternion.identity, canvas.transform);
            TextBox.transform.SetSiblingIndex(0);
            TextBox.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // �e�L�X�g�{�b�N�X�摜�̕ێ��`�F�b�N
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
