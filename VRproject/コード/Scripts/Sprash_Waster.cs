using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����Ԃ��M�~�b�N�̔����Ǘ�
public class Sprash_Waster : MonoBehaviour
{
    private GameObject player;
    public float Mag;
    public GameObject sprash_Waster;
    [SerializeField]
    private ParticleSystem waterParticle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 magnitude = transform.position - player.transform.position;
        Mag = magnitude.magnitude;

        // ��������p�[�e�B�N����ێ����Ă���ꍇ
        if (waterParticle != null)
        {
            // �p�[�e�B�N���Đ��I��
            if (waterParticle.isStopped)
            {
                // �p�[�e�B�N���폜
                Destroy(waterParticle.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Car�I�u�W�F�N�g�Ɠ��������ꍇ
        if (other.tag == "Car")
        {
            Debug.Log("Hit");
            if (Mag < 100)
            {
                // �p�[�e�B�N�����Đ�����Ă��Ȃ�����
                if (waterParticle == null)
                {
                    // �p�[�e�B�N���I�u�W�F�N�g�𐶐�
                    waterParticle = Instantiate(sprash_Waster, gameObject.transform).GetComponent<ParticleSystem>();
                }
            }
        }
    }
}
