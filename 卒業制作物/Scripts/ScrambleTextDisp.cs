using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// �����������ŕ\�����Ă���܂��B
/// DOToween�g�p
/// </summary>

public class ScrambleTextDisp : MonoBehaviour
{
    // �\��������e�L�X�g�ꏊ
    [SerializeField]
    private Text text;
    // �\�����镶����
    [SerializeField]
    private string Disp_Char;

    // Start is called before the first frame update
    void Start()
    {
        // �e�L�X�g�R���|�[�l���g�̎w�肪�Ȃ��ꍇ�͎����̃I�u�W�F�N�g����擾����
        if (text == null)
        {
            text = GetComponent<Text>();
        }
    }


    public void Set_DispChar(string text)
    {
        Disp_Char = text;
    }

    public void LoadingText()
    {
        text.DOText(Disp_Char, 2, scrambleMode: ScrambleMode.All).SetEase(Ease.InOutCirc);
    }

}
