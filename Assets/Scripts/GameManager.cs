using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Player player;
    public float playerSpeed;
    public MapGenerator mapGenerator;
    public EnemyGenerator enemyGenerator;
    public GameObject gameCamera;
    public float cameraSpeed;

    private Vector2 zoneSize;
    private Vector2 zoneCenter;

    private Camera camera;

    void Start () {
        camera = gameCamera.GetComponent<Camera>();

        enemyGenerator.SetCamera(camera);
        enemyGenerator.SetCameraRadius(Vector3.Distance(camera.transform.position,
            camera.ScreenToWorldPoint(new Vector2(camera.pixelWidth, camera.pixelHeight))));

        mapGenerator.GenerateNextZone();
        mapGenerator.GenerateNextZone();

        zoneSize = mapGenerator.zonePrefab.GetComponent<BoxCollider2D>().size;
        zoneCenter = mapGenerator.zonePrefab.transform.position;
    }
	
	void Update () {
        if (!IsPlayerInCamera())
        {
            GameOver();
        }

        if (player.IsAlive())
            CheckTap();

        enemyGenerator.GenerateEnemy();

        CheckOtherActions();

        MoveCamera();
        MovePlayer();
    }

    void CheckTap()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        player.SetVelocity(new Vector2(horizontal, vertical));
    }
    
    void CheckOtherActions()
    {
        if (Input.GetKey(KeyCode.R) || (!player.IsAlive() && Input.GetKey(KeyCode.Mouse0)))
        {
            SceneManager.LoadScene("Main");
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Move camera up with cameraSpeed
    void MoveCamera()
    {
        gameCamera.transform.position += Vector3.up * cameraSpeed * Time.deltaTime;
       
    }

    // Move camera up with cameraSpeed
    void MovePlayer()
    {
        player.transform.position += Vector3.up * playerSpeed * Time.deltaTime;

        // Check for exit from horizontal bounds
        Vector2 campPosition = new Vector2(Mathf.Clamp(player.transform.position.x, zoneCenter.x - zoneSize.x / 2, zoneCenter.x + zoneSize.x / 2),
            player.transform.position.y);
        player.transform.position = campPosition;
    }

    bool IsPlayerInCamera()
    {
        float pixelHeight = camera.pixelHeight;
        Vector2 cameraTop = camera.ScreenToWorldPoint(new Vector2(0, pixelHeight));
        float height = cameraTop.y - gameCamera.transform.position.y;

        if (player.transform.position.y < cameraTop.y && player.transform.position.y > gameCamera.transform.position.y - height) 
        {
            return true;
        }
        else
            return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Zone)))
        {
            mapGenerator.GenerateNextZone();
        }
    }

    public void GameOver()
    {
        //Debug.LogError("Game Over");
        Time.timeScale = 0;
    }
}
