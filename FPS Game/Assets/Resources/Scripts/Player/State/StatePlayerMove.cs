using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEngine;

public class StatePlayerMove : StatePlayerBase
{
    public float moveSpeed;
    private SoundyController _footStepSoundController;
    [SerializeField] private AudioClip footStepAudioClip;

    public override void Enter()
    {
        _footStepSoundController =
            SoundyManager.Play(footStepAudioClip, Parent.transform);
        _footStepSoundController.AudioSource.loop = true;
        _footStepSoundController.AudioSource.volume = 0.5f;
    }

    public override void Update()
    {
        if (Parent.moveVal.magnitude < 0.1f)
        {
            MakeTransition(PlayerStateTransition.STOP);
        }
    }

    public override void FixedUpdate()
    {
        switch (Parent.moveVal.Direction())
        {
            case Directions.UP:
                Parent.animator.Play("Bear_walking_back");
                break;
            case Directions.DOWN:
                Parent.animator.Play("Bear_walking_front");
                break;
            case Directions.LEFT:
                Parent.animator.Play("Bear_walking_left");
                break;
            case Directions.RIGHT:
                Parent.animator.Play("Bear_walking_right");
                break;
        }

        Parent.rigidbody.velocity = new Vector3(Parent.moveVal.x, 0, Parent.moveVal.y).normalized * moveSpeed;
    }

    public override void Exit()
    {
        if (_footStepSoundController)
        {
            _footStepSoundController.Stop();
        }
    }

    public override void BuildTransitions()
    {
        AddTransition(PlayerStateTransition.STOP, PlayerStateID.IDLE);
        AddTransition(PlayerStateTransition.START_ATTACK, PlayerStateID.ATTACK);
        AddTransition(PlayerStateTransition.START_DASH, PlayerStateID.DASH);
    }
}