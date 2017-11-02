using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class ViewAroundCubeCheck : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = this.transform.parent.GetComponent<Player>();
    }


    void Start()
    {

    }


    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cube"))
        {
            TileInfoManager.instance.listGeoCube.Add(other.transform.parent.GetComponent<GeographyCube>());
            other.transform.parent.GetComponent<GeographyCube>().enabled = true;

            //other.transform.DOLocalMove(other.transform.localPosition + Vector3.up * 0.1f, 0.3f).OnComplete(
            //    () =>
            //    {
            //        other.transform.DOLocalMove(other.transform.localPosition + Vector3.up * -0.1f, 0.3f);
            //    });
            other.GetComponent<NavMeshSourceTag>().enabled = true;
            //other.GetComponent<LODGroup>().enabled = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("FieldObject"))
        {
            other.GetComponent<NavMeshObstacle>().enabled = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cube"))
        {
            TileInfoManager.instance.listGeoCube.Remove(other.transform.parent.GetComponent<GeographyCube>());
            other.transform.parent.GetComponent<GeographyCube>().enabled = false;
            other.GetComponent<NavMeshSourceTag>().enabled = false;
            //other.GetComponent<LODGroup>().enabled = false;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("FieldObject"))
        {
            other.GetComponent<NavMeshObstacle>().enabled = false;
        }


    }


}
