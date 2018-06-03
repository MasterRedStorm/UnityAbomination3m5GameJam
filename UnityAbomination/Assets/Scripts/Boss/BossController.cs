using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    private Animator animator;
    private bool isAttacking = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            AttackTop();
            
        }
        if (Input.GetKeyUp("space"))
        {
            AttackBottom();
        }

    }


    private void AttackTop()
    {
        Debug.Log("Start attack top");
        isAttacking = true;
        animator.SetBool("AttackUp", true);
        GetComponent<BossAttacks>().FlowTop();
    }

    private void AttackBottom()
    {
        Debug.Log("Start attack bot");
        isAttacking = true;
        animator.SetBool("AttackDown", true);
        GetComponent<BossAttacks>().FlowBottom();
    }

    private void SuperAttack()
    {
        Debug.Log("Start Superattack");
        isAttacking = true;
        GetComponent<BossAttacks>().SuperFlow();
    }

    private void ResetAttackAnimations()
    {
        animator.SetBool("AttackUp", false);
        animator.SetBool("AttackDown", false);
    }
}
