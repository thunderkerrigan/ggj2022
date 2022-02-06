

using Overtime.FSM;

public enum EnemyStateID
{
    IDLE,
    ATTACK,
    MOVE,
    DEAD
}

public enum EnemyStateTransition
{
    START_MOVE,
    START_ATTACK,
    STOP,
    DIE
}

    public abstract class StateEnemyBase : State<FSMEnemy, EnemyStateID, EnemyStateTransition>
    {
    }
