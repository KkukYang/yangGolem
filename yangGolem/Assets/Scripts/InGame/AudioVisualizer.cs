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

    public Vector3 gravity = new Vector3(0.0f, 0.25f, 0.0f);

    public float testFloat = 1.0f;
    public FFTWindow testFFT;

    void Awake()
    {
        this.aSource = GetComponent<AudioSource>();

        cubesTransform = new Transform[samples.Length];
        cubesTransform2 = new Transform[samples.Length];

        GameObject tempCube;

        for (int i = 0; i < samples.Length-1; i++)
        {
            tempCube = Instantiate(cube);
            tempCube.transform.parent = this.transform;
            tempCube.transform.localPosition = new Vector3(i * -(20.0f / 64.0f), 0.0f, 0.0f);
            tempCube.transform.eulerAngles = new Vector3(0.0f, -45.0f, 0.0f);

            cubesTransform[i] = tempCube.GetComponent<Transform>();
        }

        for (int i = 0; i < samples.Length - 1; i++)
        {
            tempCube = Instantiate(cube2);
            tempCube.transform.parent = this.transform;
            tempCube.transform.localPosition = new Vector3(0.0f, 0.0f, i * -(20.0f / 64.0f));
            tempCube.transform.eulerAngles = new Vector3(0.0f, -45.0f, 0.0f);

            cubesTransform2[i] = tempCube.GetComponent<Transform>();
        }

    }

    IEnumerator Start()
    {
        while(aSource == null)
        {
            yield return null;
        }

        StartCoroutine("StartCubeAction");
    }

    IEnumerator StartCubeAction()
    {
        while(true)
        {
            aSource.GetSpectrumData(this.samples, 0, testFFT);

            for (int i = 0; i < samples.Length - 1; i++)
            {
                cubePos.Set(cubesTransform[i].localPosition.x, Mathf.Clamp(samples[i] * (50 + i * i), 0, 50) * testFloat, cubesTransform[i].localPosition.z);

                if (cubePos.y >= cubesTransform[i].localPosition.y)
                {
                    cubesTransform[i].localPosition = cubePos;
                }
                else
                {
                    cubesTransform[i].localPosition -= gravity;
                }

            }

            for (int i = samples.Length - 2; i >= 0; i--)
            {
                cubePos.Set(cubesTransform2[i].localPosition.x, Mathf.Clamp(samples[i] * (50 + i * i), 0, 50) * testFloat, cubesTransform2[i].localPosition.z);

                if (cubePos.y >= cubesTransform2[i].localPosition.y)
                {
                    cubesTransform2[i].localPosition = cubePos;
                }
                else
                {
                    cubesTransform2[i].localPosition -= gravity;
                }

            }

            yield return CoroutineManager.instance.GetWaitForSeconds(0.03f);
        }
    }



    void Update()
    {


    }
}
