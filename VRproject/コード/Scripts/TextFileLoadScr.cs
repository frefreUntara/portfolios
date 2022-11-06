using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �X�N���v�g�����̃R�}���h�敪��
enum OrderState
{
    MAINCOMAND, // Text�AScene�Ȃ�
    SUBCOMAND,  // �L�����\���A�V�[���J�ڂȂ�
    ORDERDATA   // ��b�{���A�J�ڐ�̃V�[�����w��Ȃ�
}

// ���݂̃X�N���v�g�̃X�e�[�g�ꗗ
public enum LoadState
{
    NONE,
    LOADING,
    COMPLETE,
}

/// <summary>
/// NPC�L�����̉�b�C�x���g�̊���ƃe�L�X�g��z��ŊǗ�
/// </summary>
public class TextFileLoadScr
{
    /// ���݂̍s
    private int CurrentRow = 0;
    public string[,] OrderDetails;
    public LoadState loadState = LoadState.NONE;

    /// <summary>
    /// �e�L�X�g�t�@�C����ǂݍ��݁A����ƃe�L�X�g��z��ŕۑ�
    /// </summary>
    /// <param name="filePath">Resources�t�@�C�����J�����g�Ƃ����ǂݍ��ݑΏۂ̃t�@�C���p�X</param>
    public void CreateText(string filePath)
    {
        Reset();

        // ���s�ŋ�؂����f�[�^�i�[
        string[] LoadTexts;

        // Resources�t�@�C������e�L�X�g�t�@�C���ǂݍ���
        var LoadData = Resources.Load<TextAsset>(filePath).text;

        // ���s���ƂɃe�L�X�g��؂蕪���Ċi�[
        LoadTexts = LoadData.Split('\n');

        int neo_arraySize = 0;
        foreach (string text in LoadTexts)
        {
            // �e�L�X�g�f�[�^�Ő������e�L�X�g���J�E���g
            if (FindComand(text))
            {
                neo_arraySize++;
            }
        }

        // �X�N���v�g�̃T�C�Y�ɍ��킹��string�z��\�z
        OrderDetails = new string[neo_arraySize, 3];

        // ����ƃe�L�X�g�z��̃C���f�b�N�X
        int Emotion_Index = 0;

        foreach (var CurrtneDeta in LoadTexts)
        {
            // �ǂݍ��񂾍s���w�肵�������Ƃ��Đ������Ă��邩�`�F�b�N
            if (FindComand(CurrtneDeta))
            {
                string[] ComandData = CurrtneDeta.Split('/');
                OrderDetails[Emotion_Index, (int)OrderState.MAINCOMAND] = ComandData[0];
                OrderDetails[Emotion_Index, (int)OrderState.SUBCOMAND] = ComandData[1].Split('>')[0];
                OrderDetails[Emotion_Index, (int)OrderState.ORDERDATA] = ComandData[1].Split('>')[1];
                Emotion_Index++;
            }
        }
        loadState = LoadState.LOADING;
    }
    /// <summary>
    /// �s�̍\���������ʂ肩�`�F�b�N
    /// </summary>
    /// <param name="SeachTarget"></param>
    /// <returns></returns>
    private bool FindComand(string SeachTarget)
    {
        return -1 != SeachTarget.IndexOf('>');
    }
    /// <summary>
    /// ���݂̃X�N���v�g�̃��C���R�}���h�擾
    /// </summary>
    public string GetCurrent_MainComand()
    {
        if (IsRowOver() == false)
        {
            return OrderDetails[CurrentRow, (int)OrderState.MAINCOMAND];
        }
        else
        {
            return null;
        }

    }
     /// <summary>
    /// ���݂̃X�N���v�g�̃T�u�R�}���h�擾
    /// </summary>
    public string GetCurrent_SubComand()
    {
        if (IsRowOver() == false)
        {
            return OrderDetails[CurrentRow, (int)OrderState.SUBCOMAND];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// �\���Ώۂ̕������z�񂩂�擾
    /// </summary>
    /// <returns></returns>
    public string GetCurrentRowText()
    {
        if (IsRowOver() == false)
        {
            return OrderDetails[CurrentRow, (int)OrderState.ORDERDATA];
        }
        else
        {
            return null;
        }
    }
    
    /// <summary>
    /// �J�����g�s���ړ�
    /// </summary>
    public void NextRow()
    {
        if (IsRowOver() == false)
        {
            CurrentRow++;
        }
    }
    /// <summary>
    /// �S�s�o�͂���������������
    /// </summary>
    /// <returns></returns>
    public bool IsRowOver()
    {
        if (OrderDetails == null) { return true; }
        // return CurrentRow >= Text.Length;
        return CurrentRow >= OrderDetails.GetLength(0);
    }

    /// <summary>
    /// ������Ԃɂ���B
    /// ���݂̓ǂݍ��ݑΏۂ�0�s�ڂɂ���
    /// </summary>
    private void Reset()
    {
        CurrentRow = 0;
    }
}

