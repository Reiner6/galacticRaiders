using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{


    public float speed;

    public int damage;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
    }

    /// <summary>
    /// this method destroys this object upon being outside of the cameras FOV
    /// </summary>

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
    /// <summary>
    /// upong trigger enter the enemy controller does damage and this object is destroyed
    /// </summary>

    private void OnTriggerEnter2D(Collider2D collision)
    {

        EnemyController temp = collision.gameObject.GetComponent<EnemyController>();
        if (temp != null)
        {
            temp.Damage(damage);
            Destroy(this.gameObject);
        }
    }
}