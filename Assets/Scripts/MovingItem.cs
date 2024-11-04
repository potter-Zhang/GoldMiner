using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingItem : Item
{
    public float speed;
    public float[] x;
    public float[] y;
    public float length;
    public int dir;
    //public bool moveDirection;

    private Vector3 origin;
    private Vector3 target;
    
    private Animator animator;
    private bool pause;

    void Awake()
    {
        
        animator = GetComponent<Animator>();
        origin = transform.position;
        target = origin + new Vector3(length, 0, 0) * dir;
        animator.SetBool("moveDirection", transform.position.x < target.x);

    }

    void Update()
    {
        if (pause)
            return;

        if ((target - transform.position).sqrMagnitude > float.Epsilon)
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        else
        {
            transform.position = target;
            dir = -dir;
            target = origin + new Vector3(length, 0, 0) * dir;
            animator.SetBool("moveDirection", transform.position.x < target.x);
            
        }

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

    public void SetParam(float len, float spd, int d)
    {
        length = len;
        speed = spd;
        dir = d;
    }

    
}
