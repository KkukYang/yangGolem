using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float sensitivity = 120.0f;
    public float moveSpeed = 5;
    public float jumpSpeed = 5;
    CharacterController _characterController;
    public float gravity = -9.8f;
    float yVelocity = 0;

    private PlayerAnim pa;
    Vector3 rot;

	void Start () {
        pa = GetComponent<PlayerAnim>();
       _characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (TotalData.onCar != false) return;
        if (TotalData.onCarMaker != false) return;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 moveDir;

        if (v != 0 || h != 0) {
            pa.PlayMoveAnim();
        }
        else
        {
            pa.PlayWaitAnim();
        }
        if (_characterController.isGrounded == true)
        {
            yVelocity = 0.0f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            yVelocity = jumpSpeed;
        }

        moveDir = MoveController(v, h) * moveSpeed;
        moveDir.y = yVelocity;
        yVelocity += gravity * Time.deltaTime;
        _characterController.Move(moveDir* Time.deltaTime);
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
        transform.rotation = Quaternion.Euler(rot);
        return vec;
    }
}
