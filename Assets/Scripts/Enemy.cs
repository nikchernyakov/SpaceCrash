using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Dieble {

    private Vector2 direction;
    private Quaternion rotation;
    public Transform positionTransform;

    private bool isMove = true;
    public float deadBottom;

    public float speed;
    public float mass;

    public void SetDeadBottom(float bottom)
    {
        deadBottom = bottom;
    }

	// Use this for initialization
	void Start () {
        Player player = FindObjectOfType<Player>();
        direction = player.transform.position - transform.position;
        direction.Normalize();
        transform.rotation = Quaternion.FromToRotation(transform.position, player.transform.position - transform.position);
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Player)))
        {
            ConnectTo(collider);
        }
        else if(collider.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Enemy)))
        {
            ConnectTo(collider);
        }
        else if(collider.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Obstacle)))
        {
            Die();
        }
    }
}
