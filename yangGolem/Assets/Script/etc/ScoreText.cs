 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 적이 죽을때 얻는 점수 정보를 나타낼때 사용.
public class ScoreText : MonoBehaviour {

	public int score;
	public Queue<GameObject> targetQueue;
	public Sprite[] img;

	void Awake()
	{
		transform.Find("ScoreText").GetComponent<AnimationEvent>().add = new AnimationEvent.Add(EndEvent);
	}

	void OnEnable()
	{
		if(score.ToString().Length > 0)
		{
			transform.Find("ScoreText").Find("1").GetComponent<SpriteRenderer>().sprite = img[int.Parse(score.ToString("0000").Substring(3,1))];
			transform.Find("ScoreText").Find("1").gameObject.SetActive(true);
		}
		else
			transform.Find("ScoreText").Find("1").gameObject.SetActive(false);

		if(score.ToString().Length > 1)
		{
			transform.Find("ScoreText").Find("10").GetComponent<SpriteRenderer>().sprite = img[int.Parse(score.ToString("0000").Substring(2,1))];
			transform.Find("ScoreText").Find("10").gameObject.SetActive(true);
		}
		else
			transform.Find("ScoreText").Find("10").gameObject.SetActive(false);

		if(score.ToString().Length > 2)
		{
			transform.Find("ScoreText").Find("100").GetComponent<SpriteRenderer>().sprite = img[int.Parse(score.ToString("0000").Substring(1,1))];
			transform.Find("ScoreText").Find("100").gameObject.SetActive(true);
		}
		else
			transform.Find("ScoreText").Find("100").gameObject.SetActive(false);

		if(score.ToString().Length > 3)
		{
			transform.Find("ScoreText").Find("1000").GetComponent<SpriteRenderer>().sprite = img[int.Parse(score.ToString("0000").Substring(0,1))];
			transform.Find("ScoreText").Find("1000").gameObject.SetActive(true);
		}
		else
			transform.Find("ScoreText").Find("1000").gameObject.SetActive(false);

		switch(score.ToString().Length)
		{
		case 1:
			transform.Find("ScoreText").localPosition = new Vector3(-0.33f, 0.0f, 0.0f);
			break;
		case 2:
			transform.Find("ScoreText").localPosition = new Vector3(-0.22f, 0.0f, 0.0f);
			break;
		case 3:
			transform.Find("ScoreText").localPosition = new Vector3(-0.11f, 0.0f, 0.0f);
			break;
		case 4:
			transform.Find("ScoreText").localPosition = new Vector3(0.0f, 0.0f, 0.0f);
			break;
		}

		transform.Find("ScoreText").GetComponent<Animator>().enabled = true;
	}

	public void EndEvent()
	{
		transform.Find("ScoreText").GetComponent<Animator>().enabled = false;
		gameObject.SetActive(false);
		targetQueue.Enqueue(gameObject);
	}
}
