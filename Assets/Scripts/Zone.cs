using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {
    private MapGenerator.ZoneProperties m_zoneProperties;
    private SpriteRenderer spriteRenderer;

    private int[,] grid;
    private Vector2 zoneSize;

    public void SetZoneProperties(MapGenerator.ZoneProperties zoneProperties)
    {
        m_zoneProperties = zoneProperties;
    }

    void Start()
    {
        zoneSize = GetComponent<BoxCollider2D>().size;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (m_zoneProperties == null)
            return;

        GenerateEnvironment();
    }

    public void GenerateEnvironment()
    {
        GenerateGrid();
        GenerateObstacles();
        GenerateBackground(m_zoneProperties.background);
    }

    public void GenerateBackground(Sprite background)
    {
        spriteRenderer.sprite = background;
    }

    public void GenerateGrid()
    {
        grid = new int[m_zoneProperties.rowCount, m_zoneProperties.columnCount];
    }

    public void GenerateObstacles()
    {
        bool previousRowHasObstacle = false;
        Vector2 zoneRightDownPoint = (Vector2) transform.position - zoneSize / 2;
        for(int rowInd = 0; rowInd < m_zoneProperties.rowCount;
            rowInd += previousRowHasObstacle ? m_zoneProperties.filledRowDistance : 1)
        {
            previousRowHasObstacle = !previousRowHasObstacle;
            //Debug.Log(m_zoneProperties.obstacleRowProbability);

            if (!CommonHandler.IsRandomSaysTrue(m_zoneProperties.obstacleRowProbability)) continue;
            else previousRowHasObstacle = true;
            //Debug.Log("Row loop");
            bool obstacleHasExit = false;

            for(int columnInd = 0; columnInd < m_zoneProperties.columnCount; columnInd++)
            {
                if ((columnInd < m_zoneProperties.columnCount - 1 || obstacleHasExit) 
                    && CommonHandler.IsRandomSaysTrue(m_zoneProperties.obstacleProbability))
                {
                   // Debug.Log("Column loop");
                    Obstacle obstacle = Instantiate(m_zoneProperties.obstacle, gameObject.transform);
                    obstacle.transform.position =
                        new Vector2(zoneRightDownPoint.x + columnInd * m_zoneProperties.obstacleSize.x + m_zoneProperties.obstacleSize.x / 2,
                       zoneRightDownPoint.y + rowInd * m_zoneProperties.obstacleSize.y + m_zoneProperties.obstacleSize.y / 2);
                } else
                {
                    obstacleHasExit = true;
                }
            }
        }
    }
}
