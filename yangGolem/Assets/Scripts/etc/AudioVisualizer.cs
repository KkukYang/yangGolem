using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class AudioVisualizer : MonoBehaviour
{
    public AudioSource aSource = null;
    public float[] samples = new float[64];

    public GameObject cube;
    public GameObject cube2;

    private Vector3 cubePos;
    private Transform[] cubesTransform;
    private Transform[] cubesTransform2;

    private Vector3 gravity = new Vector3(0.0f, 0.25f, 0.0f);

    public bool isStart = false;

    void Awake()
    {
        this.aSource = GetComponent<AudioSource>();

        cubesTransform = new Transform[samples.Length];
        cubesTransform2 = new Transform[samples.Length];

        GameObject tempCube;

        for (int i = 0; i < samples.Length; i++)
        {
            tempCube = Instantiate(cube);
            tempCube.transform.parent = this.transform;
            tempCube.transform.localPosition = new Vector3(i * -(10.0f / 64.0f), 0.0f, 0.0f);
            tempCube.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            cubesTransform[i] = tempCube.GetComponent<Transform>();
        }

        for (int i = 0; i < samples.Length; i++)
        {
            tempCube = Instantiate(cube2);
            tempCube.transform.parent = this.transform;
            tempCube.transform.localPosition = new Vector3(0.0f, 0.0f, i * -(10.0f / 64.0f));
            tempCube.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            cubesTransform2[i] = tempCube.GetComponent<Transform>();
        }

    }

    IEnumerator Start()
    {
        while(aSource == null)
        {
            yield return null;
        }

        isStart = true;
    }

    void Update()
    {
        if(!isStart)
        {
            return;
        }

        aSource.GetSpectrumData(this.samples, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < samples.Length; i++)
        {
            cubePos.Set(cubesTransform[i].position.x, Mathf.Clamp(samples[i] * (50 + i * i), 0, 50) * 0.2f, cubesTransform[i].position.z);

            if (cubePos.y >= cubesTransform[i].position.y)
            {
                cubesTransform[i].position = cubePos;
            }
            else
            {
                cubesTransform[i].position -= gravity;
            }

        }

        for (int i = samples.Length-1; i >= 0; i--)
        {
            cubePos.Set(cubesTransform2[i].position.x, Mathf.Clamp(samples[i] * (50 + i * i), 0, 50) * 0.2f, cubesTransform2[i].position.z);

            if (cubePos.y >= cubesTransform2[i].position.y)
            {
                cubesTransform2[i].position = cubePos;
            }
            else
            {
                cubesTransform2[i].position -= gravity;
            }

        }

    }
}
