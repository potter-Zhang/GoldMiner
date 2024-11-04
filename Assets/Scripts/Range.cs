using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    void Awake()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.size = new Vector2(10 * Camera.main.aspect, 10);
        //Debug.Log(Camera.main.scaledPixelWidth + " " + Camera.main.scaledPixelHeight);
    }
}
