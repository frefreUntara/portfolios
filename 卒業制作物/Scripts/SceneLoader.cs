using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

//  遷移シーン遷移
// カクつきを抑える為に非同期で読み込み＋とフェードイン、アウト時間の5秒ディレイ
public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string LoadSceneName = "";

    public async void SceneLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadSceneName);
        operation.allowSceneActivation = false;
        await Task.Delay(500);
        operation.allowSceneActivation = true;
    }

}
