using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

     public int damage;
   

    private void OnTriggerEnter2D(Collider2D other)
    {
       gameObject.SetActive(false);
    }
}
