using System;
using Overtime.FSM;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]

public class FSMEnemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private SpriteRenderer m_bloodPool;
        public SpriteRenderer bloodPool => m_bloodPool;
        private Rigidbody m_rigidbody;
        public new Rigidbody rigidbody => m_rigidbody;
        private NavMeshAgent m_navMeshAgent;
        public NavMeshAgent navMeshAgent => m_navMeshAgent;
        private NavMeshObstacle m_navMeshObstacle;
        public NavMeshObstacle navMeshObstacle => m_navMeshObstacle;
        private Animator m_animator;
        public Animator animator => m_animator;
        private PhotonView m_photonView;
        public PhotonView photonView => m_photonView;
        [SerializeField]
        private Collider m_collider;
        public new Collider collider => m_collider;

            [HideInInspector]
        public Garden targetGarden;
        private StateMachine<FSMEnemy, EnemyStateID, EnemyStateTransition> m_FSM;
        public StateMachine<FSMEnemy, EnemyStateID, EnemyStateTransition> FSM => m_FSM;

        public EnemyStateID m_InitialState;

        public ScriptableObject[] m_States;

        public bool m_Debug;

        void OnDestroy()
        {
            m_FSM.Destroy();
        }

        void Start()
        {
            m_navMeshAgent = GetComponent<NavMeshAgent>();
            m_navMeshObstacle = GetComponent<NavMeshObstacle>();
            m_animator = GetComponent<Animator>();
            m_photonView = GetComponent<PhotonView>();
            m_rigidbody = GetComponent<Rigidbody>();
            
            m_FSM = new StateMachine<FSMEnemy, EnemyStateID, EnemyStateTransition>(this, m_States, m_InitialState, m_Debug);
        }

        void Update()
        {
            m_FSM.Update();
        }

        void FixedUpdate()
        {
            m_FSM.FixedUpdate();
        }

        void OnTriggerEnter(Collider col)
        {
            m_FSM.OnTriggerEnter(col);
        }

#if UNITY_EDITOR
        void OnGUI()
        {
            if(m_Debug)
            {
                GUI.color = Color.white;
                GUI.Label(new Rect(0.0f, 0.0f, 500.0f, 500.0f), string.Format("Example State: {0}", FSM.CurrentStateName));
            }
        }
#endif
        public void TakeDamage(float damage)
        {
            if (m_FSM.CurrentState.StateID != EnemyStateID.DEAD)
            {
                m_FSM.MakeTransition(EnemyStateTransition.DIE);
            }
        }
    }
    
