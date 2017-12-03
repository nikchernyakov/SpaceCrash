using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public MenuMapGenerator mapGenerator;
    public MenuEnemyGenerator enemyGenerator;
    public UIManager uiManager;

    public GameObject mainMenuCanvas;
    public GameObject aboutMenuCanvas;

    public GameObject gameCamera;
    public float cameraSpeed;
    private Camera cameraObject;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;

        cameraObject = gameCamera.GetComponent<Camera>();

        enemyGenerator.SetCamera(cameraObject);
        enemyGenerator.SetCameraRadius(Vector3.Distance(cameraObject.transform.position,
            cameraObject.ScreenToWorldPoint(new Vector2(cameraObject.pixelWidth, cameraObject.pixelHeight))));
    }
	
	// Update is called once per frame
	void Update () {
        CheckOtherActions();
        MoveCamera();
    }

    // Move camera up with cameraSpeed
    void MoveCamera()
    {
        gameCamera.transform.position += Vector3.up * cameraSpeed * Time.deltaTime;

    }

    void CheckOtherActions()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartGame();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Exit();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void AboutGame()
    {
        mainMenuCanvas.SetActive(false);
        aboutMenuCanvas.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        mainMenuCanvas.SetActive(true);
        aboutMenuCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.GetTagNameByEnum(TagEnum.Zone)))
        {
            mapGenerator.GenerateNextZone();
        }
    }

}
