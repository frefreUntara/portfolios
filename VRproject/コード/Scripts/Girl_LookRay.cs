using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl_LookRay : MonoBehaviour
{
    [SerializeField] bool mIsOver = false;
    [SerializeField] bool mIsLookGirl = false;
    [SerializeField] float mMaxLookTime = 1.5f;
    float mLoockTimeounter = 0;
    [SerializeField] float mLineOfSightRange = 5;
    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        RayHit(1 << 7);
    }

    private void RayHit(int layerMask_)
    {
        if (mIsOver == true) { return; }

        Ray ray = new Ray(this.transform.position, this.transform.forward);
        //‰ÂŽ‹‰»
        Debug.DrawRay(ray.origin, ray.direction * 10.0f, Color.blue);

        if (Physics.Raycast(ray, out RaycastHit hit, mLineOfSightRange, layerMask_))
        {
            mIsLookGirl = true;
            mLoockTimeounter += Time.deltaTime;
            
            if (mLoockTimeounter >= mMaxLookTime) //ˆê’èŽžŠÔŒ©‚Ä‚¢‚½‚ç
            {
                mIsOver = true;
            }
        }
        else
        {
            mIsLookGirl = false;
            mLoockTimeounter = 0;
        }

    }

    public bool IsLook_OverTime() { return mIsOver; }

    public bool IsLook_Girl() { return mIsLookGirl; }

    public float Look_Time() { return mLoockTimeounter; }

    public void Reset_Parametor()
    {
        mIsOver = false;
        mIsLookGirl = false;
        mLoockTimeounter = 0;
    }
}
