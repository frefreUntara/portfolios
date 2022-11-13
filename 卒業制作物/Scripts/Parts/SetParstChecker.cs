using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ґ������^���N�̃p�[�c�Q���K��̃t�H�[�}�b�g�ʂ肩�`�F�b�N����
public class SetParstChecker : MonoBehaviour
{
    private List<GameObject> grids;
    // Start is called before the first frame update
    void Start()
    {
        grids=new List<GameObject>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            grids.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }
    // �Z�b�g���ꂽ��Ԃ���������ԁA�t�H�[�}�b�g���`�F�b�N
    // �d�l
    // �E�{�̂ł���R�A��1�ł��鎖
    // 0�A�����2�ȏ�ŕs�K���ƕԂ����
    public bool  Check_setPartsFormat()
    {
        bool is_trueFormat = false;

        foreach(var partsID in grids)
        {
            // �R�A�̃`�F�b�N
            if (partsID.GetComponent<GridAdmin>().GetPartsID() == ID_Admin.PartsID.ECORE) 
            {
                // 2�ڂ̏d�����֎~
                if (is_trueFormat)
                {
                    return false;
                }
                is_trueFormat = true;
            }
        }

        return is_trueFormat;
    }

    // �p�[�c���Z�b�g�ł���ꏊ�̔z������擾
    public List<GameObject> GetGrits()
    {
        return grids;
    }

}
