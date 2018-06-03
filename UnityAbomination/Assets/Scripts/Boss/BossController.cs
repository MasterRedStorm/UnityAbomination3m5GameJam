using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    private Animator animator;
    private static bool isAttacking = false;
    private BossAttacks attackScript;
    private GameManager gameManager;
    private int attackCounter = 0;
    public int FlowTopTime = 10;
    public int FlowBottomTime = 20;
    public int FlowSuperTime = 30;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        attackScript = GetComponent<BossAttacks>();
        attackScript.AttackFinishedEvent += AttackScript_AttackFinishedEvent;
    }

    private void AttackScript_AttackFinishedEvent(BossAttacks.AttackType attackType)
    {
        ResetAttackAnimations();
    }

    // Update is called once per frame
    void Update () {

        if(gameManager.travelDistance > FlowTopTime && attackCounter == 0)
        {
            AttackTop();
            attackCounter++;
        }
        if (gameManager.travelDistance > FlowBottomTime && attackCounter == 1)
        {
            AttackBottom();
            attackCounter++;
        }
        if (gameManager.travelDistance > FlowSuperTime && attackCounter == 2)
        {
            SuperAttack();
            attackCounter++;
        }

    }


    private void AttackTop()
    {
        if (isAttacking == false)
        {
            Debug.Log("Start attack top");
            isAttacking = true;
            animator.SetBool("AttackUp", true);
            attackScript.FlowTop();
        }
    }

    private void AttackBottom()
    {
        if(isAttacking == false)
        {
            Debug.Log("Start attack bot");
            isAttacking = true;
            animator.SetBool("AttackDown", true);
            attackScript.FlowBottom();
        }

    }

    private void SuperAttack()
    {
        if (isAttacking == false)
        {
            Debug.Log("Start Superattack");
            isAttacking = true;
            attackScript.SuperFlow();
        }
    }

    private void ResetAttackAnimations()
    {
        Debug.Log("Attack finished");
        isAttacking = false;
        animator.SetBool("AttackUp", false);
        animator.SetBool("AttackDown", false);
    }
}
