using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public List<Sprite> backgrounds;
    public float zonesDistance;
    public GameObject zonesContainer;
    public Zone zonePrefab;
    public Zone startZone;
    public int zoneOnSceneCount;
    public ZoneProperties zoneProperties;

    [System.Serializable]
    public class ZoneProperties : System.Object
    {
        public int rowCount;
        //public int rowSize;
        public int columnCount;
        //public int columnSize;

        public int filledRowDistance;
        [Range(0, 1)]
        public float obstacleRowProbability;
        [Range(0, 1)]
        public float obstacleProbability;
        public float columnDistance;

        public Vector2 obstacleSize;
        public Obstacle obstacle;

        public Sprite background = null;
    }

    private List<Zone> zoneList;
    private Vector2 zoneSize;

    private bool previousRowHasObstacle = false;
    private int filledRowRest = 0;

    private void Start()
    {
        zoneSize = zonePrefab.GetComponent<BoxCollider2D>().size;

        GenerateNextZone();
        GenerateNextZone();
    }

    public void GenerateNextZone()
    {
        if (zoneList == null)
        {
            if (startZone != null)
            {
                zoneList = new List<Zone>();
                zoneList.Add(startZone);
                GenerateNextZone();
            }
            else
            {
                Debug.LogError("Generator don't has a start point");
            }
        }
        else
        {
            if (zoneList.Count > zoneOnSceneCount)
            {
                Destroy(zoneList[0].gameObject);
                zoneList.RemoveAt(0);
            }

            if (zoneList.Count > 0)
            {
                InstantiateNextZone(zonePrefab, zoneList[zoneList.Count - 1].transform.position);
            } 
            else
            {
                Debug.LogError("Generator has empty zone list");
            }
        }
    }

    private void InstantiateNextZone(Zone nextZone, Vector2 previousZonePosition)
    {
        Zone zoneInstance = Instantiate(nextZone, previousZonePosition + new Vector2(0, zonesDistance), Quaternion.identity);
        zoneInstance.transform.parent = zonesContainer.transform;
        zoneList.Add(zoneInstance);
        ZoneProperties zoneProperties = GenerateProperties();
        zoneInstance.SetBackground(zoneProperties.background);
        GenerateObstacles(zoneInstance.transform, zoneProperties);
    }

    public void GenerateObstacles(Transform zoneTransform, ZoneProperties zoneProperties)
    {
        
        Vector2 zoneRightDownPoint = (Vector2) zoneTransform.position - zoneSize / 2;
        int rowInd = previousRowHasObstacle ? filledRowRest : 0;
        previousRowHasObstacle = false;
        for (; rowInd < zoneProperties.rowCount;
            rowInd += previousRowHasObstacle ? zoneProperties.filledRowDistance : 1)
        {
            // Change when skip
            previousRowHasObstacle = !previousRowHasObstacle;

            if (!CommonHandler.IsRandomSaysTrue(zoneProperties.obstacleRowProbability)) continue;
            else previousRowHasObstacle = true;
            
            // Get random exit column index
            int randomEmptyColumnInd = Random.Range(0, zoneProperties.columnCount);

            for (int columnInd = 0; columnInd < zoneProperties.columnCount; columnInd++)
            {
                if (columnInd != randomEmptyColumnInd
                    && CommonHandler.IsRandomSaysTrue(zoneProperties.obstacleProbability))
                {
                    Obstacle obstacle = Instantiate(zoneProperties.obstacle, zoneTransform);
                    obstacle.transform.position =
                        new Vector2(zoneRightDownPoint.x  // Start of zone
                        + columnInd * (zoneProperties.obstacleSize.x // Go to the obstacle position in row
                        + zoneProperties.columnDistance * (columnInd != 0 ? 1 : 0)) // Add column distance if not start of zone
                        + zoneProperties.obstacleSize.x / 2, // Add obstacle center
                       zoneRightDownPoint.y + rowInd * zoneProperties.obstacleSize.y + zoneProperties.obstacleSize.y / 2);
                }
            }
        }
        if (previousRowHasObstacle && rowInd >= zoneProperties.rowCount)
        {
            filledRowRest = rowInd - zoneProperties.rowCount;
        }
        else
        {
            filledRowRest = 0;
        }
    }

    private ZoneProperties GenerateProperties()
    {
        ZoneProperties zoneProperties = this.zoneProperties;
        zoneProperties.background = backgrounds[Random.Range(0, backgrounds.Count - 1)];
        
        return zoneProperties;
    }

}
