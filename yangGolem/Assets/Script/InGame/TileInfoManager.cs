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
        for(int x = 0; x < CheckLocationByClick.instance.row; x++)
        {
            for(int y = 0; y < CheckLocationByClick.instance.col; y++)
            {
                switch(arrFloorTile[x,y])
                {
                    case (int)EnumFloorTile.Normal:
                        {
                            GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumFloorTile.Normal.ToString()]) as GameObject;
                            //좌표에 따른 위치 지정.
                            _tileObj.transform.parent = floorTileGroup.transform;
                            _tileObj.transform.localPosition = new Vector3(x * 0.5f - (0.5f * 0.5f) * (CheckLocationByClick.instance.row - 1)
                                , 0.0f
                                , y * -0.5f + (0.5f * 0.5f) * (CheckLocationByClick.instance.col - 1));
                            _tileObj.transform.eulerAngles = new Vector3(90.0f, 45.0f, 0.0f);
                            _tileObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                            _tileObj.SetActive(true);
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

        for (int i = 1; i < lines.Length; i++)
        {
            string[] temp = lines[i].Split(spliter, option);

            for (int j = 0; j < temp.Length; j++)
            {
                arrLayer[i, j] = int.Parse(temp[j]);
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