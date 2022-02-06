
    using System.Collections;
    using UnityEngine;

    public class StateEnemyAttack:StateEnemyBase
    {
        public float attackFrequency = 2f;
        public int damage = 10;
        
        private Coroutine _attackCoroutine;
        public override void Enter()
        {
            Parent.targetGarden.OnPotagerdeath += StopAttack;
            _attackCoroutine = StartCoroutine(Attack());
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void Exit()
        {
            Parent.targetGarden.OnPotagerdeath -= StopAttack;
        }

        public override void BuildTransitions()
        {
            AddTransition(EnemyStateTransition.DIE, EnemyStateID.DEAD);
            AddTransition(EnemyStateTransition.STOP, EnemyStateID.IDLE);
        }

        private IEnumerator Attack()
        {
            while (Parent.targetGarden.isAlive())
            {
                yield return new WaitForSeconds(attackFrequency);
                Parent.targetGarden.TakeDamage(damage);
            }
        }
        
        private void StopAttack()
        {
            StopCoroutine(_attackCoroutine);
            MakeTransition(EnemyStateTransition.STOP);
        }
    }
