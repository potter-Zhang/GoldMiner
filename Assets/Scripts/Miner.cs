using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Miner : MonoBehaviour
{
    public float deltaDeg;
    public float radius;
    public float strength;
    public int doubleMagic;
    public GameObject fireCraker;
    
    delegate void Action();

    private bool isShooting;
    private bool isPulling;
    private bool hasDestroyed;

    private float deg;
    private float deltaRad;
    private float rad;
    public Text valueEffect;
    
    public Vector3 origin;
    public AudioClip[] audios;
    private AudioSource myaudio;
    private Vector3 target;
    private Vector3 step;

    private ItemInfo pullingItemInfo;
    private float timer;
    private float getTime;

    private Action action;

    private LineRenderer line;
    public GameObject pullingItem;
    private Animator player;
    private List<GameObject> fireCrakers;
    private BoxCollider2D boxCollider;

    private bool pause;
    
    void Awake()
    {
        isPulling = false;
        myaudio = GetComponent<AudioSource>();
        fireCrakers = new List<GameObject>();
        boxCollider = GetComponent<BoxCollider2D>();
        line = GetComponent<LineRenderer>();
        isShooting = false;
        timer = 0;
        pullingItemInfo = new ItemInfo(0, 3);
        getTime = 0.3f;
        
        origin = transform.position;
        
        strength = 1;
        doubleMagic = 1;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, origin);
        
        deltaRad = deltaDeg * Mathf.Deg2Rad;
        
        action = Sway;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        valueEffect = GameObject.Find("ValueEffect").GetComponent<Text>();
        valueEffect.gameObject.SetActive(false);
        pause = false;
    }

    public void Pause(bool p)
    {
        pause = p;
        Debug.Log("pause = " + p);
        
    }

    public int getNumOfFireCrakers()
    {
        return fireCrakers.Count;
    }
    public void Reset()
    {
        pullingItemInfo = new ItemInfo(0, 3);
        boxCollider.enabled = true;
        transform.position = origin + radius * new Vector3(Mathf.Cos(rad), -Mathf.Sin(rad), 0f);
        action = Sway;
        valueEffect.color = new Vector4(0, 0, 0, 0);

        
        
        isShooting = false;
        pause = false;
    }

    void Sway()
    {
       
        rad = deg * Mathf.Deg2Rad;
        transform.position = origin + radius * new Vector3(Mathf.Cos(rad), -Mathf.Sin(rad), 0f);
        transform.rotation = Quaternion.Euler(0, 0, 90 - deg);
        deg += deltaDeg * Time.deltaTime;
        if (deg >= 180 || deg <= 0)
        {
            if (deg >= 180)
                deg = 180;
            else
                deg = 0;
            deltaDeg = -deltaDeg;
        }

    }

    void Shoot()
    {

        
        transform.position += step * Time.deltaTime;
        
        
    }

    void Pull()
    {
        //Debug.Log("enter pull");
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * pullingItemInfo.reverseWeight * strength);
        if (pullingItem != null)
        {
            pullingItem.transform.position = transform.position + step;
            
        }
            
        if ((target - transform.position).sqrMagnitude <= float.Epsilon)
        {
            if (pullingItem != null && !hasDestroyed)
            {
                pullingItem.GetComponent<Item>().Get();
                Destroy(pullingItem);
                Debug.Log("destroy item");
            } else
            {
                pullingItemInfo.value = 0;
            }

            hasDestroyed = false;
            pullingItem = null;
            isPulling = false;
            //timer = 0;
            if (pullingItemInfo.value < 50)
            {
                player.SetTrigger("Mad");
                myaudio.clip = audios[Random.Range(0, 2)];
                myaudio.Play();
            }    
            else
            {
                
                player.SetTrigger("Laugh");
                myaudio.clip = audios[Random.Range(2, 5)];
                myaudio.Play();
            }
            ValueEffect();
            valueEffect.text = "$" + pullingItemInfo.value;
            
            action = Get;
        }
            
        //Debug.Log("exit pull");

    }
    void Stop()
    {
            
            GetComponent<BoxCollider2D>().enabled = false;
            
            action = Pull;
            isPulling = true;
        
    }
    void Get()
    {
        timer += Time.deltaTime;
        if (timer > getTime)
        {
            //Debug.Log("enter Get");
            boxCollider.enabled = true;
            
          
            Debug.Log("get: time = " + getTime);
            //.Log("value = " + value);
            
            
            pullingItemInfo = new ItemInfo(0, 3);
     
            
            //GetComponent<BoxCollider2D>().enabled = true;
            timer = 0;
            
            action = Sway;
            isShooting = false;
        }
    }
    public void GetUserInput()
    {
        if (!isShooting)
        {
            target = transform.position;
            step = new Vector3(Mathf.Cos(rad), -Mathf.Sin(rad), 0f) * 3;
            isShooting = true;
            action = Shoot;
        } 
        else if (isPulling && pullingItem != null)
        {
            Debug.Log("throw one");
            ThrowFireCraker(pullingItem);
            
        }
    }

    public void AddFireCraker()
    {
        fireCrakers.Add(Instantiate(fireCraker, origin + new Vector3((1 + fireCrakers.Count) * 0.5f, 0.5f, 0), Quaternion.identity));
        
    }

    public void ThrowFireCraker(GameObject target)
    {

        if ((pullingItemInfo.value / doubleMagic != 10 && pullingItemInfo.value / doubleMagic != 20) || fireCrakers.Count == 0)
            return;
        Debug.Log("throw");
        pullingItem = null;
        hasDestroyed = true;
        //stopTime = 1;
        pullingItemInfo = new ItemInfo(0, 3);
        
        action = Stop;
        
        fireCrakers[fireCrakers.Count - 1].transform.position = origin;
        
        GameObject tmp = fireCrakers[fireCrakers.Count - 1];
        fireCrakers.RemoveAt(fireCrakers.Count - 1);
        StartCoroutine(tmp.GetComponent<FireCraker>().Throw(target));
        
        
    }

    
    
    
    void ValueEffect()
    {
        Debug.Log("valueeffect");
        valueEffect.rectTransform.anchoredPosition3D = new Vector3(0, -30, 0);
        
        valueEffect.gameObject.SetActive(true);
        StartCoroutine(ValueFade());

    }

    IEnumerator ValueFade()
    {
        float timer = 0;
        for ( ; timer < 1.5f; )
        {
            timer += Time.deltaTime;
            valueEffect.rectTransform.anchoredPosition3D += 5 * Vector3.up * Time.deltaTime;
            valueEffect.color = new Vector4(0, 0, 0, 1 - timer / 1.5f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //valueEffect.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (pause)
            return;
        action();
        line.SetPosition(0, transform.position);

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        Item item = other.gameObject.GetComponent<Item>();
        if (item == null || pullingItem != null || !isShooting)
            return;
        boxCollider.enabled = false;

        Debug.Log("hit");
        
        action = Stop;
        pullingItemInfo = item.Catch();
        pullingItemInfo.value *= doubleMagic;
        
        pullingItem = other.gameObject;
        
        pullingItem.transform.rotation = transform.rotation;
        //if (other.gameObject.transform.localScale.x > 1f && other.gameObject.transform.localScale.y > 1.5f)
        //step *= 0.33f;
        /*else */
        if (other.gameObject.transform.localScale.y > 1.5f)
            step *= 0.25f;
        else
            step *= 0.12f;
        
        //Destroy(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        
        action = Stop;
    }
}
