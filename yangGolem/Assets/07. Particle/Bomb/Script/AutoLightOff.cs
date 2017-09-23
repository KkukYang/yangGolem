using UnityEngine;
using System.Collections;

public class AutoLightOff : MonoBehaviour 
{
    public bool destroy = true;

    public float duration = 0.2f;

    public float delayTime = 0.1f;    
    
    public float targetValue = 0.0f;

    float startValue = 1.0f;
    float oldValue = 0.0f;

    Light _light = null;

    void Awake()
    {
        _light = GetComponent<Light>();
    }

    void OnEnable()
    {
        StartCoroutine(LightOffProcess());
    }

    void OnDisable()
    {
        _light.intensity = oldValue;
        StopAllCoroutines();        
    }

    IEnumerator LightOffProcess()
    {
        oldValue = _light.intensity;
        float currentValue = startValue;
        float deltaTime = 0.0f;

        while( (deltaTime / duration) < 1.0f )
        {
            yield return new WaitForSeconds(delayTime);

            deltaTime += Time.deltaTime;
            _light.intensity = Mathf.Lerp(currentValue, targetValue, (deltaTime / duration));
            currentValue = _light.intensity;
        }

        if (destroy)
        {
            Destroy(gameObject);
        }

        yield return null;
    }
}
