using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

// 初期起動でファイルが未生成の場合
// 初期データを書き込む
public class First_CSVExporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            StreamReader Filecheck = new StreamReader(Application.persistentDataPath + "/partsPos.csv");
        }
        catch(FileNotFoundException e)
        {
            TextAsset csvFile = Resources.Load<TextAsset>("parts_Default");
            string[] csvData = csvFile.ToString().Split('\n');
            string csv = "";

            for (int i = 0; i < csvData.Length; ++i)
            {
                csv += csvData[i];
                csv += '\n';
            }

            File.WriteAllText(@Application.persistentDataPath + "/partsPos.csv", csv, Encoding.UTF8);
        }
    }
}
