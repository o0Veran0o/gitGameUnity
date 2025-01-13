using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 10f;
    public int damage = 20;
    public Rigidbody2D rb;


    private void Update()
    {
        
    }

    
    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.name);
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null){
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
