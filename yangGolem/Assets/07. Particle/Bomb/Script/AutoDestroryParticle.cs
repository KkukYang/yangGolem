using UnityEngine;
using System.Collections;

public class AutoDestroryParticle : MonoBehaviour 
{
    public bool destroy = true;

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine("ParticleProcess");
    }

    IEnumerator ParticleProcess()
    {
        yield return null;

        ParticleSystem ps = GetComponent<ParticleSystem>();

        while ( true )
        {
            if(ps.IsAlive(true) == false )
            {
                if (destroy)
                    Destroy(gameObject);
                else
                    gameObject.SetActive(false);
                break;
            }            

            yield return new WaitForSeconds(0.3f);
        }
    }
}
