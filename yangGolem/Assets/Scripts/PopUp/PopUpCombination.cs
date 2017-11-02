using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCombination : MonoBehaviour
{
    public UIScrollView uiScrollView; // 판넬에 붙어있음.
    public UIGrid uiGrid;

    public UIButton exitButton;
    Dictionary<int, ItemInfo> dicPlayerInventory;

    private void Awake()
    {
        EventDelegate eventButton = new EventDelegate(this, "ExitButtonEvent");
        EventDelegate.Add(exitButton.onClick, eventButton);
    }


    private void OnEnable()
    {
        dicPlayerInventory = GameInfoManager.instance.playerInventory.dicPlayerInventory;

        ////테스트용.
        //for (int i = 0; i < 5; i++)
        //{
        //    GameObject row = Instantiate(ResourceManager.instance.popup["RowSlotGroup"] as GameObject) as GameObject;
        //    row.name = "RowSlotGroup_" + i;
        //    row.transform.parent = uiGrid.transform;
        //    row.transform.localPosition = Vector3.zero;
        //    row.transform.localScale = Vector3.one;
        //}
        //GameObject popup = Instantiate(Resources.Load("Resource 폴더 안에 있는 프리팹 경로 ex) Popup/Menu") as GameObject) as GameObject;

        //float time = Time.deltaTime; //1초를 프레임레이트로 나눈것 60프레임이라면 1/60 의 값이 떨어짐.
        //Time.time;

        //SetPopUpInit();
        PopUpManager.instance.StartPopUp(this.gameObject);

        Invoke("SetPopUpInit", 0.1f);

    }

    //private void OnDisable()
    //{
    //    //일단 그리드 하위 오브젝트 싹 비우고. 나중에 재활용 하던지 팝업이니 그냥 이렇게 일단.
    //    for (int i = 0; i < uiGrid.transform.childCount; i++)
    //    {
    //        Destroy(uiGrid.transform.GetChild(i).gameObject);
    //    }

    //}

    //public void StartPopUp()
    //{
    //    PopUpManager.instance.StartPopUp(this.gameObject);
    //}

    public void SetPopUpInit()
    {
        int idx = 0;
        foreach (var combination in GameInfoManager.instance.dicItemCombinationInfo)
        {
            GameObject row = null;
            if(uiGrid.transform.Find("RowSlotGroup_" + idx) != null)
            {
                row = uiGrid.transform.Find("RowSlotGroup_" + idx).gameObject;
            }
            else
            {
                row = Instantiate(ResourceManager.instance.popup["RowSlotGroup"] as GameObject);
            }

            row.name = "RowSlotGroup_" + idx++;
            row.transform.parent = uiGrid.transform;
            row.transform.localPosition = Vector3.zero;//new Vector3(0.0f, 0.0f, 0.1f);
            row.transform.localScale = Vector3.one;


            //세부사항 필요한거 켜기.
            int matIdx = 0;
            foreach (int material in combination.Value.listMaterial)
            {

                //아이템 이미지.
                row.transform.Find("ItemSlot_" + matIdx + "/Image").GetComponent<UISprite>().spriteName = material.ToString();

                //아이템들의 갯수를 판단하여 Cover를 씌울지 말지 결정해야한다.
                int itemCnt = dicPlayerInventory[material].itemCnt;
                if (itemCnt > 0)
                {
                    row.transform.Find("ItemSlot_" + matIdx + "/Cover").gameObject.SetActive(false);
                    row.transform.Find("ItemSlot_" + matIdx + "/Label").GetComponent<UILabel>().text = itemCnt.ToString();
                    row.transform.Find("ItemSlot_" + matIdx + "/Label").GetComponent<UILabel>().color = Color.black;
                }
                else
                {
                    row.transform.Find("ItemSlot_" + matIdx + "/Cover").gameObject.SetActive(true);
                    row.transform.Find("ItemSlot_" + matIdx + "/Label").GetComponent<UILabel>().text = itemCnt.ToString();
                    row.transform.Find("ItemSlot_" + matIdx + "/Label").GetComponent<UILabel>().color = Color.red;
                }

                //기호처리. //맨 마지막꺼 처리 '=' 으로 보여야.
                if (combination.Value.listMaterial.Count == matIdx + 1)
                {
                    row.transform.Find("Sign_" + matIdx).GetComponent<UISprite>().spriteName = "Arrow";
                }
                else
                {
                    row.transform.Find("Sign_" + matIdx).GetComponent<UISprite>().spriteName = "Plus";
                }

                row.transform.Find("ItemSlot_" + matIdx).gameObject.SetActive(true);
                row.transform.Find("Sign_" + matIdx).gameObject.SetActive(true);

                matIdx++;

            }

            row.transform.Find("ItemSlot_" + matIdx + "/Image").GetComponent<UISprite>().spriteName = combination.Value.id.ToString();

            int mixedItemCnt = dicPlayerInventory[combination.Value.id].itemCnt;
            if (mixedItemCnt > 0)
            {
                row.transform.Find("ItemSlot_" + matIdx + "/Label").GetComponent<UILabel>().color = Color.black;
            }
            else
            {
                row.transform.Find("ItemSlot_" + matIdx + "/Label").GetComponent<UILabel>().color = Color.red;
            }
            row.transform.Find("ItemSlot_" + matIdx + "/Label").GetComponent<UILabel>().text = mixedItemCnt.ToString();
            row.transform.Find("ItemSlot_" + matIdx).gameObject.SetActive(true);


            //결과적으로 아이템이 모두 준비되었을때 결과 아이템도 Cover를 씌울지 말지 결정. matIdx를 이용 이전꺼가 켜졌는지 아닌지.
            for (int i = 0; i < matIdx; i++)
            {
                if (!row.transform.Find("ItemSlot_" + i + "/Cover").gameObject.activeSelf)//안켜져있으면 합성 가능하다는것. //합성버튼은 활성화.
                {
                    row.transform.Find("ItemSlot_" + matIdx + "/Cover").gameObject.SetActive(false);
                    row.transform.Find("CombinationButton").gameObject.SetActive(true);
                }
                else
                {
                    row.transform.Find("ItemSlot_" + matIdx + "/Cover").gameObject.SetActive(true);
                    row.transform.Find("CombinationButton").gameObject.SetActive(false);
                    break;
                }
            }



            row.SetActive(true);
        }

        // 종류 모두 불러오고 나서, 콤비네이션 종류가 4개 이상인지 판단. //알아서 UIGrid excute됨.
        uiScrollView.enabled = true;
        uiGrid.enabled = true;
    }

    void ExitButtonEvent()
    {
        PopUpManager.instance.EndPopUp(this.gameObject);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ExitButtonEvent();
        }
    }
}
