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

        public Vector2 obstacleSize;
        public Obstacle obstacle;

        public Sprite background = null;
    }



    private List<Zone> zoneList;

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
        zoneInstance.SetZoneProperties(GenerateProperties());
    }

    private ZoneProperties GenerateProperties()
    {
        ZoneProperties zoneProperties = this.zoneProperties;
        zoneProperties.background = backgrounds[Random.Range(0, backgrounds.Count - 1)];
        
        return zoneProperties;
    }

}
