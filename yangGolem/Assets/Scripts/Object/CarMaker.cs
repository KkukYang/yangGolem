using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMaker : MonoBehaviour {

    public GameObject[] carBody;
    GameObject[] _carChair;
    GameObject[] _carBody;
    GameObject[] _carWheel;
    int _chairNum, _bodyNum, _wheelNum;
	void Awake () {
        _chairNum = 0;
        _bodyNum = 0;
        _wheelNum = 0;
        _carChair = new GameObject[500];
        _carBody = new GameObject[500];
        _carWheel = new GameObject[500];
        for (int i = 0; i < _carChair.Length; i++)
        {
            _carChair[i] = Instantiate(carBody[1]) as GameObject;
            _carChair[i].SetActive(false);
            _carBody[i] = Instantiate(carBody[2]) as GameObject;
            _carBody[i].SetActive(false);
            _carWheel[i] = Instantiate(carBody[3]) as GameObject;
            _carWheel[i].SetActive(false);
        }
	}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TotalData.carBodyNum = 0;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            TotalData.carBodyNum = 1;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            TotalData.carBodyNum = 2;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            TotalData.carBodyNum = 3;
        }
	}
    public void CarBodyFunc(Transform _setParent, Vector3 pos,CarCtrl selectedCar)
    {
        if (TotalData.carBodyNum < 0) return;
        if (TotalData.carBodyNum == 1)
        {
            _carChair[_chairNum].SetActive(true);
            _carChair[_chairNum].transform.position = pos;
            _carChair[_chairNum].transform.SetParent(_setParent);
            _carChair[_chairNum].transform.localRotation = Quaternion.Euler(0, 0, 0);
            selectedCar.playerPos = _carChair[_chairNum].transform;
            ++_chairNum;
        }
        else if (TotalData.carBodyNum == 2)
        {
            _carBody[_bodyNum].SetActive(true);
            _carBody[_bodyNum].transform.position = pos;
            _carBody[_bodyNum].transform.SetParent(_setParent);
            _carBody[_bodyNum].transform.localRotation = Quaternion.Euler(0, 0, 0);
            ++_bodyNum;
        }
        else if (TotalData.carBodyNum == 3)
        {
            _carWheel[_wheelNum].SetActive(true);
            _carWheel[_wheelNum].transform.position = pos;
            _carWheel[_wheelNum].transform.SetParent(_setParent);
            _carWheel[_wheelNum].transform.localRotation = Quaternion.Euler(0, 0, 0);
            ++_wheelNum;
        }
    }
    public void OnCarBodySelected(Button name)
    {
        switch (name.name)
        {
            case "btnCarChair":
                TotalData.carBodyNum = 1;
                break;
            case "btnCarBody":
                TotalData.carBodyNum = 2;
                break;
            case "btnCarWheel":
                TotalData.carBodyNum = 3;
                break;
        }

    }
}
