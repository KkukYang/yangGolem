using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlManager : MonoBehaviour
{

    //이동 하고자 하는 경우,  X Z좌표로 움직이면 됨.
    Camera thisCamera = null;
    public GameObject player;
    public Vector3 increment;
    public Vector3 initCameraPos;
    public Vector3 initPlayerPos;

    //[Range(5, 20)]
    public float fov;
    public float min;
    public float max;
    public float zoomSnap;

    public bool isInit = false;

	bool shake = false;

    [Range(0.0f, 1.0f)]
    public float translucentAlpha;
    public Material translucentMaterial;
    private List<TranslucentObj> listTranslucentObj = new List<TranslucentObj>();

    void Start()
    {
        isInit = false;
        thisCamera = GetComponent<Camera>();
        fov = thisCamera.fieldOfView = 50.0f;

		Transform p = this.transform.parent;

//        this.transform.parent = player.transform;
//        this.transform.localPosition = new Vector3(0.0f, 6.0f, -3.5f);
//		this.transform.parent = p;
		p.parent = player.transform;
		p.localPosition = new Vector3(0.0f, 6.0f, -3.5f);
		p.parent = null;
        initCameraPos = this.transform.position;
        initPlayerPos = player.transform.position;

        min *= TileInfoManager.instance.viewAround * 0.1f;
        max *= TileInfoManager.instance.viewAround * 0.1f;
        zoomSnap *= TileInfoManager.instance.viewAround * 0.1f;
        fov *= TileInfoManager.instance.viewAround * 0.1f;
        isInit = true;

        //StartCoroutine(UpdateTranslucent());

    }

    //IEnumerator UpdateTranslucent()
    //{
    //    RaycastHit[] arrHit = null;
    //    List<Material> listMaterial = new List<Material>();

    //    while (true)
    //    {
    //        //해제 먼저.
    //        if(arrHit != null)
    //        {
    //            foreach (RaycastHit hit in arrHit)
    //            {
    //                RollbackTranslucent(hit.transform, listMaterial);
    //            }
    //        }

    //        //반투명 적용
    //        //RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float maxDistance, int layermask);
    //        Debug.DrawRay(this.transform.position,
    //        Vector3.Normalize(player.transform.position - this.transform.position) * Vector3.Distance(player.transform.position, this.transform.position),
    //        Color.red);

    //        int mask = 1 << LayerMask.NameToLayer("FieldObject") | 1 << LayerMask.NameToLayer("Cube");

    //        arrHit = Physics.RaycastAll(this.transform.position
    //            , player.transform.position - this.transform.position
    //            , Vector3.Distance(player.transform.position, this.transform.position) * 0.9f
    //            , mask);

    //        foreach (RaycastHit hit in arrHit)
    //        {
    //            Debug.Log("name : " + hit.transform.name);
    //            //hit에 들어온 Object 들 hit포함 children에 갖고있는 MaterialComponent들을 뽑아 반투명한다.
    //            SetTranslucent(hit.transform, listMaterial);
    //        }

    //        yield return CoroutineManager.instance.GetWaitForSeconds(0.5f);
    //    }
    //}

    //void SetTranslucent(Transform _hitTransform, List<Material> _listPreColor)
    //{
    //    _listPreColor.Clear();

    //    //반투명 작업.
    //    foreach (MeshRenderer _meshRenderer in _hitTransform.GetComponentsInChildren<MeshRenderer>(true))
    //    {
    //        _listPreColor.Add(_meshRenderer.material);

    //        translucentMaterial.color = new Color(_meshRenderer.material.color.r
    //            , _meshRenderer.material.color.g
    //            , _meshRenderer.material.color.b
    //            , _meshRenderer.material.color.a * translucentAlpha);

    //        _meshRenderer.material = new Material(translucentMaterial);
    //    }
    //}


    //void RollbackTranslucent(Transform _hitTransform, List<Material> _listPreColor)
    //{
    //    int idx = 0;
    //    foreach (MeshRenderer _meshRenderer in _hitTransform.GetComponentsInChildren<MeshRenderer>(true))
    //    {
    //        _meshRenderer.material = new Material(_listPreColor[idx]);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        //들어왔을때 TranslucentObj 에 등록을 해야함.
        listTranslucentObj.Add(new TranslucentObj(other.gameObject, other.GetComponentsInChildren<MeshRenderer>(true)));

        //바꿔버림.
        foreach (MeshRenderer _meshRenderer in other.GetComponentsInChildren<MeshRenderer>(true))
        {
            translucentMaterial.color = new Color(_meshRenderer.material.color.r
                , _meshRenderer.material.color.g
                , _meshRenderer.material.color.b
                , _meshRenderer.material.color.a * translucentAlpha);
            //translucentMaterial.color = new Color(1.0f
            //    , 1.0f
            //    , 1.0f
            //    , 0.0f);

            _meshRenderer.material = new Material(translucentMaterial);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //나올때. list에서 find를 해야함.
        TranslucentObj translucentObj = listTranslucentObj.Find(_translucentObj => _translucentObj.obj == other.gameObject);
        listTranslucentObj.Remove(translucentObj);

        //원상태로 바꿈.
        int idx = 0;
        foreach (MeshRenderer _meshRenderer in other.GetComponentsInChildren<MeshRenderer>(true))
        {
            _meshRenderer.material = new Material(translucentObj.arrMtlOrigin[idx++]);
        }
    }

    void Update()
    {
        if (!isInit)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.P) && shake == false)
        {
            StartCoroutine(ShakeIt());
        }

        if (PopUpManager.instance.listPopUp.Find(popup => popup.name == "PopUpTest") == null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                fov += zoomSnap;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                fov -= zoomSnap;
            }

            if (fov < min)
            {
                fov = min;
            }
            else if (fov > max)
            {
                fov = max;
            }

            thisCamera.fieldOfView = Mathf.Clamp(fov, min, max);
        }


    }

    private void LateUpdate()
    {
		if (isInit)
        {
            increment = player.transform.position - initPlayerPos;
			this.transform.parent.position = new Vector3(initCameraPos.x + increment.x
                , initCameraPos.y + increment.y
                , initCameraPos.z + increment.z);
        }

    }
	IEnumerator ShakeIt()
	{
		float ttime = 0;
		Vector3 pos = transform.localPosition;
		shake = true;
		while (true) {
			ttime += Time.deltaTime;
			if (ttime >= 1) 
			{
				transform.localPosition = pos;
				shake = false;
				yield break;
			}
			transform.localPosition = pos + transform.up * Random.Range (-0.1f, 0.1f) + transform.right * Random.Range (-0.1f, 0.1f);//new Vector3 (Random.Range (-0.1f, 0.1f),0, Random.Range (-0.1f, 0.1f));
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}
}

public class TranslucentObj
{
    public GameObject obj;  //해당 오브젝트인지 판별용.
    public Material[] arrMtlOrigin;

    public TranslucentObj(GameObject _obj, MeshRenderer[] _arrMeshRendererOrigin)
    {
        int idx = 0;

        obj = _obj;
        arrMtlOrigin = new Material[_arrMeshRendererOrigin.Length];

        foreach(MeshRenderer meshRenderer in _arrMeshRendererOrigin)
        {
            arrMtlOrigin[idx++] = new Material(meshRenderer.material);
        }
    }
}