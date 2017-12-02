using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneProperties
{
    public Sprite background = null;
}

public class Zone : MonoBehaviour {
    private ZoneProperties m_zoneProperties;
    private SpriteRenderer spriteRenderer;

    public void SetZoneProperties(ZoneProperties zoneProperties)
    {
        m_zoneProperties = zoneProperties;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (m_zoneProperties == null)
            return;
        GenerateEnvironment();
    }

    public void GenerateEnvironment()
    {
        GenerateBackground(m_zoneProperties.background);
    }

    public void GenerateBackground(Sprite background)
    {
        spriteRenderer.sprite = background;
    }
}
