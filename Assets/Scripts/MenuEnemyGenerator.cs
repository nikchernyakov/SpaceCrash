using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnemyGenerator : MonoBehaviour {

    public SimpleEnemy enemy;
    [Range(0, 1)]
    public float enemyProbability;
    public GameObject enemyContainer;
    public float enemyGenerateReserve = 2f;
    [Range(0, 90)]
    public float generatePointAngle = 0;
    public float generateCooldown = 0;

    private float currentGenerateCooldown = 0;

    private Camera gameCamera;
    private float cameraRadius;
    private float cameraBottom;

    public void SetCamera(Camera camera)
    {
        this.gameCamera = camera;
    }

    public void SetCameraRadius(float radius)
    {
        cameraRadius = radius;
    }

    public float GetCameraBottom()
    {
        float pixelHeight = gameCamera.pixelHeight;
        Vector2 cameraTop = gameCamera.ScreenToWorldPoint(new Vector2(0, pixelHeight));
        float height = cameraTop.y - gameCamera.transform.position.y;

        return gameCamera.transform.position.y - height;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGenerateCooldown <= 0)
        {
            currentGenerateCooldown = generateCooldown;
            GenerateEnemy();
        }

        currentGenerateCooldown -= Time.deltaTime;
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

        enemyPosition += (Vector2) gameCamera.transform.position;

        SimpleEnemy enemyInstance = Instantiate(enemy, enemyContainer.transform);
        enemyInstance.transform.position = enemyPosition;
        enemyInstance.SetDeadBottom(GetCameraBottom());
        enemy.SetTarget(gameCamera.transform.position);

    }
}
