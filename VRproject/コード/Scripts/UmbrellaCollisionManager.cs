using UnityEngine;


public class UmbrellaCollisionManager : MonoBehaviour
{
    private const float SCALE = 1.4f;

    /// <summary>
    /// 傘の当たり判定をデバイスの開き具合と同期させる
    /// </summary>
    [SerializeField]
    private GameObject Cloth {
        get; set;
    }

    public float Position {
        set {
            float scale = SCALE * value;

            Cloth.transform.localScale = new Vector3(scale, scale, scale);
        }
    }


    private void Start()
    {
        Cloth = GameObject.Find("Cloth");
    }
}
