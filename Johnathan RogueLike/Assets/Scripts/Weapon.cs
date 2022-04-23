using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Animator weapon;
    // Start is called before the first frame update
    void Start()
    {
        weapon = this.GetComponent<Animator>();
    }
    void PlaySword()
    {
        
    }
}
