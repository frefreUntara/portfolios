using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �e�L�X�g�𕶎�����`���ŏo�͂���B
/// �e�L�X�g�̕\���A��������A�e�L�X�g�{�b�N�X�̕\����\���A�e�L�X�g�̍X�V
/// �X�N���v�g�̓e�L�X�g���t���Ă���I�u�W�F�N�g�ɃA�^�b�`���鎖�B
/// ����ҁF���{�d�q���w�Z�@�Q�[������ȂQ�N�@20CI0205 �ɓ������@
/// </summary>

// ���ɏI��������b�C�x���g��ێ����郍�O�N���X
struct Talk_Event_Logger
{
    [SerializeField] private List<string> LogEvent;
    // �������݃X�g�b�p�[
    bool isAdded;

    // �X�^�b�N�p�̘A�z�z��쐬
    public void Create_LogEventArray()
    {
        LogEvent = new List<string>();
    }

    // �I��������b�C�x���g���X�^�b�N
    public void Add(string EventName)
    {
        if (isAdded) { return; }
        LogEvent.Add(EventName);
        isAdded = true;
    }

    // ���O�ւ̏������ݐ����̃X�g�b�p�[����
    public void AddFlag_Reset()
    {
        isAdded = false;
    }

    // �w��̉�b�C�x���g���ǉ�����Ă��邩�`�F�b�N
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

    // �t�@�C���ǂݍ��݃N���X
    TextFileLoadScr textFile;
    // �ҋ@���̉�b�C�x���g
    [SerializeField] private List<string> WaitEvent;
    // �I��������b�C�x���g���O
    Talk_Event_Logger _Event_Logger;
    public string Playing_Event
    {
        get;
        set;
    }
    // NPC�̖��O
    public string CharaName = "";

    // �\��X�N���v�g
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
        // �S�s�o�͂��ꂽ������
        if (!Is_TextEnd())
        {
            if (textFile.loadState == LoadState.NONE) { return; }

            // �P�s�o���؂�����
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
                Debug.Log("���̃C�x���g�F" + WaitEvent[0]);
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
    // �S���\�����ꂽ������
    public bool Is_TextEnd() => textFile.IsRowOver();

    /// <summary>
    /// ��b�C�x���g�J�n
    /// </summary>
    /// <param _filename="_filename"></param>
    private void TalkEvent_Play(string _filename)
    {
        Playing_Event = _filename;
        textFile.CreateText("Text/" + CharaName + "/" + Playing_Event);
        ui_Controller.Set_Text(textFile.GetCurrentRowText());
    }
    /// <summary>
    /// ��b�C�x���g�ǉ�
    /// </summary>
    public void AddEvent(string EventName)
    {
        WaitEvent.Add(EventName);
    }
    /// <summary>
    /// �I��������b�C�x���g���L���[����폜
    /// </summary>
    private void DeleteEvent(string EventName)
    {
        WaitEvent.Remove(EventName);
    }


    /// <summary>
    /// �w��̉�b�C�x���g���I�����Ă��邩����
    /// </summary>
    /// <param name="TalkEv_Target"></param>
    /// <returns></returns>
    public bool Is_TargetEvent_End(string TalkEv_Target)
    {
        return _Event_Logger.FindEventLog(TalkEv_Target);
    }
}
