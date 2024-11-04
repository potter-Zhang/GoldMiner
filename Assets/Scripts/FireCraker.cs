using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCraker : MonoBehaviour
{
    public Sprite[] list;
    public GameObject[] diamond;
    private SpriteRenderer spriterenderer;

    void Awake()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        spriterenderer.sprite = list[0];
    }

    public IEnumerator Throw(GameObject target)
    {
        Vector3 step = (target.transform.position - transform.position).normalized * 1.5f;
        while ((transform.position - target.transform.position).sqrMagnitude > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 8);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(target);
        spriterenderer.sprite = list[1];
        yield return new WaitForSeconds(0.5f);
        float deg = Random.Range(0, 2) == 0 ? 45 : -45;
        float rad = deg * Mathf.Deg2Rad;
        float x = step.x * Mathf.Cos(rad) - step.y * Mathf.Sin(rad);
        float y = step.x * Mathf.Sin(rad) + step.y * Mathf.Cos(rad);
        step = new Vector3(x, y, 0);
        GameManager.instance.levelCreator.itemList.Add(Instantiate(diamond[Random.Range(0, diamond.Length)], transform.position + step, Quaternion.identity));
        Destroy(gameObject);
    }
}
