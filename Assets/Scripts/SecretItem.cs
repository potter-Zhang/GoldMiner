using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SecretItem : Item
{
    delegate void Effect();
    private Effect effect;
    private bool special;
    private Text valueEffect;

    void Awake()
    {
        itemInfo.value = 10 * Random.Range(1, 31);
        //effect = Empty;
        special = false;
        valueEffect = GameManager.instance.miner.valueEffect;
    }

    
    void FireCrakerEffect()
    {
        GameManager.instance.miner.AddFireCraker();
        valueEffect.text = "加一个炸弹！";
    }

    void StrengthEffect()
    {
        GameManager.instance.miner.strength = 3;
        valueEffect.text = "大力！";
    }

    void GoldenClockEffect()
    {
        GameManager.instance.player_time += 5;
        valueEffect.text = "加多5秒！";
    }

    public override void Get()
    {
        if (special)
        {
            itemInfo.value = 0;
            effect();
        }
        base.Get();


    }


    public void LuckyPlantEffect()
    {
        itemInfo.value = 10 * Random.Range(40, 61);
        int choice = Random.Range(0, 10);
        if (choice < 3)
            special = true;
        if (choice == 0)
            effect = FireCrakerEffect;
        else if (choice == 1)
            effect = StrengthEffect;
        else if (choice == 2)
            effect = GoldenClockEffect;
        
    }

   

    
}
