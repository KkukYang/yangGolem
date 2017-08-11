using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 적이 죽을때 얻는 점수 정보를 나타낼때 사용.
public class DamageText : MonoBehaviour
{

    public int dmage;
    public Queue<GameObject> targetQueue;
    public Sprite[] img;

    void Awake()
    {
        transform.Find("DamageText").GetComponent<AnimationEvent>().add = new AnimationEvent.Add(EndEvent);
    }

    void OnEnable()
    {
        if (dmage.ToString().Length > 0)
        {
            transform.Find("DamageText").Find("1").GetComponent<SpriteRenderer>().sprite = img[int.Parse(dmage.ToString("0000").Substring(3, 1))];
            transform.Find("DamageText").Find("1").gameObject.SetActive(true);
        }
        else
            transform.Find("DamageText").Find("1").gameObject.SetActive(false);

        if (dmage.ToString().Length > 1)
        {
            transform.Find("DamageText").Find("10").GetComponent<SpriteRenderer>().sprite = img[int.Parse(dmage.ToString("0000").Substring(2, 1))];
            transform.Find("DamageText").Find("10").gameObject.SetActive(true);
        }
        else
            transform.Find("DamageText").Find("10").gameObject.SetActive(false);

        if (dmage.ToString().Length > 2)
        {
            transform.Find("DamageText").Find("100").GetComponent<SpriteRenderer>().sprite = img[int.Parse(dmage.ToString("0000").Substring(1, 1))];
            transform.Find("DamageText").Find("100").gameObject.SetActive(true);
        }
        else
            transform.Find("DamageText").Find("100").gameObject.SetActive(false);

        if (dmage.ToString().Length > 3)
        {
            transform.Find("DamageText").Find("1000").GetComponent<SpriteRenderer>().sprite = img[int.Parse(dmage.ToString("0000").Substring(0, 1))];
            transform.Find("DamageText").Find("1000").gameObject.SetActive(true);
        }
        else
            transform.Find("DamageText").Find("1000").gameObject.SetActive(false);

        switch (dmage.ToString().Length)
        {
            case 1:
                transform.Find("DamageText").localPosition = new Vector3(-0.33f, 0.0f, 0.0f);
                break;
            case 2:
                transform.Find("DamageText").localPosition = new Vector3(-0.22f, 0.0f, 0.0f);
                break;
            case 3:
                transform.Find("DamageText").localPosition = new Vector3(-0.11f, 0.0f, 0.0f);
                break;
            case 4:
                transform.Find("DamageText").localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
        }

        transform.Find("DamageText").GetComponent<Animator>().enabled = true;
    }

    public void EndEvent()
    {
        transform.Find("DamageText").GetComponent<Animator>().enabled = false;
        gameObject.SetActive(false);
        targetQueue.Enqueue(gameObject);
    }
}
