using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ゲーム内の編成コストチェック
public class SetCost_Checker : MonoBehaviour
{
    [SerializeField]
    private int MaxCost;

    [SerializeField]
    private int Now_useCost = 0;

    [SerializeField]
    private Text costtext;

    [SerializeField]
    private Color cost_under = Color.white;

    [SerializeField]
    private Color cost_over = Color.red;

    private void Start()
    {
        SetText();
    }

    public void Add_Cost(int cost)
    {
        if (cost < 0) { return; }
        Now_useCost += cost;
        SetText();
    }

    public void RemoveCost(int cost)
    {
        if (cost < 0) { return; }
        Now_useCost -= cost;
        SetText();
    }

    public void AddMaxCost(int max_cost)
    {
        if (max_cost < 0) { return; }
        MaxCost += max_cost;
    }

    private void SetText()
    {
        if (costtext == null) { return; }
        if (Is_CostOver())
        {
            costtext.color = cost_over;
        }
        else
        {
            costtext.color = cost_under;
        }

        costtext.text = Now_useCost + "/" + MaxCost;
    }

    public bool Is_CostOver()
    {
        return Now_useCost > MaxCost;
    }

}
