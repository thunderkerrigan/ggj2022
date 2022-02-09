using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEngine;

public class StatePlayerAttack : StatePlayerBase
{
    public float attackTime = 0.4f;
    public float moveSpeed;
    [SerializeField] private AudioClip swingAudioClip;


    public override void Enter()
    {
        Parent.canAttack = false;
        SoundyManager.Play(swingAudioClip, Parent.transform);
        StartCoroutine(AttackDelay());
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
        Parent.rigidbody.velocity = new Vector3(Parent.moveVal.x, 0, Parent.moveVal.y).normalized * moveSpeed;
    }

    private IEnumerator AttackDelay()
    {
        
        switch (Parent.attackVal.Direction())
        {
            case Directions.UP:
                Parent.animator.Play("Bear_attacking_back");
                break;
            case Directions.DOWN:
                Parent.animator.Play("Bear_attacking_front");
                break;
            case Directions.LEFT:
                Parent.animator.Play("Bear_attacking_left");
                break;
            case Directions.RIGHT:
                Parent.animator.Play("Bear_attacking_right");
                break;
        }

        Parent.weapon.transform.rotation = Quaternion.LookRotation(new Vector3(Parent.attackVal.x, 0, Parent.attackVal.y), Vector3.up);
        Parent.weapon.GetComponent<WeaponHandler>().TriggerWeapon();
        yield return new WaitForSeconds(attackTime);
        
        MakeTransition(Parent.moveVal.magnitude > 0.1f ? PlayerStateTransition.START_MOVE : PlayerStateTransition.STOP);
    }

    public override void Exit()
    {
        Parent.canAttack = true;
    }

    public override void BuildTransitions()
    {
        AddTransition(PlayerStateTransition.STOP, PlayerStateID.IDLE);
        AddTransition(PlayerStateTransition.START_MOVE, PlayerStateID.MOVE);
    }
}