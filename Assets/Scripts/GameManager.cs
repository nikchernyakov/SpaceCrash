using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Player player;
    public float tapCooldown;
    private float currentTapCooldown;

    public MapGenerator mapGenerator;

    public GameObject gameCamera;

    void Start () {
        currentTapCooldown = 0;

        mapGenerator.GenerateNextZone();
        mapGenerator.GenerateNextZone();
        mapGenerator.GenerateNextZone();
    }
	
	void Update () {
        if (player.IsAlive() && IsPlayerInCamera())
            CheckTap();

        CheckOtherActions();

        // Decrease current tap cooldown
        if (currentTapCooldown > 0)
        {
            currentTapCooldown -= Time.deltaTime;
        }

        MoveCamera();
    }

    void CheckTap()
    {
        if (player.HasEnergy() && currentTapCooldown <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                player.Jump();
                currentTapCooldown = tapCooldown;
            }
        }
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

    // Move camera by player for horizontal
    void MoveCamera()
    {
        gameCamera.transform.position = new Vector3(player.transform.position.x,
            gameCamera.transform.position.y, gameCamera.transform.position.z);
    }

    bool IsPlayerInCamera()
    {
        Camera camera = gameCamera.GetComponent<Camera>();
        float height = camera.pixelHeight;
        Vector2 cameraTop = camera.ScreenToWorldPoint(new Vector2(0, height));

        if (player.transform.position.y < cameraTop.y)
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
            Debug.Log("EnterZone");
            mapGenerator.GenerateNextZone();
        }
    }
}
