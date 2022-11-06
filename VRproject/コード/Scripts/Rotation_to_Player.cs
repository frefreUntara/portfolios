using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 
public class Rotation_to_Player : MonoBehaviour
{
    // 決定アニメーションプレファブ
    [SerializeField] GameObject AnimPref;

    // UIが向く対象のカメラ
    public GameObject camera
    { get; set; }

    // 生成された初期位置
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
    /// カメラと初期位置を補間する
    /// </summary>
    private void RePostion()
    {
        Vector3 rePosition = camera.transform.position - DefaultPos;
        rePosition.y = 0;
        rePosition *= 0.5f;

        transform.position = DefaultPos + (-transform.forward * rePosition.magnitude);
    }
    /// <summary>
    /// カメラの方にオブジェクトを向かせる
    /// </summary>
    private void ReRotate()
    {
        transform.LookAt(camera.transform.position);

        Quaternion rotate = transform.rotation;
        rotate *= Quaternion.Euler(0, 180, 0);
        transform.rotation = rotate;
    }
}
