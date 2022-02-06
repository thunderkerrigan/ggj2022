
    using UnityEngine;

    public class StateEnemyMove:StateEnemyBase
    {
        public float speed;
        public override void Enter()
        {
            // TODO: subscribe to targetted garden
            Parent.targetGarden.OnPotagerdeath += MoveToNextGarden;
        }

        public override void Update()
        {
        }

        public override void FixedUpdate()
        {
            if (!Parent.photonView.IsMine)
            {
                return;
            }
            var direction = Parent.targetGarden.transform.position - this.transform.position;

                var look = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;

                if (look.y < 45 && look.y > 0 || look.y <= 360 && look.y >= 315)
                {
                    Parent.animator.Play("Rabbit_walking_back");
                }
                else if (look.y >= 45 && look.y < 135)
                {
                    Parent.animator.Play("Rabbit_walking_right");
                }
                else if (look.y >= 135 && look.y < 225)
                {
                    Parent.animator.Play("Rabbit_walking_front");
                }
                else if (look.y >= 225 && look.y < 315)
                {
                    Parent.animator.Play("Rabbit_walking_left");
                }

            Parent.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        public override void OnTriggerEnter(Collider collider)
        {
            var garden = collider.transform.gameObject.GetComponent<Garden>();
            if (collider.transform.gameObject.GetComponent<Garden>() != null)
            {
                MakeTransition(EnemyStateTransition.START_ATTACK);
            }
        }

        public override void Exit()
        {
            Parent.targetGarden.OnPotagerdeath -= MoveToNextGarden;
        }

        public override void BuildTransitions()
        {
            AddTransition(EnemyStateTransition.DIE, EnemyStateID.DEAD);
            AddTransition(EnemyStateTransition.START_ATTACK, EnemyStateID.ATTACK);
            AddTransition(EnemyStateTransition.STOP, EnemyStateID.IDLE);
        }

        private void MoveToNextGarden()
        {
            MakeTransition(EnemyStateTransition.STOP);
        }
    }
