using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W���Ƃɕs�v�ȉ�b�C�x���g�G���A���A�N�e�B�u������
/// </summary>

public class TalkAreaSwicher : MonoBehaviour
{
    [SerializeField] GameObject[] Talk_Events;
    [SerializeField] GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        int stage_Number = GameManager.GetComponent<Stage>().GetNumber();
        for(int i = 0; i < Talk_Events.Length; ++i)
        {
            if (i != stage_Number)
            {
                Talk_Events[i].SetActive(false);
            }
        }
    }
}
