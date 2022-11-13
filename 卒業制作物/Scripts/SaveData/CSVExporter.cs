using System.IO;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

// 編成データをcsv吐き出し
public class CSVExporter : MonoBehaviour
{

    enum SaveStatus
    {
        ECANSAVE        = 0 << 0,
        ECOSTOVER       = 1 << 0,
        ECORENONE       = 1 << 1,
        ETOOMANYCORE    = 1 << 2,
    }

    [SerializeField]
    private Text Console;
    
    [SerializeField]
    private Image Button;

    [SerializeField]
    private Color Default_Color;
    
    [SerializeField]
    private Color Saveable_Color;

    [SerializeField]
    private Color SaveDisable_Color;

    [SerializeField]
    private SetCost_Checker cost_manager;

    [SerializeField]
    private AudioPlayer audioPlayer;

    private SetParstChecker admin;
    private string convertCSV;
    
    private int SaveFlag = 0;
    // Start is called before the first frame update
    void Start()
    {
        admin = GetComponent<SetParstChecker>();
        Console.text = "保存する";
        Button.color = Default_Color;
    }

    public void CSVExporter_WAKEUP()
    {
       
        string[] writeData = { "パーツタイプ", "パーツID", "X座標", "Y座標" };
        convertCSV = string.Join(",", writeData);
        convertCSV += "\n";
        
        // TODO:ここの処理
        if (admin.Check_setPartsFormat())
        {
            if (cost_manager.Is_CostOver() == false)
            {
                Button.color = Saveable_Color;
                Console.text = "保存中";
                ExpoteSE_Play();
                ConverttoCSV();
            }
            else
            {
                Button.color = SaveDisable_Color;
                Console.text = "コストオーバー";
            }
        }
        else
        {
            Debug.LogError("コアを１つだけセットしてください");

            Button.color = SaveDisable_Color;
            Console.text = "コアを１つだけ\nセットしてください";
        }

        ButtonUIReset();
    }

    // 設置されたパーツからIDを受け取り、CSVへ吐き出し
    void ConverttoCSV()
    {
        List<GameObject> grids = admin.GetGrits();
        foreach (GameObject partsData in grids)
        {
            GridAdmin chell = partsData.GetComponent<GridAdmin>();
            int id = (int)chell.GetPartsID();
            Vector2 index = chell.GetGridIndex();

            // 受け取ったIDが[未設置]以外の場合だけIDを書き込む
            if (id != (int)ID_Admin.PartsID.ENONE)
            {
                string[] LineData = { "0", id.ToString(), index.x.ToString(), index.y.ToString() };
                convertCSV += string.Join(",", LineData);
                convertCSV += "\n";
            }
        }

        Debug.Log(Application.persistentDataPath);

        File.WriteAllText(@Application.persistentDataPath + "/partsPos.csv", convertCSV, Encoding.UTF8);
        
        
        Button.color = Saveable_Color;
        Console.text = "完了";
    }
     
    // エラー吐いた結果、UIの表示変更
    private async void ButtonUIReset()
    {
        await Task.Delay(1500);
        if (Button == null) { return; }
        Button.color = Default_Color;
        Console.text = "保存する";
    }

    private void ExpoteSE_Play()
    {
        audioPlayer.AudioPlay();
    }

}
