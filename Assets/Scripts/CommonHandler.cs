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

    public static bool IsObjectInCamera(Camera camera, Transform cameraTransform, Vector2 objectPosition)
    {
        float pixelHeight = camera.pixelHeight;
        Vector2 cameraTop = camera.ScreenToWorldPoint(new Vector2(0, pixelHeight));
        float height = cameraTop.y - cameraTransform.position.y;

        if (objectPosition.y < cameraTop.y && objectPosition.y > cameraTransform.position.y - height)
        {
            return true;
        }
        else
            return false;
    }

}
