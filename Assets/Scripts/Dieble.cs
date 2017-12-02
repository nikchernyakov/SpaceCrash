using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dieble : MonoBehaviour {

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
	
    public virtual void ReceiveKill()
    {
        Die();
    }
}
