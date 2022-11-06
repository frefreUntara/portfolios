using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 
public class Rotation_to_Player : MonoBehaviour
{
    // ����A�j���[�V�����v���t�@�u
    [SerializeField] GameObject AnimPref;

    // UI�������Ώۂ̃J����
    public GameObject camera
    { get; set; }

    // �������ꂽ�����ʒu
    [SerializeField]
    private Vector3 DefaultPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        DefaultPos = transform.position;
        
        ReRotate();
        RePostion();
    }

    // Update is called once per frame
    void Update()
    {
        ReRotate();
    }

    /// <summary>
    /// �J�����Ə����ʒu���Ԃ���
    /// </summary>
    private void RePostion()
    {
        Vector3 rePosition = camera.transform.position - DefaultPos;
        rePosition.y = 0;
        rePosition *= 0.5f;

        transform.position = DefaultPos + (-transform.forward * rePosition.magnitude);
    }
    /// <summary>
    /// �J�����̕��ɃI�u�W�F�N�g����������
    /// </summary>
    private void ReRotate()
    {
        transform.LookAt(camera.transform.position);

        Quaternion rotate = transform.rotation;
        rotate *= Quaternion.Euler(0, 180, 0);
        transform.rotation = rotate;
    }
}
