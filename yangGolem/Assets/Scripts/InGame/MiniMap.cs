using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform map;
    public Transform frame;
    public UISlider slider;

    Vector2 mapSize;
    int yPos = 0;
    int xPos = 0;

    private void Awake()
    {
        mapSize = map.GetComponent<BoxCollider2D>().size;
    }

    private void OnEnable()
    {
        mapSize = map.GetComponent<BoxCollider2D>().size;
        xPos = Player.instance.curBottomPositionID % TileInfoManager.instance.col;
        yPos = Player.instance.curBottomPositionID / TileInfoManager.instance.col;

        StartCoroutine(UpdateMiniMap());
    }


    private void OnDisable()
    {
        StopCoroutine(UpdateMiniMap());

    }

    IEnumerator UpdateMiniMap()
    {
        while(true)
        {
            xPos = Player.instance.curBottomPositionID % TileInfoManager.instance.col;
            yPos = Player.instance.curBottomPositionID / TileInfoManager.instance.col;
            mapSize.ToString();
            frame.localPosition = new Vector3((xPos / 300.0f) * mapSize.x - mapSize.x * 0.5f
                , (yPos / 300.0f) * mapSize.x - mapSize.y * 0.5f
                , 0.0f);

            yield return new WaitForSeconds(0.2f);
        }
    }


    void Update()
    {
        map.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, slider.value);


    }
}
