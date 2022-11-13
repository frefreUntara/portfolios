using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// グリッド1つ1つが持つパラメータ
public class PartsSetAble : MonoBehaviour
{
    enum Status { 
        EEMPTY,
        ESET,
    }
    [SerializeField]
    Status state = Status.EEMPTY;

    [SerializeField]
    public RectTransform myRect
    {
        private set;
        get;
    }

    [SerializeField]
    private ID_Admin.PartsID setID = ID_Admin.PartsID.ENONE;

    [SerializeField]
    private float checkRange = 25;

    [SerializeField]
    private RectTransform setParts;
    

    private Image img;

    [SerializeField]
    private Color setableColor = Color.green;
    
    [SerializeField]
    private Color dissetableColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(setParts == null)
        {
            setID = ID_Admin.PartsID.ENONE;
            state = Status.EEMPTY;
        }
        switch (state)
        {
            case Status.ESET:
                img.color = dissetableColor;
                setParts.transform.position = myRect.transform.position;
                break;

            case Status.EEMPTY:
                img.color = setableColor;
                break;
        }

    }

    public void SetPartsChecker(Move_to_Pointer parts, RectTransform targetRect)
    {
      
        ID_Admin.PartsID checkID = parts.id;

        if(state==Status.ESET && setID != checkID) { return; }

        Rect rect = new Rect(myRect.localPosition.x, myRect.localPosition.y, myRect.rect.width, myRect.rect.height);
        Rect checkrect = new Rect(targetRect.localPosition.x, targetRect.localPosition.y, targetRect.rect.width, targetRect.rect.height);

        ///　自分の場所が空か、同じパーツのIDの場合当たり判定
        ///　

        if (rect.Overlaps(checkrect) && setParts == null)
        {
            if (state == Status.EEMPTY)
            {
                parts.IsSetable = true;
                setID = checkID;
                state = Status.ESET;
                setParts = targetRect;
                parts.SetPutOnMass(myRect);
            }
        }
        else if(setParts != null)
        {
            rect = new Rect(myRect.localPosition.x, myRect.localPosition.y, myRect.rect.width, myRect.rect.height);
            checkrect = new Rect(setParts.localPosition.x, setParts.localPosition.y, setParts.rect.width, setParts.rect.height);

            if (!rect.Overlaps(checkrect))
            {
                parts.IsSetable = false;
                setID = ID_Admin.PartsID.ENONE;
                state = Status.EEMPTY;
                setParts = null;
            }
        }
    }

}
