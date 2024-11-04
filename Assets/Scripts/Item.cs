using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    public int value;
    public float reverseWeight;

    public ItemInfo(int v, float rw)
    {
        value = v;
        reverseWeight = rw;
        
    }
}

public class Item : MonoBehaviour
{
    public ItemInfo itemInfo;

    

    public virtual ItemInfo Catch()
    {
        return itemInfo;
    }

    public virtual void Get()
    {
        GameManager.instance.player_money += itemInfo.value;
    }

    public virtual void Pause(bool p)
    {
        
    }
}
