using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Live2D.Cubism.Framework.LookAt;
using Live2D.Cubism.Framework.Raycasting;
using Live2D.Cubism.Core;


public enum INDENT_LEVEL
{
    THEME = 0,
    MAIN = 1,
    SUB  = 2,
    CONF = 3,
}

public class CollisionWizard : EditorWindow
{
    public GameObject m_avator
    {
        get;
        private set;
    }

    private string[] m_collisionType = { "Bounding BOX", "Triangles" };

    GameObject m_drawAbles = null;
    CubismRaycaster m_rayCaster = null;
    private Vector2 m_scrollPos = Vector2.zero;
    // 読み取り専用
    Dictionary<string, bool> m_ColliderConfigs = new Dictionary<string, bool>();
    // 書き込み専用
    Dictionary<string, CubismRaycastablePrecision> m_ColliderSystems = new Dictionary<string, CubismRaycastablePrecision>();

    // メニューバーに追加
    [MenuItem("frefre/Collision_Build_Up")]
    public static void Collision_Build_Up()
    {
        var window = GetWindow<ColisionWizard>();
        window.titleContent = new GUIContent("Live2D 当たり判定レイヤー設定");
    }

    private void OnGUI()
    {
        m_avator = EditorGUILayout.ObjectField("アバター", m_avator, typeof(GameObject), true) as GameObject;

        if (m_avator == null) { return; }
        
        m_drawAbles = m_avator.transform.Find("Drawables").gameObject;

        bool isAvatorFormat = isCubismAvator(m_avator);
        if (isAvatorFormat == false)
        {
            EditorGUILayout.LabelField("Live2Dで出力したCubismアバターをアタッチしてください");
            return;
        }
        
        Configs();
    }

    // アバターの規格がCubismのものか判定
    private bool isCubismAvator(GameObject avator)
    {
        return avator.GetComponent<CubismModel>() != null;
    }

    //　現在のアバターにセットされているコンポーネントの状態出力
    private void Configs()
    {
        if (m_avator == null) { return; }

        EditorGUILayout.LabelField(m_avator.name + "現在の設定");

        EditorGUI.indentLevel= (int)INDENT_LEVEL.MAIN;
        m_rayCaster = m_avator.GetComponent<CubismRaycaster>();
        bool isCollisionSet = m_rayCaster != null;
        EditorGUILayout.LabelField("当たり判定：" + (isCollisionSet ? "ON" : "OFF"));
        
        m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos);
        LoadDrawablesObj();
        ViewDrawablesConf();
        EditorGUILayout.EndScrollView();
        LoadDrawablesObj();
    }

    // コンポーネントの読み取り
    private void LoadDrawablesObj()
    {
        if (m_avator == null) { return; }

        for (int i = 0; i < m_drawAbles.transform.childCount; i++)
        {
            string objName = m_drawAbles.transform.GetChild(i).name;
            var cubismRaycaster = m_drawAbles.transform.GetChild(i).GetComponent<CubismRaycastable>();
            bool isCollisionable = cubismRaycaster != null;
            if (m_ColliderConfigs.ContainsKey(objName))
            {
                m_ColliderConfigs[objName] = isCollisionable;
            }
            else
            {
                m_ColliderConfigs.Add(objName, isCollisionable);
            }

            if (isCollisionable)
            {
                if (m_ColliderSystems.ContainsKey(objName))
                {
                    m_ColliderSystems[objName] = cubismRaycaster.Precision;
                }
                else
                {
                    m_ColliderSystems.Add(objName, cubismRaycaster.Precision);
                }
            }
        }
    }

    // 表示＋GUI操作
    private Dictionary<string, bool> ViewDrawablesConf()
    {
        if (m_avator == null) { return null; }


        for (int i = 0; i < m_drawAbles.transform.childCount; i++) {
            string objName = m_drawAbles.transform.GetChild(i).name;
            var cubismRaycaster = m_drawAbles.transform.GetChild(i).GetComponent<CubismRaycastable>();
            bool isCollisionable = cubismRaycaster != null;

            EditorGUI.indentLevel = (int)INDENT_LEVEL.SUB;
            EditorGUILayout.LabelField(objName);

            EditorGUI.indentLevel = (int)INDENT_LEVEL.CONF;
            m_ColliderConfigs[objName] = EditorGUILayout.Toggle(objName + " 当たり判定：", m_ColliderConfigs[objName]);

            if (m_ColliderSystems.ContainsKey(objName) && m_ColliderConfigs[objName])
            {
                m_ColliderSystems[objName] = (CubismRaycastablePrecision)EditorGUILayout.Popup((int)m_ColliderSystems[objName], m_collisionType);
            }

            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            GUILayout.Space(2);
        }

        foreach (var config in m_ColliderConfigs)
        {
            Transform addLayer = m_drawAbles.transform.Find(config.Key);
            CubismRaycastable cubiRay = addLayer.GetComponent<CubismRaycastable>();
            if (config.Value)
            {
                if (cubiRay == null)
                {
                    cubiRay = addLayer.gameObject.AddComponent<CubismRaycastable>();
                }

                if (m_ColliderSystems.ContainsKey(config.Key)) {
                    cubiRay.Precision = m_ColliderSystems[config.Key];
                }
            }
            else
            {
                EditorApplication.delayCall += () => DestroyImmediate(cubiRay);
            }
        }

            return null;

    }
}
