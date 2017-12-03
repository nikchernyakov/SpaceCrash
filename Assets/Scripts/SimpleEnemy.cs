using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour {

    private Vector2 direction;
    private Quaternion rotation;
    public float preemptionLength;

    private Vector3 target;
    public float speed;

    public float deadBottom;

    public void SetDeadBottom(float bottom)
    {
        deadBottom = bottom;
    }

    public void SetTarget(Vector3 targetPosition)
    {
        target = targetPosition;
    }

	// Use this for initialization
	void Start () {

        direction = (new Vector3(0, preemptionLength) + target) - transform.position;
        direction.Normalize();
        transform.rotation = Quaternion.FromToRotation(transform.position,
            (new Vector3(0, preemptionLength) + target) - transform.position);

    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < deadBottom)
        {
            Destroy(gameObject);
        }

        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}
