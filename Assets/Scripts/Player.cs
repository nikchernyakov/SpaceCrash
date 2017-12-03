using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Dieble {

    public Vector2 shiftPower;
    public float massEffect;

    private bool isAlive;
    private Rigidbody2D rb;
    private float summaryMass = 1;

    private GameManager gameManager;

    public void KillEnemy(Enemy enemy)
    {
        RemoveMass(enemy.mass);

        gameManager.AddScoreForKill();
    }

    public void AddMass(float mass)
    {
        Debug.Log("Add mass: " + mass);
        summaryMass += mass;
    }

    public void RemoveMass(float mass)
    {
        //Debug.Log("Remove mass: " + summaryMass);
        summaryMass = Mathf.Clamp(summaryMass - mass, 1, Mathf.Infinity);
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        isAlive = true;

        gameManager = FindObjectOfType<GameManager>();
        
    }

    void Update () {
       

	}

    public void SetVelocity(Vector2 direction)
    {

        if (direction.x != 0 || direction.y != 0)
            direction *= Mathf.Sqrt(2) / 2;

        float shiftPowerY = shiftPower.y;

        if(direction.y < 0)
        {
            shiftPowerY *= 2;
        }
        else
        {
            shiftPowerY *= 1.5f;
        }
        
        rb.velocity = new Vector3(direction.x * shiftPower.x, direction.y * shiftPowerY) * (1 / summaryMass);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Obstacle)))
        {
            Die();
        }
        else if (collision.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Enemy)))
        {
            
        }


    }

    protected override void Die()
    {
        /*rb.simulated = false;
        isAlive = false;*/
        gameManager.GameOver();
    }
}
