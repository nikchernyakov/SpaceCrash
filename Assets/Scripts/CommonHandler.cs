using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonHandler {
    public static bool IsRandomSaysTrue(float probability)
    {
        float randValue = Random.value;
        //Debug.Log(randValue);
        //randValue *= probability;

        return randValue < probability;
        //return true;
    }

}
