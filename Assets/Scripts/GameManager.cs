using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Player player;
    public float playerSpeed;

    public MapGenerator mapGenerator;
    public EnemyGenerator enemyGenerator;
    public UIManager uiManager;

    public GameObject gameCamera;
    public float cameraSpeed;
    public float scoreEarnTime;

    public int scoresNeedToUpHardLevel = 10;
    public UpLevelProperties upLevelProperties;
    
    [System.Serializable]
    public class UpLevelProperties
    {
        public float playerShiftYDelta;

        // GameManager
        public float cameraSpeedDelta;
        public float scoreEarnTimeDelta;
        public float scoresNeedToUpHardLevelDelta;
        // EnemyGenerator
        public float enemyPropabilityDelta;
        public float enemyGenerateCooldownDelta;
        // Enemy
        public float enemySpeedDelta;
        public float enemyMassDelta;
        // MapGenerator
        public float obstacleRowProbabilityDelta;
        public float obstacleProbabilityDelta;

    }

    private int score;
    private float scoreCoolDown = 0;
    private Vector2 zoneSize;
    private Vector2 zoneCenter;

    private Camera camera;

    private bool isGameOver = false;

    void Start () {
        Time.timeScale = 1;
        isGameOver = false;

        camera = gameCamera.GetComponent<Camera>();

        enemyGenerator.SetCamera(camera);
        enemyGenerator.SetCameraRadius(Vector3.Distance(camera.transform.position,
            camera.ScreenToWorldPoint(new Vector2(camera.pixelWidth, camera.pixelHeight))));

        zoneSize = mapGenerator.zonePrefab.GetComponent<BoxCollider2D>().size;
        zoneCenter = mapGenerator.zonePrefab.transform.position;
    }
	
	void Update () {
        if (isGameOver)
            return;

        if (!IsPlayerInCamera())
        {
            GameOver();
        }

        CheckOtherActions();

        MoveCamera();
        MovePlayer();

        CheckScore();
    }

    bool CheckTap()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 velocity = new Vector2(horizontal, vertical);

        player.SetVelocity(velocity);

        return velocity.Equals(Vector2.zero);
    }
    
    void CheckOtherActions()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("Main");
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void CheckScore()
    {
        if(scoreCoolDown <= 0)
        {
            score++;
            uiManager.ChangeScore(score);
            scoreCoolDown = scoreEarnTime;
        }

        scoreCoolDown -= Time.deltaTime;

        if(score % scoresNeedToUpHardLevel == 0)
        {
            UpHardLevel();
        }
    }

    public void UpHardLevel()
    {
        Debug.Log("Up level");

        player.shiftPower.y += upLevelProperties.playerShiftYDelta;

        cameraSpeed += upLevelProperties.cameraSpeedDelta;
        scoreEarnTime -= upLevelProperties.scoreEarnTimeDelta;
        scoresNeedToUpHardLevel = (int) (scoresNeedToUpHardLevel * upLevelProperties.scoresNeedToUpHardLevelDelta);

        enemyGenerator.enemyProbability += upLevelProperties.enemyPropabilityDelta;
        enemyGenerator.generateCooldown -= upLevelProperties.enemyGenerateCooldownDelta;

        enemyGenerator.enemySpeedProperties += upLevelProperties.enemySpeedDelta;
        enemyGenerator.enemyMassProperties += upLevelProperties.enemyMassDelta;

        mapGenerator.zoneProperties.obstacleProbability += upLevelProperties.obstacleProbabilityDelta;
        mapGenerator.zoneProperties.obstacleRowProbability += upLevelProperties.obstacleRowProbabilityDelta;
    }

    // Move camera up with cameraSpeed
    void MoveCamera()
    {
        gameCamera.transform.position += Vector3.up * cameraSpeed * Time.deltaTime;
       
    }

    // Move camera up with cameraSpeed
    void MovePlayer()
    {
        CheckTap();

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
        isGameOver = true;
    }
}
