using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// マウスの移動についてくるクラス
public class PartsMove : MonoBehaviour
{
    [SerializeField]
    bool isChoise = true;

    public bool isSet;

    private Vector3 removePosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        isSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsChoose();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            isChoise = false;
            if (isSet == false)
            {
                Destroy(gameObject);
            }
        }
        Move_by_MousePosition();
    }

    // マウスの位置から自分が選ばれているかチェック
    void IsChoose()
    {

        int max_distance = 50;
        int layerMask = 1 << 10;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit_info = Physics2D.Raycast(ray.origin, ray.direction, max_distance, layerMask);

        if (hit_info == false) { return; }

        if (hit_info.collider.gameObject == gameObject)
        {
            isChoise = true;
        }
    }

    // マウスカーソルと座標を同期させる
    void Move_by_MousePosition()
    {
        if (isChoise)
        {
            Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos.z = 0;
            transform.position = mousepos;
        }
        else
        {
            Move_to_SourceObject();
        }
    }

    // パーツリストの戻る
    void Move_to_SourceObject()
    {
        transform.position = removePosition;
    }

    // グリッドとの当たり判定
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Grid")
        {
            GameObject getParts = collision.gameObject.GetComponent<GridAdmin>().setParts;
            if (getParts == gameObject)
            {
                isSet = true;
                removePosition = collision.gameObject.transform.position;
                transform.position = collision.gameObject.transform.position;
            }
        }
    }

    // デッキデータ読み込み時に指定されたグリッドに生成する
    public void CreateParts_in_Grids(GameObject gridMass)
    {
        isChoise = false;
        isSet = true;
        removePosition = gridMass.transform.position;
        transform.position = gridMass.transform.position;
    }
}
