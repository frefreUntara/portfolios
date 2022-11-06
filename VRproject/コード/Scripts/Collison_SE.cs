using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����蔻��͊�{�I��BoxCollider�œ���B
/// ��ʂ̃I�u�W�F�N�g�ɃR���C�_�[��������̂Ńf�[�^�팸�����˂Ă��܂�
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
public class Collison_SE : MonoBehaviour
{
    [SerializeField] private List<AudioClip> mHitSE;
    private AudioSource mAudioSource;
    private BoxCollider mBoxCollider;
    // Start is called before the first frame update
    void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mBoxCollider = GetComponent<BoxCollider>();
        if (mAudioSource == null)
        {
            Debug.LogError("AudioSource���A�^�b�`����Ă��܂���");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Umbllera")
        {
            mAudioSource.Stop();
            mAudioSource.PlayOneShot(mHitSE[Random.Range(0, mHitSE.Count)]);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (mBoxCollider.isTrigger==false) { return; }

        if (collision.gameObject.tag == "Umbllera")
        {
            mAudioSource.Stop();
            mAudioSource.PlayOneShot(mHitSE[Random.Range(0, mHitSE.Count)]);
        }
    }
}
