using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelCreator : MonoBehaviour
{
    public GameObject[] levelInfo;
    public GameObject player;
    public GameObject miner;
    public GameObject wall;
    public GameObject UI;
    public GameObject[] listOfItem;
    private bool lucky;

    public List<GameObject> itemList;
    private List<GameObject> necessityList;
    private float scaler;

    

    
    // Start is called before the first frame update
    void Awake()
    {
        itemList = new List<GameObject>();
        necessityList = new List<GameObject>();
        necessityList.Add(Instantiate(UI, new Vector3(0, 0, 0), Quaternion.identity));
        necessityList.Add(Instantiate(player, new Vector3(0, 3.8f, 0), Quaternion.identity));

        necessityList.Add(Instantiate(wall, Vector3.zero, Quaternion.identity));
        necessityList.Add(Instantiate(miner, new Vector3(0, 3, 0), Quaternion.identity));
        lucky = false;
        scaler = 5 * Camera.main.aspect / 9f;
        
    }

    public void LuckyEffect()
    {
        lucky = true;
    }

    
    public void CreateLevel()
    {
        
            LevelInfo level = levelInfo[(GameManager.instance.level - 1) % levelInfo.Length].GetComponent<LevelInfo>();
            if (level.NItems != null)
                for (int i = 0; i < level.NItems.Length; i++)
                {
                    itemList.Add(Instantiate(listOfItem[level.NItems[i].ID], new Vector3(level.NItems[i].x * scaler, level.NItems[i].y , 0), Quaternion.identity));
                }
            if (level.MItems != null)
                for (int i = 0; i < level.MItems.Length; i++)
                {
                    itemList.Add(Instantiate(listOfItem[level.MItems[i].ID], new Vector3(level.MItems[i].x * scaler, level.MItems[i].y, 0), Quaternion.identity));
                    itemList[itemList.Count - 1].GetComponent<MovingItem>().SetParam(level.MItems[i].length * scaler, level.MItems[i].speed, level.MItems[i].dir);
                }
            if (level.SItems != null)
                for (int i = 0; i < level.SItems.Length; i++)
                {
                    itemList.Add(Instantiate(listOfItem[10], new Vector3(level.SItems[i].x * scaler, level.SItems[i].y, 0), Quaternion.identity));
                    if (lucky)
                    {
                        itemList[itemList.Count - 1].GetComponent<SecretItem>().LuckyPlantEffect();
                    }
                }
            if (level.GItems != null)
                for (int i = 0; i < level.GItems.Length; i++)
                {
                    itemList.Add(Instantiate(listOfItem[13], level.GItems[i].target[0], Quaternion.identity));
                    itemList[itemList.Count - 1].GetComponent<ItemSpawner>().SetParams(level.GItems[i].target, level.GItems[i].speed, level.GItems[i].itemToSpawn);
                }
            if (level.RItems != null)
                for (int i = 0; i < level.RItems.Length; i++)
                {
                    itemList.Add(Instantiate(listOfItem[14], level.RItems[i].startPoint * scaler, Quaternion.identity));
                    itemList[itemList.Count - 1].GetComponent<ItemReplacer>().SetParams(level.RItems[i].endPoint * scaler, level.RItems[i].itemToExchange, level.RItems[i].value);
                }
        lucky = false;
        
        
    }

    public void Pause(bool p)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            var refItem = itemList[i];
            if (refItem != null)
            {
                Item tmp = refItem.GetComponent<Item>();
                if (tmp != null)
                    tmp.Pause(p);
                
            }
                
        }
        

    }

    public void Clear()
    {
        Debug.Log("Clear()");
        for (int i = 0; i < itemList.Count; i++)
            if (itemList[i] != null)
            {
                Debug.Log("destroy one");
                Destroy(itemList[i]);
            }
        for (int i = 0; i < necessityList.Count - 1; i++)
            necessityList[i].SetActive(false);
        StartCoroutine(ResetMiner());
        
    }

    IEnumerator ResetMiner()
    {
        necessityList[3].GetComponent<Miner>().Reset();
        yield return new WaitForSeconds(3);
        necessityList[3].SetActive(false);
    }

    public void Show()
    {
        for (int i = 0; i < necessityList.Count; i++)
            necessityList[i].SetActive(true);
    }

 
    
}
