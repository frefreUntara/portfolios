using System.IO;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

// �Ґ��f�[�^��csv�f���o��
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
        Console.text = "�ۑ�����";
        Button.color = Default_Color;
    }

    public void CSVExporter_WAKEUP()
    {
       
        string[] writeData = { "�p�[�c�^�C�v", "�p�[�cID", "X���W", "Y���W" };
        convertCSV = string.Join(",", writeData);
        convertCSV += "\n";
        
        // TODO:�����̏���
        if (admin.Check_setPartsFormat())
        {
            if (cost_manager.Is_CostOver() == false)
            {
                Button.color = Saveable_Color;
                Console.text = "�ۑ���";
                ExpoteSE_Play();
                ConverttoCSV();
            }
            else
            {
                Button.color = SaveDisable_Color;
                Console.text = "�R�X�g�I�[�o�[";
            }
        }
        else
        {
            Debug.LogError("�R�A���P�����Z�b�g���Ă�������");

            Button.color = SaveDisable_Color;
            Console.text = "�R�A���P����\n�Z�b�g���Ă�������";
        }

        ButtonUIReset();
    }

    // �ݒu���ꂽ�p�[�c����ID���󂯎��ACSV�֓f���o��
    void ConverttoCSV()
    {
        List<GameObject> grids = admin.GetGrits();
        foreach (GameObject partsData in grids)
        {
            GridAdmin chell = partsData.GetComponent<GridAdmin>();
            int id = (int)chell.GetPartsID();
            Vector2 index = chell.GetGridIndex();

            // �󂯎����ID��[���ݒu]�ȊO�̏ꍇ����ID����������
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
        Console.text = "����";
    }
     
    // �G���[�f�������ʁAUI�̕\���ύX
    private async void ButtonUIReset()
    {
        await Task.Delay(1500);
        if (Button == null) { return; }
        Button.color = Default_Color;
        Console.text = "�ۑ�����";
    }

    private void ExpoteSE_Play()
    {
        audioPlayer.AudioPlay();
    }

}
