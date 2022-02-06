
    public class StateEnemyIdle :StateEnemyBase
    {
        public override void Enter()
        {
            // Search for next garden
            //Parent.FindNextGarden();
           var nextGarden =  GardenManager.Instance.FindNearestGardenFromPosition(Parent.transform);
            if (nextGarden != null)
            {
                Parent.targetGarden = nextGarden;
                Parent.navMeshAgent.SetDestination(nextGarden.transform.position);
                MakeTransition(EnemyStateTransition.START_MOVE);
            }
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

        public override void BuildTransitions()
        {
            AddTransition(EnemyStateTransition.DIE, EnemyStateID.DEAD);
            AddTransition(EnemyStateTransition.START_ATTACK, EnemyStateID.ATTACK);
            AddTransition(EnemyStateTransition.START_MOVE, EnemyStateID.MOVE);
        }
    }
