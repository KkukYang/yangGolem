using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class TileInfoManager : MonoBehaviour
{
    private static TileInfoManager s_instance = null;
    public static TileInfoManager instance
    {
        get
        {
            if(s_instance == null)
            {
                s_instance = FindObjectOfType(typeof(TileInfoManager)) as TileInfoManager;
                if(s_instance == null)
                {
                    Debug.Log("TileInfoManager null");
                }
            }
            return s_instance;
        }
    }
    
        
    //dic layer [row, col] [x, y].
    public Dictionary<int, int[,]> dicTileInfo = new Dictionary<int, int[,]>();
    public int[,] arrFloorTile;

    public GameObject floorTileGroup;

    //Test
    public float testX, testY;

    private void Awake()
    {
        InitializeStage();
    }

    private void Start()
    {
        //바닥타일 배치.
        SetFloorTile();


        //지형 & 오브젝트 배치.
        SetGeography();

    }

    void SetFloorTile()
    {
        for(int y = 0; y < CheckLocationByClick.instance.col; y++)
        {
            for(int x = 0; x < CheckLocationByClick.instance.row; x++)
            {
                switch(arrFloorTile[y,x])
                {
                    case (int)EnumFloorTile.Normal:
                        {
                            GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumFloorTile.Normal.ToString()]) as GameObject;
                            //좌표에 따른 위치 지정.
                            SetTileOnLand(_tileObj, floorTileGroup, x, y);
                        }
                        break;

                    case (int)EnumFloorTile.Grass:
                        {
                            GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumFloorTile.Grass.ToString()]) as GameObject;
                            //좌표에 따른 위치 지정.
                            SetTileOnLand(_tileObj, floorTileGroup, x, y);
                        }
                        break;

                    case (int)EnumFloorTile.Soil:
                        {
                            GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumFloorTile.Soil.ToString()]) as GameObject;
                            //좌표에 따른 위치 지정.
                            SetTileOnLand(_tileObj, floorTileGroup, x, y);
                        }
                        break;


                    default:
                        {
                            Debug.Log(x + ", " + y + " exception!! " + arrFloorTile[x,y]);
                        }
                        break;
                }
            }
        }
    }

    void SetTileOnLand(GameObject _obj, GameObject _group, int _x, int _y)
    {
        _obj.transform.parent = _group.transform;
        _obj.transform.localPosition = new Vector3(_x * 0.5f - (0.5f * 0.5f) * (CheckLocationByClick.instance.row - 1)
            , 0.0f
            , _y * -0.5f + (0.5f * 0.5f) * (CheckLocationByClick.instance.col - 1));
        _obj.transform.eulerAngles = new Vector3(90.0f, 45.0f, 0.0f);
        _obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _obj.SetActive(true);
    }

    void SetGeography()
    {

    }

    private void InitializeStage()
    {
        //바닥 타일 데이터 셋팅.
        arrFloorTile = new int[CheckLocationByClick.instance.row, CheckLocationByClick.instance.col];
        InitTileInfoFromCsv(arrFloorTile, 0);

        //지형 & 배치된 오브젝트 데이터 셋팅.
        dicTileInfo.Clear();
        dicTileInfo.Add(0, new int[CheckLocationByClick.instance.row, CheckLocationByClick.instance.col]);
    }



    private int[,] InitTileInfoFromCsv(int[,] arrLayer, int stage = 0)
    {
        TextAsset data;
        data = Resources.Load("CSVs/FloorTileInfo_" + stage) as TextAsset;

        if (data == null)
        {
            Debug.LogError("> [ConfigManager: Csv Load Fail]");
        }

        System.StringSplitOptions option = System.StringSplitOptions.RemoveEmptyEntries;
        string[] lines = data.text.Split(new char[] { '\r', '\n' }, option);
        char[] spliter = new char[1] { ',' };

        for (int y = 0; y < lines.Length; y++)
        {
            string[] temp = lines[y].Split(spliter, option);

            for (int x = 0; x < temp.Length; x++)
            {
                arrLayer[y, x] = int.Parse(temp[x]);
            }
        }

        return arrLayer;
    }



    void Update()
    {

    }
}


public enum EnumFieldObject
{
    NULL = 0,
    Tree,
    Geography,
}

public enum EnumFloorTile
{
    Normal,
    Grass,
    Soil,
}