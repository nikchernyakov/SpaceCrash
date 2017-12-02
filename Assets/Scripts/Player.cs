using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Dieble {

    public float jumpPowerX;
    public float jumpPowerY;
    public int energyAmount;

    public PowerLevel power;

    private bool isAlive;
    private Rigidbody2D rb;
    private int currentEnergyAmount;

    public bool IsAlive()
    {
        return isAlive;
    }

    public bool HasEnergy()
    {
        return currentEnergyAmount > 0;
    }

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(2, 1.5f);

        isAlive = true;
        currentEnergyAmount = energyAmount;
        
    }

    void Update () {
        // Rotate transform by speed vector
        transform.rotation = Quaternion.FromToRotation(transform.position, rb.velocity);

	}

    public void Jump()
    {
        rb.velocity += new Vector2(jumpPowerX, jumpPowerY);
        currentEnergyAmount--;
    }


    public void Clash(Obstacle obstacle)
    {
        if (CheckClash(obstacle))
        {
            obstacle.ReceiveKill();
        }
        else
        {
            Die();
        }
    }

    public bool CheckClash(Obstacle obstacle)
    {
        return power >= obstacle.hp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Obstacle)))
        {
            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
            if(obstacle != null)
                Clash(obstacle);
        }
    }

    protected override void Die()
    {
        rb.simulated = false;
        isAlive = false;
    }
}
