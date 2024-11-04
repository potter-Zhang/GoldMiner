using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{
    public float range;
    public Sprite[] list;

    private SpriteRenderer spriterenderer;
    private AudioSource myclip;
    public AudioClip[] audios;

    void Awake()
    {
        myclip = GetComponent<AudioSource>();
        spriterenderer = GetComponent<SpriteRenderer>();
        spriterenderer.sprite = list[0];
        
    }
    //public override void disabled()
    
        //GetComponent<BoxCollider2D>().enabled = false;
        //spriterenderer.sprite = list[1];
        //StartCoroutine(BombEffect());
    
    public override ItemInfo Catch()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        spriterenderer.sprite = list[1];
        StartCoroutine(BombEffect());
        return base.Catch();
    }

    IEnumerator BombEffect()
    {
        Vector3 one = new Vector3(2, 2, 2);
        myclip.clip = audios[Random.Range(0, audios.Length)];
        myclip.Play();
        GameManager.instance.miner.Pause(true);
        float timer = 0;
        for ( ; timer < 0.5f; )
        {
            timer += Time.deltaTime;
            transform.localScale += one * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        bomb();
        transform.localScale = new Vector3(1, 1, 1);
        spriterenderer.sprite = list[2];

    }

    IEnumerator TriggerBombEffect()
    {
        yield return BombEffect();
        Destroy(gameObject);
    }

    public void TriggerBomb()
    {
        if (GetComponent<BoxCollider2D>().enabled == false)
            return;
        GetComponent<BoxCollider2D>().enabled = false;
        spriterenderer.sprite = list[1];
        StartCoroutine(TriggerBombEffect());

    }

    void bomb()
    {
        RaycastHit2D hit;
        Debug.Log("bomb");
        
        for (int i = 0; i < 360; i += 30)
        {
            float rad = i * Mathf.Deg2Rad;
            hit = Physics2D.Raycast(transform.position, new Vector3(Mathf.Cos(i), Mathf.Sin(i), 0), range);
            if (hit.collider != null)
            {
                Debug.Log("hit one" + hit.collider.name);
                var refItem = hit.collider.gameObject.GetComponent<Item>();
                if (refItem != null)
                {
                    var trigger = hit.collider.gameObject.GetComponent<Bomb>();
                    if (trigger != null)
                    {
                        trigger.TriggerBomb();
                        
                    }
                        
                    else
                        Destroy(hit.collider.gameObject);
                }
                
            }
                
        }

        GameManager.instance.miner.Pause(false);
    }
    
}
