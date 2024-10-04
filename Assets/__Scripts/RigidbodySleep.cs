using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(Rigidbody) )] //a
public class RigidbodySleep : MonoBehaviour
{
    private int sleepCountdown = 4; //b
    private Rigidbody rigid;

    void Awake(){
        rigid = GetComponent<Rigidbody>(); //c
    }

    void FixedUpdate(){
        if (sleepCountdown > 0 ){ //d
            rigid.Sleep();
            sleepCountdown--;
        }
    }
}
