using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Dieble {

    public float shiftPower;

    private bool isAlive;
    private Rigidbody2D rb;

    public bool IsAlive()
    {
        return isAlive;
    }

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        //rb.velocity = new Vector2(2, 1.5f);

        isAlive = true;
        
    }

    void Update () {
       

	}

    public void SetVelocity(Vector2 direction)
    {
        rb.velocity = direction * shiftPower;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Obstacle)))
        {
            Die();
        }
    }

    protected override void Die()
    {
        /*rb.simulated = false;
        isAlive = false;*/
        FindObjectOfType<GameManager>().GameOver();
    }
}
