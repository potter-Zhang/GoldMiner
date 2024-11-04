using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemSpawner : Item
{
    public Vector3[] targetList;
    public float speed;
    public GameObject[] itemToSpawn;

    private Vector3 target;
    private int index;
    private List<GameObject> refList;
    private GameObject[] myItems;
    private bool pause;
    private float scaler;

    void Awake()
    {
        index = 1;
        target = Vector3.zero;
        refList = GameManager.instance.levelCreator.itemList;
        pause = false;
        scaler = 5 * Camera.main.aspect / 9f;
        myItems = new GameObject[targetList.Length];
        target = targetList[index];
        target.x *= scaler;
    }

    public void SetParams(Vector3[] list, float spd, GameObject[] sp)
    {
        targetList = list;
        
        speed = spd;
        itemToSpawn = sp;
        myItems = new GameObject[list.Length];
        target = list[index];
        target.x *= scaler;
    }

    public override void Pause(bool p)
    {
        pause = p;
    }

    public override ItemInfo Catch()
    {
        Pause(true);
        return base.Catch();
    }

    // Update is called once per frame
    void Update()
    {
        if (pause)
            return;
        if ((transform.position - target).sqrMagnitude > float.Epsilon)
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        else
        {
            transform.position = target;
            if (myItems[index] == null)
            {
                var refItem = Instantiate(itemToSpawn[Random.Range(0, itemToSpawn.Length)], transform.position, Quaternion.identity);
                refList.Add(refItem);
                myItems[index] = refItem;
            }
            
            target = targetList[index = (index + 1) % targetList.Length];
            target.x *= scaler;
        }
            
    }
}
