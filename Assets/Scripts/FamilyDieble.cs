using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyDieble : Dieble {

    public FamilyDieble parent;
    public FamilyDieble child;

    protected virtual void Update()
    {
        transform.position = parent.transform.position;

    }
}
