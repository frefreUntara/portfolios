using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BoxCollider))]
/// ��b�C�x���g�����G���A�Ǘ��N���X
/// ����ҁF���{�d�q���w�Z�@�Q�[������ȂQ�N�@20CI0205 �ɓ������@
public class Talk_EventArea : MonoBehaviour
{
    // ���s��������b�C�x���g
    public string EventName = null;
    // ���m�������^�[�Q�b�g�^�O
    public string TargetTag = "Girl";
    // ���m�͈�
    public Vector3 PerceptionAreaScale = new Vector3(10, 1, 3);
    // ���m����������
    [SerializeField] private bool isPassing = false;
    // �������������������
    public bool isLoockMe
    {
        get;
        set;
    }
   

    // ��b�C�x���g�X�N���v�g
    private TalkEvent_Scr talkev_;
    // 
    private float TimeCounter;
    // 
    private float NextPsee = 3;

    private NavMeshAgent girlAI;

    [SerializeField] private Talk_Event_LookUI_Manager look_manager;

    [SerializeField]
    private Girl_LookRay mGirl_LookRay;

    [SerializeField]
    private NaviMoveVer3 naviMove;

    // Start is called before the first frame update
    void Start()
    {
        TimeCounter = NextPsee;
        // �G���A����p�̃R���C�_�[��
        BoxCollider Perception_area = GetComponent<BoxCollider>();
        if (Perception_area == null)
        {
            Perception_area = gameObject.AddComponent<BoxCollider>();
        }

        mGirl_LookRay.enabled = false;
        look_manager.enabled = false;

        Perception_area.isTrigger = true;
        Perception_area.size = PerceptionAreaScale;
    }

    private void Update()
    {
        if (talkev_ == null) { return; }

        if (isLoockMe == false)
        {
            TimeCounter += Time.deltaTime;
            if (TimeCounter >= NextPsee)
            {
                talkev_.AddEvent("�˂��˂�");
                TimeCounter = 0;
            }
        }
        else
        {
            if (isPassing)
            {
                if (talkev_.Is_TargetEvent_End(EventName))
                {
                    girlAI.speed = 3.5f;
                    talkev_ = null;
                    mGirl_LookRay.enabled = false;
                    naviMove.SetMoveFlag(true);
                    naviMove.speed = 3.5f;
                }
            }
            else
            {
                /// �����Œe���ߎ�
                /// ��������������b�C�x���g�����ɃX�^�b�N�����Ă�
                talkev_.AddEvent(EventName);
                isPassing = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPassing) { return; }

        if (other.gameObject.tag == TargetTag)
        {
            look_manager.Girl = other.gameObject;
            look_manager.Create_LookUI();
            talkev_ = other.GetComponent<TalkEvent_Scr>();
            girlAI = other.GetComponent<NavMeshAgent>();
            girlAI.speed = 0;
            naviMove.SetMoveFlag(false);
            naviMove.speed = 0;
            isLoockMe = false;
            mGirl_LookRay.enabled = true;
            look_manager.enabled = true;
        }
    }
    /// <summary>
    /// �G���A���ɑΏۂ����������̃t���O�擾
    /// </summary>
    /// <returns></returns>
    public bool IsPassing() => isPassing;
}
