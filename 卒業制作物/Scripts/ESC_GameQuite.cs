using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESC_GameQuite : MonoBehaviour
{
    /// <summary>
    /// エスケープキーでゲームを閉じる
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }       
    }
}
