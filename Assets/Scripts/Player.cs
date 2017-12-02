using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Dieble {

    public Vector2 shiftPower;
    public float massEffect;

    private bool isAlive;
    private Rigidbody2D rb;
    private float summaryMass = 1;

    public void AddMass(float mass)
    {
        Debug.Log("Add mass: " + summaryMass);
        summaryMass += mass;
    }

    public void RemoveMass(float mass)
    {
        Debug.Log("Remove mass: " + summaryMass);
        summaryMass = Mathf.Clamp(summaryMass - mass, 1, Mathf.Infinity);
    }

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
        //float effect = summaryMass *
        if (direction.x != 0 || direction.y != 0)
            direction *= Mathf.Sqrt(2) / 2;
        rb.velocity = new Vector3(direction.x * shiftPower.x, direction.y * shiftPower.y) * (1 / summaryMass);
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
