using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Live2D.Cubism.Framework.LookAt;
using Live2D.Cubism.Framework.Raycasting;
using Live2D.Cubism.Core;

public class BuildupWizard : EditorWindow
{
    public GameObject m_avator {
        get;
        private set;
    }
    CubismRaycaster m_rayCaster = null;
    CubismLookController m_lookController = null;

    bool m_isCollision;
    bool m_isLookAt;

    // メニューバーに追加
    [MenuItem("frefre/Build_Up")]
    
    // ウィンドウの生成
    public static void Build_Up()
    {
        var window = GetWindow<BuildupWizard>();
        window.titleContent = new GUIContent("Live2D 簡易セットアップ");
    }

    private void OnGUI()
    {
        m_avator = EditorGUILayout.ObjectField("アバター", m_avator, typeof(GameObject), true) as GameObject;
        if(m_avator == null) { return; }

        bool isAvatorFormat = isCubismAvator(m_avator);
        if (isAvatorFormat == false)
        {
            GUILayout.Box("Live2Dで出力したCubismアバターをアタッチしてください");
            return;
        }
        m_isCollision = EditorGUILayout.Toggle("当たり判定", m_isCollision);
        m_isLookAt = EditorGUILayout.Toggle("画面タップ先の追従", m_isLookAt);
        GUILayout.Space(2);
        DrawBorder();
        
        Configs(m_avator);

        if (GUILayout.Button("設定を適応") && isAvatorFormat)
        {
            OnValidate();
        }
    }

    //　現在のアバターにセットされているコンポーネントの状態出力
    private void Configs(GameObject avator)
    {
        if(avator == null) { return; }


        GUILayout.Box(avator.name + "現在の設定");

        m_rayCaster = avator.GetComponent<CubismRaycaster>();
        bool isCollisionSet = m_rayCaster != null;
        GUILayout.Box("当たり判定：" + (isCollisionSet ? "ON" : "OFF"));

        m_lookController = avator.GetComponent<CubismLookController>();
        bool isLookAtSet = m_lookController != null;
        GUILayout.Box("タップ先目線追従：" + (isLookAtSet ? "ON" : "OFF"));
    }

    // アバターの規格がCubismのものか判定
    private bool isCubismAvator(GameObject avator)
    {
        return avator.GetComponent<CubismModel>() != null;
    }

    // 罫線を引く
    private void DrawBorder()
    {
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
    }

    // コンポーネントの追加/削除
    private void OnValidate()
    {
        if (m_avator == null) { return; }

        // 当たり判定システム
        if (m_isCollision && m_rayCaster == null)
        {
            m_avator.AddComponent<CubismRaycaster>();
        }
        else if (m_isCollision == false)
        {
            EditorApplication.delayCall += ()=> DestroyImmediate(m_avator.GetComponent<CubismRaycaster>());
        }

        // タップ先追従システム
        if (m_isLookAt && m_lookController == null)
        {
            m_avator.AddComponent<CubismLookController>();
        }
        else if (m_isLookAt == false)
        {
            EditorApplication.delayCall += ()=> DestroyImmediate(m_avator.GetComponent<CubismLookController>());
        }

    }
}

public class ViewConfigs<T>
{
    public  string m_configName
    {
        private set;
        get;
    }

    public T m_configComponent
    {
        private set;
        get;
    }

    public ViewConfigs(string confName)
    {
        m_configName = confName;
    }
}

