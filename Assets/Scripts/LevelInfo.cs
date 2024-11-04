using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NItem
{
    public int ID;
    public float x;
    public float y;
    
}

[System.Serializable]
public class SItem
{
    public float x;
    public float y;

}

[System.Serializable]
public class MItem
{
    public int ID, dir;
    public float x;
    public float y;
    public float length, speed;
}

[System.Serializable]
public class GItem
{
    
    public Vector3[] target;
    public float speed;
    public GameObject[] itemToSpawn;
}

[System.Serializable]
public class RItem
{ 
    public Vector3 startPoint;
    
    public Vector3 endPoint;
    public GameObject itemToExchange;
    public int value;
}



public class LevelInfo : MonoBehaviour
{
    public NItem[] NItems;
    
    public MItem[] MItems;
    
    public SItem[] SItems;

    public GItem[] GItems;

    public RItem[] RItems;
}
