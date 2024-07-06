using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tail : MonoBehaviour
{
    public GameObject bullet;
    public TrailRenderer trailRanderer; 
    // Start is called before the first frame update
    void Start()
    {
        ColorType color = bullet.GetComponent<BulletController>().bulletCollor;
        if(color == ColorType.Red)
        {
            trailRanderer.startColor = Color.red;
        }
        else if(color == ColorType.Yellow)
        {
            trailRanderer.startColor = Color.yellow;
        }
        else if (color == ColorType.Blue)
        {
            trailRanderer.startColor = Color.blue;
        }
        else if (color == ColorType.Purple)
        {
            Color purple = new Color(128f / 255f, 0f / 255f, 128f / 255f);
            trailRanderer.startColor = purple;
        }
        else if (color == ColorType.White)
        {
            trailRanderer.startColor = Color.white;
        }
    }

}
