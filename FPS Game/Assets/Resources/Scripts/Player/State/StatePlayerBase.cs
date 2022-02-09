using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Overtime.FSM;

public enum PlayerStateID
{
    IDLE,
    ATTACK,
    DASH,
    MOVE,
    DEAD
}

public enum PlayerStateTransition
{
    START_MOVE,
    START_ATTACK,
    START_DASH,
    STOP,
    DIE
}
public abstract  class StatePlayerBase : State<FSMPlayer, PlayerStateID, PlayerStateTransition>
{
   
}
