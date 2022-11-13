using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;


public class CSVImporter : MonoBehaviour
{

    enum Comands
    {
        // ID
        EID=1,
        // Xç¿ïW
        EIND_X=2,
        // Yç¿ïW
        EIND_Y=3,
        
        ESIZE
    }

    [SerializeField]
    private GameObject GridParent;

    [SerializeField]
    private GameObject[] parts;

    [SerializeField]
    private GameObject setTransform;

    [SerializeField]
    private SetCost_Checker costChecker;

    // Start is called before the first frame update
    void Start()
    {
        FortressMap_Reset();
    }

    public void FortressMap_Reset()
    {
        StreamReader csvFile = new StreamReader(Application.persistentDataPath + "/partsPos.csv");
        List<string> csv = new List<string>();

        while (!csvFile.EndOfStream) 
        {
            string csvSplit = csvFile.ReadLine();
            csv.Add(csvSplit);
        }

        string[] csvPartsData = new string[csv.Count];

        for(int i = 0; i < csvPartsData.Length; ++i)
        {
            csvPartsData[i] = csv[i];
        }

        Import_FortressData_CSV(csvPartsData);
    }

    public void FortressMap_Default()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("parts_Default");
        string[] csvData = csvFile.ToString().Split('\n');

        Import_FortressData_CSV(csvData);
    }

    void Import_FortressData_CSV(string[] csvData)
    {
        GridParts_Destory();
        
        for(int i = 1; i < csvData.Length; ++i)
        {
            string[] parametor = csvData[i].Split(',');
            if (parametor.Length < (int)Comands.EID) { continue; }
            int id = Int32.Parse(parametor[(int)Comands.EID]);

            GameObject mass = GameObject.Find("Mass " + parametor[(int)Comands.EIND_X] + parametor[(int)Comands.EIND_Y][0]);
            Quaternion Drection = Quaternion.identity;
            if (id != 0) 
            {
                Drection = Quaternion.Euler(0, 0,-90);
            }
            
            GameObject part = Instantiate(parts[id], mass.transform.position, Drection, setTransform.transform);
            part.GetComponent<PartsMove>().CreateParts_in_Grids(mass);
            
            PartsData cost = part.GetComponent<PartsData>();
            cost.SetCost_manager(costChecker);
            costChecker.Add_Cost(cost.Get_Cost());
        }
    }

    void GridParts_Destory()
    {
        GameObject Parts_Parent = GameObject.Find("Parts");

        for(int i = 0; i < Parts_Parent.transform.childCount; ++i)
        {
            Destroy(Parts_Parent.transform.GetChild(i).gameObject);
        }
    }

}
