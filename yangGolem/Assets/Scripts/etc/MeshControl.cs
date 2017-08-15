using UnityEngine;
using System.Collections;

public class MeshControl : MonoBehaviour
{
    public string sortingLayerName;
    public int sortingOrder;
    Mesh mesh;
    MeshFilter meshFilter;

    public bool isFadeIn;
    private float alpha = 0.0f;

    void OnEnable()
    {
        alpha = 0.0f;

        gameObject.GetComponent<MeshRenderer>().sortingLayerName = sortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }
        if (meshFilter != null)
        {
            mesh = meshFilter.sharedMesh;
        }

        if(isFadeIn)
        {
            GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);

            StartCoroutine("FadeInMesh");
        }
    }

    IEnumerator FadeInMesh()
    {
        while(true)
        {
            if(alpha >= 1.0f)
            {
                break;
            }

            GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            alpha += 0.001f;
            yield return null;
        }
    }

    void Update()
    {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = sortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
    }
}
