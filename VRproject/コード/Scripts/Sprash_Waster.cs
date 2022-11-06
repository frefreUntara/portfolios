using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 水しぶきギミックの発生管理
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

        // 生成するパーティクルを保持している場合
        if (waterParticle != null)
        {
            // パーティクル再生終了
            if (waterParticle.isStopped)
            {
                // パーティクル削除
                Destroy(waterParticle.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Carオブジェクトと当たった場合
        if (other.tag == "Car")
        {
            Debug.Log("Hit");
            if (Mag < 100)
            {
                // パーティクルが再生されていない時に
                if (waterParticle == null)
                {
                    // パーティクルオブジェクトを生成
                    waterParticle = Instantiate(sprash_Waster, gameObject.transform).GetComponent<ParticleSystem>();
                }
            }
        }
    }
}
