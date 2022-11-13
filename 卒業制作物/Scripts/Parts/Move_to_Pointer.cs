using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// パーツをセットするグリッド1つ1つでのパラメータ
public class Move_to_Pointer : MonoBehaviour
{
    // カーソルと重なっている
    private bool IsChoise = true;
    // パーツが置ける
    public bool IsSetable = false;

    // "パーツが外された"と判定をいれるまでのレンジ
    [SerializeField]    
    [Range(0.5f,1)]
    private float RemoveScale = 0.5f;
    
    // 
    private GameObject GenerateSource;

    // 
    private Vector3 RemovePosition = Vector3.zero;
    private RectTransform HitBox;
    public ID_Admin.PartsID id
    {
        get;
        private set;
    }


    [SerializeField]
    private PartsSetAble[] Mass;

    [SerializeField]
    private RectTransform put_onMass;

    [SerializeField]
    private float GridInterval;

    // Start is called before the first frame update
    void Start()
    {
        HitBox = GetComponent<RectTransform>();
        id = GetComponent<PartsData>().GetPartsID();
    }

    // Update is called once per frame
    void Update()
    {

        if (IsChoise)
        {
            Vector3 position = Input.mousePosition;
            position.x = (int)position.x / GridInterval;
            position.y = (int)position.y / GridInterval;
            position.x *= GridInterval;
            position.y *= GridInterval;
         
            gameObject.transform.position = position;

            for(int i = 0; i < Mass.Length; ++i)
            {
                Mass[i].SetPartsChecker(this, HitBox);
            }
            if (put_onMass != null)
            {
                HitBox.anchoredPosition = put_onMass.anchoredPosition;
            }
        }
        else if (IsSetable == false)
        {
            Vector3 movedvel = RemovePosition * RemoveScale;
            gameObject.transform.position += movedvel;
            RemovePosition -= movedvel;
         
            if (RemovePosition.magnitude <= 0.3f) { Destroy(gameObject); }
        }


        if (Input.GetMouseButtonUp(0))
        {
            IsChoise = false;
            RemovePosition = GenerateSource.transform.position - transform.position;
        }
    }

    public void SetChoise()
    {
        IsChoise = true;
    }

    // 
    public void SetMass(PartsSetAble[] setAbles)
    {
        Mass = setAbles;
    }

    public void SetGenerateSource(GameObject source)
    {
        GenerateSource = source;
    }

    // パーツを設置
    public void SetPutOnMass(RectTransform massGrid)
    {
        put_onMass = massGrid;
    }
}
