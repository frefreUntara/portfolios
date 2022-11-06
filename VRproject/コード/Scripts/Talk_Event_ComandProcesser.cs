using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �R�}���h���C����ǂݎ���āA�\����V�[���J�ڂ𔭐�������
/// �v���Z�b�T�[
/// </summary>
public class Talk_Event_ComandProcesser
{
    [SerializeField] private TextFaceControl face_controller;

    /// <summary>
    /// �\��̍X�V
    /// </summary>
    /// <param emotion="�\��R�}���h"></param>
    public void Set_SubComands(string emotion)
    {
        face_controller.SetFace(emotion);
    }

    /// <summary>
    /// �V�[���J�ڃR�}���h���s
    /// </summary>
    /// <param next_SceneName="�J�ڐ�̃V�[����"></param>
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
