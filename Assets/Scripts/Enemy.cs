using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Dieble {

    private Vector2 direction;
    private Quaternion rotation;
    public float preemptionLength;

    private bool isMove = true;
    public float deadBottom;

    public float speed;
    public float mass;

    private Player player;

    public void SetDeadBottom(float bottom)
    {
        deadBottom = bottom;
    }

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        direction = (new Vector3(0, preemptionLength) + player.transform.position) - transform.position;
        direction.Normalize();
        transform.rotation = Quaternion.FromToRotation(transform.position,
            (new Vector3(0, preemptionLength) + player.transform.position) - transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        
        if(transform.position.y < deadBottom)
        {
            Destroy(gameObject);
        }

        if(isMove)
            transform.position += (Vector3) direction * speed * Time.deltaTime;
        else
        {
            //transform.position = positionTransform.position;
        }
	}

    public void ConnectTo(Collider2D collider)
    {
        isMove = false;
        //positionTransform.parent = collider.transform;
        transform.parent = collider.transform;
    }

    protected override void Die()
    {
        if(!isMove)
            player.RemoveMass(mass);
        base.Die();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isMove && collider.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Enemy)))
        {
            ConnectTo(collider);
        }
        else if(isMove && collider.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Player)))
        {
            ConnectTo(collider);
            player.AddMass(mass);
        }
        else if(collider.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Obstacle)))
        {
            Die();
        }
    }
}
