using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pig : MonsterBehaviour
{

    float distance = 0;
    float currentHp = 0;
    float escapeDistance = 0;

    Collider collider;

    float rayLenth = 1.5f;
    public GeographyCube curCubeUnderMonster = new GeographyCube();

    //void Start()
    //{
    //    _behaviour = this;
    //    player = GameObject.FindWithTag("Player").transform;
    //    nav = GetComponent<NavMeshAgent>();
    //    anim = GetComponent<Animation>();
    //    monsterState = MONSTERSTATE.Idle;

    //    maxHp = hp;     //Max Hp
    //    currentHp = hp; //hp changing check
    //    escapeDistance = moveDistance * 4;

    //    gogi = null;
    //    collider = GetComponent<Collider>();

    //    NextState();

    //}

    private void OnEnable()
    {
        //Debug.Log("monster pig OnEnable()");

        _behaviour = this;
        player = GameObject.FindWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animation>();
        collider = GetComponent<Collider>();

        collider.enabled = true;
        nav.enabled = true;
        monsterState = MONSTERSTATE.Idle;

        maxHp = hp;     //Max Hp
        currentHp = hp; //hp changing check
        escapeDistance = moveDistance * 4;

        gogi = null;
        isDie = false;

        NextState();
    }

    protected override void Die()
    {
        Debug.Log("Pig Die()");

        collider.enabled = false;
        nav.enabled = false;

        GameObject _meat = null;
        if (ResourceManager.instance.itemBox.transform.Find("Meat") != null)
        {
            _meat = ResourceManager.instance.itemBox.transform.Find("Meat").gameObject;
        }
        else
        {
            _meat = Instantiate(ResourceManager.instance.item["Meat"] as GameObject) as GameObject;
        }

        _meat.name = "Meat";
        _meat.transform.parent = this.transform.parent;
        _meat.transform.position = new Vector3(curCubeUnderMonster.transform.position.x
                    , curCubeUnderMonster.transform.Find("InvisibleCube").GetComponent<BoxCollider>().bounds.max.y
                    , curCubeUnderMonster.transform.position.z);
        _meat.SetActive(true);
    }

    void Update()
    {
        if (!isDie)
        {
            RaycastHit hitTileCheck;
            int mask = 1 << LayerMask.NameToLayer("Cube");
            Debug.DrawRay(this.transform.position,
                transform.TransformDirection(-Vector3.up * rayLenth),
                Color.red);

            if (Physics.Raycast(this.transform.position,
                transform.TransformDirection(-Vector3.up),
                out hitTileCheck, rayLenth, mask))
            {
                if (hitTileCheck.transform.parent.GetComponent<GeographyCube>() != null)
                {
                    curCubeUnderMonster = hitTileCheck.transform.parent.GetComponent<GeographyCube>();

                    //Debug.Log (cube.positionID + " " + cube.layerID);
                    monsterInfo.positionID = curCubeUnderMonster.positionID;
                    monsterInfo.layerID = curCubeUnderMonster.layerID;
                }
            }
        }





        if (monsterState != MONSTERSTATE.Damage && monsterState != MONSTERSTATE.Death)
        {
            distance = (player.position - this.transform.position).magnitude;

            if (null != gogi)
            {
                if (currentHp == hp)
                {
                    if (monsterState != MONSTERSTATE.Eat)
                        monsterState = MONSTERSTATE.Eat;
                }
                else
                {
                    if (distance > moveDistance * 2)
                    {
                        monsterState = MONSTERSTATE.Idle;
                        currentHp = hp;
                    }
                    else
                    {
                        if (monsterState != MONSTERSTATE.Attack)
                            monsterState = MONSTERSTATE.Attack;
                    }
                }
                return;
            }

            if (distance <= moveDistance && monsterState == MONSTERSTATE.Idle)
            {
                if (monsterState != MONSTERSTATE.Move)
                    monsterState = MONSTERSTATE.Move;
            }
            else if (distance > escapeDistance && monsterState == MONSTERSTATE.Move)
            {
                if (monsterState != MONSTERSTATE.Idle)
                    monsterState = MONSTERSTATE.Idle;
            }
        }
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Sword"))
        {
            --hp; //hp -= Player.instance.ATK;
            monsterState = MONSTERSTATE.Damage;
        }
    }
}
