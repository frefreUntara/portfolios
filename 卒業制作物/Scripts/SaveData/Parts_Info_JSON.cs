using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[DefaultExecutionOrder(50)]
public class Parts_Info_JSON : MonoBehaviour
{
    // JSON�t�@�C���̍\��
    [System.Serializable]
    public struct PartsData
    {
        // ���łɏ�����
        public int money;
        // �r�b�g�Ńt���O�Ǘ����܂�
        public int Useavle_Flag;

        private bool isLoadFlag;

        public PartsData(int start = 0)
        {
            Useavle_Flag = 0;
            isLoadFlag = false;
            money = 0;
        }

        public bool isLoaded()
        {
            return isLoadFlag;
        }

        public void SetDefaltStats()
        {
            money = 1000;
            Useavle_Flag = 0xF;
        }

    }

    private string DataPath;

    private PartsData partsData;

    // Start is called before the first frame update
    void Start()
    {
        FilePathCheck();

        partsData.SetDefaltStats();

        // �t�@�C����������Ȃ��ꍇ
        if (!File.Exists(DataPath))
        {
            string json = JsonUtility.ToJson(partsData, true);
            File.WriteAllText(DataPath, json);
        }
        partsData = OnLoad();
    }

    // �ۑ�
    public void OnSave()
    {
        FilePathCheck();

        string json = JsonUtility.ToJson(partsData, true);
        File.WriteAllText(DataPath, json);
        partsData = OnLoad();
    }

    // �ǂݍ���
    public PartsData OnLoad()
    {
        FilePathCheck();
        if (!File.Exists(DataPath))
        {
            PartsData default_Data = new PartsData();
            default_Data.SetDefaltStats();
            return default_Data;
        }

        string json = File.ReadAllText(DataPath);
        PartsData data = JsonUtility.FromJson<PartsData>(json);

        Debug.Log(Convert.ToString(data.Useavle_Flag, 2));
        Debug.Log(data.money);
        return data;
    }
    // ���l��ID���w�肵�ăt���O�𗧂Ă�
    public void SetUseFlag(int Parts_id)
    {
        partsData.Useavle_Flag |= 1 << Parts_id;
    }

    // ID���w�肵�ăt���O�𗧂Ă�
    public void SetUseFlag(ID_Admin.PartsID Parts_id)
    {
        partsData.Useavle_Flag |= (int)Parts_id;
    }

    // �����̒ǉ�
    public void AddMoney(int addmoney)
    {
        if (addmoney < 0) { return; }
        partsData.money += addmoney;
    }

    // �������㏑���Œǉ�
    public void SetMoney(int money)
    {
        if (money < 0) { return; }
        partsData.money = money;
    }

    // �����̎擾
    public int GetMoney()
    {
        return partsData.money;
    }

    // ID(���l)����r�b�g�t���O���Q�Ƃ��Ă�
    public bool Is_FlagON_from_ID(int Parts_id)
    {
        int bitFlag = 1 << Parts_id;
        Debug.Log(Convert.ToString(partsData.Useavle_Flag, 2));
        Debug.Log(Convert.ToString(bitFlag, 2));
        Debug.Log((partsData.Useavle_Flag & bitFlag) == bitFlag);
        return (partsData.Useavle_Flag & bitFlag) == bitFlag;
    }

    // �t�@�C���p�X�̃`�F�b�N
    private void FilePathCheck()
    {
        if (DataPath == null)
        {
            DataPath = Path.Combine(Application.persistentDataPath, "PartsData.json");
        }
    }

    // JSON�t�@�C���̒l�������l�ɐݒ�
    public void Reset_JSONData()
    {
        partsData.SetDefaltStats();
    }

    // JSON�t�@�C���̃f�[�^�`�[�g
    public void All_data_open()
    {
        partsData.money = 9999999;
        partsData.Useavle_Flag = -1;
    }
}
