using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �O���b�h1�̃X�e�[�g�Ǘ��N���X
public class GridAdmin : MonoBehaviour
{
    // �����̃}�X�ɃZ�b�g����Ă���p�[�c
    // csv�f���o���Ɏg���܂�
    public GameObject setParts;

    // �}�X�̏󋵁i�ݒu�\�j��F�ŕ\��
    [SerializeField]
    private PartsColorChanger GridColor;
    // �ݒu���ꂽ�p�[�c�������Ă���ID
    [SerializeField]
    private ID_Admin.PartsID have_PartsID = ID_Admin.PartsID.ENONE;
    // �}�X��Y���W
    [SerializeField]
    [Range(0,6)]
    private int index_Y;
    // �}�X��X���W
    [SerializeField]
    [Range(0,8)]
    private int index_X;
    // AudioSource
    [SerializeField]
    private AudioSource audios;
    // �}�X�̏�Ƀp�[�c����������SE
    [SerializeField]
    private AudioClip partsput_SE;

    // Start is called before the first frame update
    void Start()
    {
        setParts = null;
        if (GridColor == null)
        {
            GridColor = GetComponent<PartsColorChanger>();
        }

        if (audios == null) 
        {
            audios = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �p�[�c���Z�b�g����Ă��Ȃ��ꍇ�̓}�X�̐F���u�ݒu�\�v�̐F�i�R�����g�L�����͗ΐF�j�ɕύX
        if (setParts == null)
        {
            GridColor.ColorChange_SetAble();
        }
    }
    // csv�f���o���p�F���ݔz�u����Ă���p�[�c��ID��Ԃ�
   public ID_Admin.PartsID GetPartsID()
    {
        return have_PartsID;
    }
    // �����̃O���b�h���W��Ԃ�
    public Vector2 GetGridIndex()
    {
        Vector2 myindex = Vector2.zero;
        myindex.x = index_X;
        myindex.y = index_Y;
        return myindex;
    }

    // �p�[�c�����O���ꂽ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != setParts) { return; }
        audios.PlayOneShot(partsput_SE);
        GridColor.ColorChange_SetAble();
        setParts = null;
        have_PartsID = ID_Admin.PartsID.ENONE;
    }

    // �p�[�c���Z�b�g����Ă��鎞
    private void OnTriggerStay2D(Collider2D collision)
    {
        GridColor.ColorChange_SetDisAble();
    }

    // �p�[�c���Z�b�g���ꂽ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (setParts != null) { return; }

        if (collision.gameObject.CompareTag("PlayerParts"))
        {
            setParts = collision.gameObject;
            have_PartsID = collision.gameObject.GetComponent<PartsData>().GetPartsID();
        }

        if (collision.gameObject.CompareTag("DammyCollider"))
        {
            setParts = collision.gameObject;
        }

    }
}
