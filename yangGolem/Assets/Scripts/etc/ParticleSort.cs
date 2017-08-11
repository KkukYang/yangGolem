 using UnityEngine;
using System.Collections;

// 파티클의 레이어를 정의하고 제어.
public class ParticleSort : MonoBehaviour {

	public string sortingLayerName;
	public int sortingOrder;

	float durtion;
	float lifeTime;
	public bool isDestyoy = false;
	public bool isUnscaledTime = false;

	ParticleSystem ps;

    void OnDisable()
    {
        StopCoroutine("StartParticle");
    }

    void OnEnable()
    {
        ps = this.GetComponent<ParticleSystem>();
        StartCoroutine("StartParticle");
    }

    void Update()
	{
		if(isUnscaledTime)
			ps.Simulate(Time.unscaledDeltaTime, true, false);
	}

	IEnumerator StartParticle()
	{
		durtion = ps.duration;
		lifeTime = ps.startLifetime;
		ps.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortingLayerName;
		ps.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = sortingOrder;

		for(int i=0;i<ps.transform.childCount;i++)
		{
			if(ps.transform.GetChild(i).GetComponent<ParticleSystem>())
			{
				ps.transform.GetChild(i).GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortingLayerName;
				ps.transform.GetChild(i).GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = sortingOrder;
			}
		}

        if(CoroutineManager.instance != null)
            yield return CoroutineManager.instance.GetWaitForSeconds(durtion + lifeTime);
        else
            yield return new WaitForSeconds(durtion + lifeTime);


        if (isDestyoy)
			Destroy(gameObject);
	}
}
