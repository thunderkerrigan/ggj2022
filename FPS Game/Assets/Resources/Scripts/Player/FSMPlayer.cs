using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Overtime.FSM;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(AudioListener))]
public class FSMPlayer : MonoBehaviour
{
    private AudioListener m_audioListener;
    public AudioListener audioListener => m_audioListener;
    private Animator m_animator;
    public Animator animator => m_animator;
    private Rigidbody m_rigidbody;
    public new Rigidbody rigidbody => m_rigidbody;
    private PhotonView m_photonView;
    public PhotonView photonView => m_photonView;
    public GameObject weapon;
    private Vector2 m_moveVal, m_attackVal;
    public Vector2 moveVal => m_moveVal;
    public Vector2 attackVal => m_attackVal;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool canAttack = true;

    private StateMachine<FSMPlayer, PlayerStateID, PlayerStateTransition> m_FSM;
    public StateMachine<FSMPlayer, PlayerStateID, PlayerStateTransition> FSM => m_FSM;

    public PlayerStateID m_InitialState;

    public ScriptableObject[] m_States;

    public bool m_Debug;

    // Start is called before the first frame update
    void OnDestroy()
    {
        m_FSM.Destroy();
    }

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_photonView = GetComponent<PhotonView>();
        m_audioListener = GetComponent<AudioListener>();
        m_FSM = new StateMachine<FSMPlayer, PlayerStateID, PlayerStateTransition>(this, m_States, m_InitialState,
            m_Debug);
        if (PhotonNetwork.IsConnected)
        {
            if (m_photonView.IsMine)
            {
// TODO
            }
            else
            {
                Destroy(m_audioListener);
                Destroy(GetComponentInChildren<Camera>().gameObject);
                Destroy(m_rigidbody);
            }
        }

        canDash = true;
        canAttack = true;
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
            return;
        m_FSM.Update();
    }

    void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
            return;
        m_FSM.FixedUpdate();
    }

    void OnTriggerEnter(Collider col)
    {
        m_FSM.OnTriggerEnter(col);
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        if (m_Debug)
        {
            GUI.color = Color.white;
            GUI.Label(new Rect(0.0f, 0.0f, 500.0f, 500.0f), string.Format("Example State: {0}", FSM.CurrentStateName));
        }
    }
#endif

    // CONTROL MAPPING
    private void OnMove(InputValue value)
    {
        m_moveVal = value.Get<Vector2>();
        animator.SetFloat("x", moveVal.x);
    }

    private void OnLightAttack(InputValue value)
    {
        var newVal = value.Get<Vector2>();
        if (m_FSM.CurrentState.StateID != PlayerStateID.ATTACK && newVal.magnitude > 0.1f)
        {
            m_attackVal = newVal;
            m_FSM.MakeTransition(PlayerStateTransition.START_ATTACK);
        }
    }

    private void OnHeavyAttack(InputValue value)
    {
        Debug.Log("Heavy Attack");
    }

    private void OnDash(InputValue value)
    {
        Debug.Log("try to dash");
        if (moveVal.magnitude > 0.1f && canDash)
        {
            Debug.Log("dash");
            m_FSM.MakeTransition(PlayerStateTransition.START_DASH);
            // rigidbody.AddForce(new Vector3(moveVal.x, 0.1f, moveVal.y) * dashSpeed, ForceMode.VelocityChange);
            // StartCoroutine(DashCooldown());
        }
    }

    private void OnJoin(InputValue value)
    {
//      var spawnManager = GameObject.Find("PlayerSpawnManager").GetComponent<SpawnManager>();
        //    transform.position = spawnManager.GetSpawnpoint(0).position;
    }
}