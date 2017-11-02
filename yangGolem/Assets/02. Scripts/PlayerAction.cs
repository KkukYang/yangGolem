using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {

	

	public void OnAttackMove(int num)
    {
        StartCoroutine(CoAttackMove(num));
    }


    IEnumerator CoAttackMove(int num)
    {
        int count = 0;

        while (count < num)
        {
            count++;
            Player.instance.GetCharacterController().Move(transform.forward * Player.instance.attackMoveSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
	public void RollPlay()
    {
        StopCoroutine("CoRollMove");
        StartCoroutine("CoRollMove");
    }


    IEnumerator CoRollMove()
    {
        Player.instance.playerState = Player.PLAYERSTATE.ROLL;
        int count = 0;
        float speed = 1.5f;
        while (count < Player.instance.totalCount)
        {
            count++;
            Player.instance.GetCharacterController().Move(transform.forward * Time.deltaTime * speed * Player.instance.rollSpeed + transform.up * Player.instance.yVelocity * Time.deltaTime);
            if (count > Player.instance.accCount)
            {
                speed -= 0.1f;
                if (speed <= 0)
                    speed = 0;
            }
            
            yield return new WaitForFixedUpdate();
        }
        yield return null;
        Player.instance.OffRoll();
    }
}
