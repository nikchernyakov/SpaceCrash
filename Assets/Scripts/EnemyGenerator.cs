using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    public Enemy enemy;
    [Range(0, 1)]
    public float enemyProbability;
    public GameObject enemyContainer;
    public float enemyGenerateReserve = 2f;
    [Range(0, 90)]
    public float generatePointAngle = 0;

    private Camera camera;

    private float cameraRadius;
    private float cameraBottom;

    public void SetCamera(Camera camera)
    {
        this.camera = camera;
    }

    public void SetCameraRadius(float radius)
    {
        cameraRadius = radius;
    }

    public float GetCameraBottom()
    {
        float pixelHeight = camera.pixelHeight;
        Vector2 cameraTop = camera.ScreenToWorldPoint(new Vector2(0, pixelHeight));
        float height = cameraTop.y - camera.transform.position.y;

        return camera.transform.position.y - height;
    }

    // Use this for initialization
    void Start () {
	}

	// Update is called once per frame
	void Update () {
        
	}

    public void GenerateEnemy()
    {
        if (!CommonHandler.IsRandomSaysTrue(enemyProbability)) return;
        
        float radius = cameraRadius + enemyGenerateReserve;
        float randomAngle = Mathf.Clamp(Random.value * 180, generatePointAngle, 180 - generatePointAngle) * Mathf.Deg2Rad;

        Vector2 enemyPosition = Vector2.right * radius;
        // Rotate vector by random angle
        enemyPosition = new Vector2(enemyPosition.x * Mathf.Cos(randomAngle) - enemyPosition.y * Mathf.Sin(randomAngle),
            enemyPosition.x * Mathf.Sin(randomAngle) + enemyPosition.y * Mathf.Cos(randomAngle));

        enemyPosition += (Vector2) camera.transform.position;

        Instantiate(enemy, enemyContainer.transform);
        enemy.transform.position = enemyPosition;
        enemy.SetDeadBottom(GetCameraBottom());
        Debug.Log(GetCameraBottom());
    }
}
