using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReplacer : Item
{
    public GameObject itemToExchange;
    //public int divider;
    public Vector3 EndPoint;

    private List<GameObject> refList;
    private int index;
    private GameObject target;
    private bool pause;
    public int value;
    

    void Awake()
    {
        refList = GameManager.instance.levelCreator.itemList;
        index = 0;
        //target = refList[0];
        pause = false;
        
    }

    public override ItemInfo Catch()
    {
        Pause(true);
        return base.Catch();
    }

    public void SetParams(Vector3 end, GameObject it, int v)
    {
        EndPoint = end;
        itemToExchange = it;
        value = v;
    }

    void Search()
    {
        for (; index < refList.Count; index++)
        {
            if (refList[index] != null && refList[index] != GameManager.instance.miner.pullingItem && refList[index].GetComponent<Item>().itemInfo.value == value)
            {
                target = refList[index];
                break;
            }     
        }
        if (index == refList.Count)
        {
            Debug.Log("ded");
            target = null;
            pause = true;
            StartCoroutine(GoToEnd());
        }
            

        
    }

    IEnumerator GoToEnd()
    {
        while ((transform.position - EndPoint).sqrMagnitude >= 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, EndPoint, 0.5f * Time.deltaTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        DestroyImmediate(this.gameObject);
    }

    public override void Pause(bool p)
    {
        pause = p;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause)
            return;

        if (target == null)
            Search();
        else if ((transform.position - target.transform.position).sqrMagnitude > 0.05f)
        {
            if (refList[index] != GameManager.instance.miner.pullingItem)
            {
                Debug.Log("gos");
                transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.5f * Time.deltaTime);
            } 
            else
            {
                target = null;
            }
            
        }
        else
        {
            Debug.Log("catch");
            itemInfo.value += target.GetComponent<Item>().itemInfo.value / 2;
            transform.position = target.transform.position;
            Destroy(target);
            refList.Add(Instantiate(itemToExchange, transform.position, Quaternion.identity));
        }
        
        
        
    }
}
