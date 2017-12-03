using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Dieble {

    private Vector2 direction;
    private Quaternion rotation;
    public float preemptionLength;

    private Vector2 target;

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
        rotation = Quaternion.FromToRotation(transform.position,
            (new Vector3(0, preemptionLength) + player.transform.position) - transform.position);

        //transform.rotation = rotation;
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
       // Vector3 previousScale = transform.localScale;

        isMove = false;
        transform.parent = collider.transform;

    }

    protected override void Die()
    {
        transform.parent = null;

        if(!isMove)
            player.KillEnemy(this);
        base.Die();
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isMove && collider.gameObject.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Enemy)))
        {
            //Debug.Log("Enemies crash");
            if (collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.Equals(transform))
                return;
            ConnectTo(collider);
        }
        else if(isMove && collider.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Player)))
        {
            //Debug.Log("Player crash");
            ConnectTo(collider); 
        }
        else if(collider.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Obstacle)))
        {
            Die();
        }
    }
}
