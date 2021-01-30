using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public delegate void CooldownUpdateHandler(int position, float value);

public delegate void ScoreUpdateHandler(int score);

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    [SerializeField] Item[] items;
    [SerializeField] Item[] items_local;

    [SerializeField] Animator animator;

    int itemIndex;
    int previousItemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    public event CooldownUpdateHandler OnCoolDownUpdate;

    public PhotonView PV;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    int rayDistance = 2;
    private float currentFloorY = -1;
    PlayerManager playerManager;

    private List<Coroutine> CooldownCoroutines = new List<Coroutine>();

    public bool canMove = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerManager = PhotonView.Find((int) PV.InstantiationData[0]).GetComponent<PlayerManager>();
        var materialIndex = (int)PV.InstantiationData[1];
        GetComponentInChildren<CaracterHolder>().updateMaterial(materialIndex);
    }


    void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        Look();

        if (!canMove)
        {
            return;
        }

        Move();
        Jump();

        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
            {
                EquipItem(0);
            }
            else
            {
                EquipItem(itemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                EquipItem(items.Length - 1);
            }
            else
            {
                EquipItem(itemIndex - 1);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            var fired = items[itemIndex].Use();
            if (fired != -1)
            {
                if (OnCoolDownUpdate != null)
                {
                    OnCoolDownUpdate(itemIndex, fired);
                }
            }
        }

        // Hide/show current item depend on it's CD
        bool itemEnable = ((Weapon) items[itemIndex]).enable;
        GameObject itemChild = items_local[itemIndex].transform.GetChild(0).gameObject;
        if(itemEnable != itemChild.activeSelf ) {
            itemChild.SetActive(itemEnable);
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            hash.Add("itemEnable", itemEnable);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            // items[itemIndex].Use();
            // remove Object
            TakeDoudou();
        }

        if (transform.position.y < -10f) // Die if you fall out of the world
        {
            Die();
        }
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }


    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount,
            moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);

        animator.SetFloat("x", moveAmount.x);
        animator.SetFloat("z", moveAmount.z);
        animator.SetFloat("y", moveAmount.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            animator.Play("Jump_to_Run");
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void TakeDoudou()
    {
        var item = GetObjectOnClick();
        if (item != null && item.GetComponent<PhotonView>() == null) return;
        if (!item.GetComponent<PhotonView>().IsMine) return;
        DoudouManager.Instance.onPlayerLootDoudou(item.GetComponent<PhotonView>().Owner, item);
    }

    void DropItem()
    {
        var frontPosition = GetComponentInChildren<Camera>().transform.TransformPoint(Vector3.forward * rayDistance);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Cube"), frontPosition, Quaternion.identity);
    }

    Vector3[] GetClickPositionAndNormal()
    {
        Vector3[] returnData = new Vector3[] {Vector3.zero, Vector3.zero}; //0 = spawn poisiton, 1 = surface normal
        Ray ray = GetComponentInChildren<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            returnData[0] = hit.point;
            returnData[1] = hit.normal;
        }

        return returnData;
    }

    GameObject GetObjectOnClick()
    {
        var ray = GetComponentInChildren<Camera>().ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        var hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            var gameObject = hit.transform.gameObject;
            print(gameObject);
            return gameObject;
            /*
            FindObjectOfType<ScoreCanvasManager>().gameObject.GetComponent<TextMeshProUGUI>().text = $"Looting DOUDOU";
            
            if (gameObject.tag == "Destructible")
            {
                return gameObject;
            }
            */
        }

        return null;
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;

        itemIndex = _index;
        Item[] playerItem = items;
		if (PV.IsMine) { playerItem = items_local; }

        playerItem[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1){
            playerItem[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("itemIndex")) return;
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            // Display weapon for other players
            if (!(bool) changedProps.ContainsKey("itemEnable")) { EquipItem((int) changedProps["itemIndex"]); }
            else{ items[(int) changedProps["itemIndex"]].transform.GetChild(0).gameObject.SetActive((bool) changedProps["itemEnable"]); }
        }
    }

    public void SetGroundedState(bool _grounded, float _y)
    {
        currentFloorY = _y;
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine) { return; }

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        foreach (var routine in CooldownCoroutines)
        {
            StopCoroutine(routine);
        }

        playerManager.Die();
    }
}