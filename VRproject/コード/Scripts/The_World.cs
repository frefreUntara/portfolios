using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// 濡れるギミック発生時にスローモーション、ポスプロ処理
public class The_World : MonoBehaviour
{

    [SerializeField] float now_wold_Time = 1;
    [SerializeField] float the_wold_time = 0.3f;
    [SerializeField] float the_world_combert = 0.1f;
    [SerializeField] bool isStop = false;
    private float the_world_progress = 0;
    private PostProcessVolume post_processer;
    private ChromaticAberration chromatic;
    private float leap_time = 1;

    // Start is called before the first frame update
    void Start()
    {
        the_world_progress = 0;
        post_processer = GameObject.Find("Post-process Volume").GetComponent<PostProcessVolume>();
        chromatic = post_processer.profile.GetSetting<ChromaticAberration>();
        leap_time -= the_wold_time;
    }

    // Update is called once per frame
    void Update()
    {
        The_TimeStop();
        if (isStop || Input.GetKey(KeyCode.D)) 
        {
            Progress_inclement();
        }
        else
        {
            Progress_declement();
        }

        chromatic.intensity.value = 1 * the_world_progress;
        Time.timeScale = now_wold_Time;
    }

    public void Set_StopperFlag(bool set_Stop)
    {
        isStop = set_Stop;
    }

    bool Get_StopperFlag() { return isStop; }

    // 
    void The_TimeStop()
    {
        if (now_wold_Time < 0)
        {
            now_wold_Time = 0;
        }
        else
        {
            now_wold_Time = 1 - leap_time * the_world_progress;
        }
    }

    void Progress_inclement()
    {
        chromatic.active = true;
        the_world_progress += the_world_combert;

        if (the_world_progress > 1)
        {
            the_world_progress = 1;
        }
    }

    void Progress_declement()
    {
        the_world_progress -= the_world_combert;
        if (the_world_progress <= 0)
        {
           chromatic.active = false;
            the_world_progress = 0;
        }
    }

}
