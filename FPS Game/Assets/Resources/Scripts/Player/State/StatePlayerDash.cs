using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEngine;

public class StatePlayerDash : StatePlayerBase
{
    public float dashSpeed = 10f;
    public float dashTime = 0.5f;
    [SerializeField] private AudioClip dashAudioClip;

    public override void Enter()
    {
        StartCoroutine(Dashing());
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void Exit()
    {
    }

    private IEnumerator Dashing()
    {
        Parent.canDash = false;
        SoundyManager.Play(dashAudioClip, Parent.transform);
        Parent.rigidbody.velocity = (new Vector3(Parent.moveVal.x, 0, Parent.moveVal.y).normalized * dashSpeed);
        yield return new WaitForSeconds(dashTime);
        Parent.canDash = true;
        MakeTransition(PlayerStateTransition.STOP);
    }

    public override void BuildTransitions()
    {
        AddTransition(PlayerStateTransition.START_ATTACK, PlayerStateID.ATTACK);
        AddTransition(PlayerStateTransition.STOP, PlayerStateID.IDLE);
    }
}