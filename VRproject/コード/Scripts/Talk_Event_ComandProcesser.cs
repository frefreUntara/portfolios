using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// コマンドラインを読み取って、表情操作やシーン遷移を発生させる
/// プロセッサー
/// </summary>
public class Talk_Event_ComandProcesser
{
    [SerializeField] private TextFaceControl face_controller;

    /// <summary>
    /// 表情の更新
    /// </summary>
    /// <param emotion="表情コマンド"></param>
    public void Set_SubComands(string emotion)
    {
        face_controller.SetFace(emotion);
    }

    /// <summary>
    /// シーン遷移コマンド実行
    /// </summary>
    /// <param next_SceneName="遷移先のシーン名"></param>
    public void Set_SceneChange(string next_SceneName)
    {
        Calling_LoadScene(next_SceneName);
    }
    
    /// <summary>
    /// 
    /// </summary>
    void Calling_LoadScene(string agoraCom)
    {
        GameObject SceneManager = new GameObject();
        SceneChange scenechange = SceneManager.AddComponent<SceneChange>();

        string[] comand_split = agoraCom.Split('>');
        scenechange.SceneChangeFunc(comand_split[1]);
    }
}
