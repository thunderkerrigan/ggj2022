using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerIdle : StatePlayerBase
{
    public override void Enter()
    {
        Parent.animator.Play("Idle");
        Parent.rigidbody.velocity = Vector3.zero;
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
        if (Parent.moveVal.magnitude > 0.1f)
        {
            MakeTransition(PlayerStateTransition.START_MOVE);
        }
    }

    public override void Exit()
    {
    }

    public override void BuildTransitions()
    {
        AddTransition(PlayerStateTransition.START_MOVE, PlayerStateID.MOVE);
        AddTransition(PlayerStateTransition.START_DASH, PlayerStateID.DASH);
        AddTransition(PlayerStateTransition.START_ATTACK, PlayerStateID.ATTACK);
    }
}