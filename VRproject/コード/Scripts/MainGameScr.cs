using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// ゴールにキャラがついたら自動でリザルトに飛ぶ。
/// </summary>
public class MainGameScr : MonoBehaviour
{
    bool isGoal = false;
    float timeCnt;
    float transrateTime = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        timeCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoal)
        {
            timeCnt += Time.deltaTime;
            if (timeCnt >= transrateTime)
            {
                SceneManager.sceneLoaded += ResultSceneLoaded;
                SceneManager.LoadScene("Result");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isGoal = true;
        }
    }

    private void ResultSceneLoaded(Scene next, LoadSceneMode mode)
    {
        var gameManager = GameObject.FindWithTag("GameManager").GetComponent<GaugeVaule>();
        var gauge = GameObject.Find("Player").GetComponent<PlayerGauge>();

        gameManager.SetGauge(gauge.GetGauge());

        SceneManager.sceneLoaded -= ResultSceneLoaded;
    }
}
