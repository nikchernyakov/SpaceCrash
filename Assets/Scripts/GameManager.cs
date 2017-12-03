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

    public GameObject gameElementsCanvas;
    public GameObject pauseElementsCanvas;

    public GameObject gameCamera;
    public float cameraSpeed;
    public float scoreEarnTime;
    public int scoreForKill = 0;
    public int scoreCombo;
    public float scoreComboEffect = 1.5f;
    public float scoreComboTime;

    private float scoreComboTimeCooldown;

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

    private Camera cameraObject;

    private GameStateEnum gameState;

    void Start () {

        ChangeCanvas(true);
        Time.timeScale = 1;
        gameState = GameStateEnum.Play;

        cameraObject = gameCamera.GetComponent<Camera>();

        enemyGenerator.SetCamera(cameraObject);
        enemyGenerator.SetCameraRadius(Vector3.Distance(cameraObject.transform.position,
            cameraObject.ScreenToWorldPoint(new Vector2(cameraObject.pixelWidth, cameraObject.pixelHeight))));

        zoneSize = mapGenerator.zonePrefab.GetComponent<BoxCollider2D>().size;
        zoneCenter = mapGenerator.zonePrefab.transform.position;
    }
	
	void Update () {
        CheckOtherActions();

        if (!gameState.Equals(GameStateEnum.Play))
            return;

        if (!CommonHandler.IsObjectInCamera(cameraObject, gameCamera.transform, player.transform.position))
        {
            GameOver();
        }

        UpdateCombo();

        MoveCamera();
        MovePlayer();

        CheckScore();
    }

    public void UpdateCombo()
    {
        scoreComboTimeCooldown -= Time.deltaTime;

        if(scoreComboTimeCooldown <= 0)
        {
            //Debug.Log("End combo");
            scoreCombo = 0;
        }
    }

    public void AddCombo()
    {
        scoreComboTimeCooldown = scoreComboTime;
        scoreCombo++;
    }

    public void AddScoreForKill()
    {
        AddCombo();
        AddScore((int) (scoreForKill * scoreCombo * scoreComboEffect));
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState.Equals(GameStateEnum.Pause))
            {
                Play();
            }
            else if(gameState.Equals(GameStateEnum.Play))
            {
                Pause();
            }
            else if (gameState.Equals(GameStateEnum.GameOver))
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }

    void AddScore(int value)
    {
        score += value;
        uiManager.ChangeScore(score);
    }

    void CheckScore()
    {
        if(scoreCoolDown <= 0)
        {
            AddScore(1);
            scoreCoolDown = scoreEarnTime;
        }

        scoreCoolDown -= Time.deltaTime;

        if(score % scoresNeedToUpHardLevel == 0)
        {
            UpHardLevel();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void UpHardLevel()
    {
        //Debug.Log("Up level");

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Zone)))
        {
            mapGenerator.GenerateNextZone();
        }
    }

    void Play()
    {
        ChangeCanvas(true);
        Time.timeScale = 1;
        gameState = GameStateEnum.Play;
    }

    void Pause()
    {
        Time.timeScale = 0;
        gameState = GameStateEnum.Pause;

        ChangeCanvas(false);
    }

    public void ChangeCanvas(bool isGame)
    {
        gameElementsCanvas.SetActive(isGame);
        pauseElementsCanvas.SetActive(!isGame);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameState = GameStateEnum.GameOver;
    }
}
