using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	public float sensitivity = 120.0f;
	public float moveSpeed = 7;
	public CharacterController _characterController;
	public float gravity = -9.8f;
	float yVelocity = 0;
	Vector3 moveDir;
	Vector3 rot;

	public static bool onMove { get; set; }

	public float attackMoveSpeed = 1;

	bool rollTrue = false;

	public int totalCount = 20;
	public int accCount = 10;
	public float rollSpeed = 7.5f;
	public float jumpSpeed = 5;
	public LayerMask layMask;

	void Start () {
		PlayerAnim.instance.PlayerIdle();
		//_characterController = GetComponent<CharacterController>();
	}

	void Update () {
		if (PlayerHp.instance.die == true)
			return;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		RaycastHit groundHit;
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");


		Debug.DrawRay(transform.position,transform.TransformDirection(-transform.up)*10,Color.green);
		if(Physics.Raycast(transform.position,transform.TransformDirection(-transform.up),
			out groundHit,0.1f))//if (_characterController.isGrounded == true)
		{
			if(groundHit.collider.CompareTag("Floor")){
				yVelocity = 0.0f;
				if (Input.GetKeyDown (KeyCode.Space)) {
					yVelocity = jumpSpeed;
				}
				if (Input.GetKeyDown(KeyCode.LeftShift) && rollTrue == false)
				{
					PlayerAnim.instance.PlayerRoll ();
				}
			}	
		}
		if (PlayerAttack.instance.onAttack == true) {
			
			if (Input.GetMouseButtonDown (0)) {
				if (Physics.Raycast (ray, out hit, Mathf.Infinity,layMask)) {
					if (hit.collider.CompareTag ("Floor")) {
						AttackDirection (hit.point.z - transform.position.z, hit.point.x - transform.position.x);
					}
				}
			}
			return;
		}


		moveDir = Vector3.zero;

		if (PlayerAnim.instance.playerState != PlayerAnim.PLAYERSTATE.Attack
		   && PlayerAnim.instance.playerState != PlayerAnim.PLAYERSTATE.Damage
		   && PlayerAnim.instance.playerState != PlayerAnim.PLAYERSTATE.Roll) {
			if (v != 0 || h != 0) {
			
				onMove = true;
				PlayerAnim.instance.PlayerRun ();
			} else {
				onMove = false;
				PlayerAnim.instance.PlayerIdle ();
			}
		}
			
		moveDir = MoveController(v, h) * moveSpeed;
		moveDir.y = yVelocity;
		yVelocity += gravity * Time.deltaTime;
		if (rollTrue == false)
			_characterController.Move (moveDir * Time.deltaTime);

	}
	private Vector3 MoveController(float v,float h)
	{
		Vector3 vec = Vector3.zero;

		if (v > 0)
		{
			if (h > 0)
			{
				vec = (Vector3.forward + Vector3.right).normalized;
				rot.y = 45;
			}
			else if (h < 0)
			{
				vec = (Vector3.forward - Vector3.right).normalized;
				rot.y = -45;
			}
			else
			{
				vec = Vector3.forward;
				rot.y = 0;
			}
		}
		else if (v < 0)
		{
			if (h > 0)
			{
				vec = (-Vector3.forward + Vector3.right).normalized;
				rot.y = 135;
			}
			else if (h < 0)
			{
				vec = (-Vector3.forward - Vector3.right).normalized;
				rot.y = -135;
			}
			else
			{
				vec = -Vector3.forward;
				rot.y = 180;
			}
		}
		else
		{
			if (h > 0)
			{
				vec = Vector3.right;
				rot.y = 90;
			}
			else if (h < 0)
			{
				vec = -Vector3.right;
				rot.y = -90;
			}
			else
			{
				//rot.y = 0;
			}
		}
		if(rollTrue == false)
			transform.rotation = Quaternion.Euler(rot);
		return vec;
	}

	public void OnAttackMove(int num)
	{
		if (onMove == true)
		num *= 2;
		StartCoroutine (CoAttackMove (num));
	}
	IEnumerator CoAttackMove(int num)
	{
		int count=0;

		while (count < num) {
			count++;
			_characterController.Move(transform.forward*attackMoveSpeed*Time.deltaTime);
			yield return new WaitForFixedUpdate ();
		}
		yield return null;
	}
	public void RollPlay()
	{
		StopCoroutine ("CoRollMove");
		StartCoroutine("CoRollMove");
	}

	IEnumerator CoRollMove()
	{
		rollTrue = true;
		int count=0;
		float speed = 1.5f;
		while (count < totalCount) {
			count++;
			_characterController.Move(transform.forward*Time.deltaTime *speed * rollSpeed + transform.up * yVelocity * Time.deltaTime);
			if (count > accCount) {
				speed -= 0.1f;
				if (speed <= 0)
					speed = 0;
			}
			yield return new WaitForFixedUpdate ();
		}
		yield return null;
		rollTrue = false;
	}

	private void AttackDirection(float v,float h)
	{

		float dis = 0.75f;
		if (v > dis)
		{
			if (h > dis)
			{
				rot.y = 45;
			}
			else if (h < -dis)
			{
				rot.y = -45;
			}
			else
			{
				rot.y = 0;
			}
		}
		else if (v < -dis)
		{
			if (h > dis)
			{
				rot.y = 135;
			}
			else if (h < -dis)
			{
				rot.y = -135;
			}
			else
			{
				rot.y = 180;
			}
		}
		else
		{
			if (h > dis)
			{
				rot.y = 90;
			}
			else if (h < -dis)
			{
				rot.y = -90;
			}
			else
			{
				//rot.y = 0;
			}
		}
		transform.rotation = Quaternion.Euler(rot);

	}
}
