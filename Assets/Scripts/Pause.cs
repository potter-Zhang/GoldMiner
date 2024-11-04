using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool pause = false;

    public virtual void SetPause()
    {
        pause = !pause;
    }

}
