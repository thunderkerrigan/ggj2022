
    using System.Collections;
    using UnityEngine;

    public class StateEnemyIdle :StateEnemyBase
    {
        private  bool shouldSearchGarden = true;
        private Coroutine searchCoroutine;
        public override void Enter()
        {
            shouldSearchGarden = true;
            searchCoroutine = StartCoroutine(SearchGarden());
        }

        public override void Update()
        {
            if (searchCoroutine == null)
            {
                searchCoroutine = StartCoroutine(SearchGarden());
            }
        }

        public override void FixedUpdate()
        {
        }

        public override void Exit()
        {
            if (searchCoroutine != null)
            {
                StopCoroutine(searchCoroutine);
            }
        }

        public override void BuildTransitions()
        {
            AddTransition(EnemyStateTransition.DIE, EnemyStateID.DEAD);
            AddTransition(EnemyStateTransition.START_ATTACK, EnemyStateID.ATTACK);
            AddTransition(EnemyStateTransition.START_MOVE, EnemyStateID.MOVE);
        }

        private IEnumerator SearchGarden()
        {
            if (shouldSearchGarden)
            {
                if (GardenManager.Instance)
                {
                    var nextGarden =  GardenManager.Instance.FindNearestGardenFromPosition(Parent.transform);
                    if (nextGarden != null)
                    {
                        Parent.targetGarden = nextGarden;
                        Parent.navMeshAgent.SetDestination(nextGarden.transform.position);
                        MakeTransition(EnemyStateTransition.START_MOVE);
                        shouldSearchGarden = false;
                    }   
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
