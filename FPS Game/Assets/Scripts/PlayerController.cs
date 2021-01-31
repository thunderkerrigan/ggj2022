using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Linq;
using Doozy.Engine;
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
    [SerializeField] AudioManager_Baby audioManager_Baby;

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
    public bool reverseControl = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerManager = PhotonView.Find((int) PV.InstantiationData[0]).GetComponent<PlayerManager>();
        var materialIndex = (int) PV.InstantiationData[1];
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

        CheckIsLookingDoudou();
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
        if (itemEnable != itemChild.activeSelf)
        {
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
            TakePowerUp();
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

    void CheckIsLookingDoudou()
    {
        var item = GetObjectOnClick();
        if (item == null) return;
        if (item.GetComponent<PhotonView>() == null) return;
        if (item.GetComponent<PickableItem>() == null) return;
        if (item.GetComponent<Doudou>() != null)
        {
            if (item.GetPhotonView().IsMine)
            {
                GameEventMessage.SendEvent("Use");
            }
            else
            {
                GameEventMessage.SendEvent("CantUse");
            }
        }
        else if (item.GetComponent<PowerUp>() != null)
        {
            GameEventMessage.SendEvent("Use");
        }
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (reverseControl)
        {
            moveDir = new Vector3(-Input.GetAxisRaw("Horizontal"), 0, -Input.GetAxisRaw("Vertical")).normalized;
        }

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * sprintSpeed, ref smoothMoveVelocity, smoothTime);
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
            this.playAudioClip("grunt");
        }
    }

    void TakeDoudou()
    {
        var item = GetObjectOnClick();
        if (item == null) return;
        if (item.GetComponent<PhotonView>() == null) return;
        if (!item.GetComponent<PhotonView>().IsMine) return;
        if (item.GetComponent<Doudou>() == null) return;
        this.playAudioClip("relieved");
        DoudouManager.Instance.onPlayerLootDoudou(item.GetComponent<PhotonView>().Owner, item);
        CanvasManager.Instance.showGoToEndZoneText();
    }

    void TakePowerUp()
    {
        var item = GetObjectOnClick();
        if (item == null) return;
        if (item.GetComponent<PhotonView>() == null) return;
        if (item.GetComponent<PickableItem>() == null) return;
        if (item.GetComponent<PowerUp>() != null)
        {
            if (!item.GetComponent<PhotonView>().IsMine) {
                item.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            switch (item.GetComponent<PowerUp>().type)
            {
                case PowerUpType.MachineGun:
                    StartCoroutine(startPowerUpMachineGunRoutine());
                    break;
                case PowerUpType.Speed:
                    StartCoroutine(startPowerUpSpeedRoutine());
                    break;
                case PowerUpType.ReverseControl:
                    TakeReverseControlMalus();
                    break;
                case PowerUpType.Stunt:
                    TakeStuntMalus();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.playAudioClip("surprised");

            PhotonNetwork.Destroy(item);
        }
    }

    void TakeStuntMalus()
    {
        Hashtable hash = new Hashtable();
        hash.Add("malus", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    void TakeReverseControlMalus()
    {
        Hashtable hash = new Hashtable();
        hash.Add("malus", 1);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    IEnumerator startPowerUpMachineGunRoutine()
    {
        foreach (var diaperWeapon in GetComponentsInChildren<DiaperWeapon>().ToList())
        {
            diaperWeapon.setCooldown(0.15f);
        }

        yield return new WaitForSeconds(5);
        foreach (var diaperWeapon in GetComponentsInChildren<DiaperWeapon>().ToList())
        {
            diaperWeapon.resetCooldown();
        }
    }

    IEnumerator startPowerUpSpeedRoutine()
    {
        var currentSprintSpeed = sprintSpeed;
        sprintSpeed = currentSprintSpeed * 2;
        yield return new WaitForSeconds(10);
        sprintSpeed = currentSprintSpeed;
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
            return gameObject;
        }

        return null;
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;

        itemIndex = _index;
        Item[] playerItem = items;
        if (PV.IsMine)
        {
            playerItem = items_local;
        }

        playerItem[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
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
        if (changedProps.ContainsKey("itemIndex"))
        {
            if (!PV.IsMine && targetPlayer == PV.Owner)
            {
                // Display weapon for other players
                if (!changedProps.ContainsKey("itemEnable"))
                {
                    EquipItem((int) changedProps["itemIndex"]);
                }
                else
                {
                    items[(int) changedProps["itemIndex"]].transform.GetChild(0).gameObject
                        .SetActive((bool) changedProps["itemEnable"]);
                }
            }
        }
        else if (changedProps.ContainsKey("audioClipIndex")) {
            // Play sound for other players
            if (!PV.IsMine && targetPlayer == PV.Owner) {
                this.playAudioClip((string) changedProps["audioClipType"], (int) changedProps["audioClipIndex"]);
            }
        }
        else if (changedProps.ContainsKey("malus"))
        {
            if (PV.IsMine && targetPlayer != PhotonNetwork.LocalPlayer)
            {
                if ((int) changedProps["malus"] == 0)
                {
                    StartCoroutine(startStuntRoutine());
                }
                else
                {
                    StartCoroutine(startReverseControlRoutine());
                }
            }
        }
    }

    IEnumerator startStuntRoutine()
    {
        // Stop
        canMove = false;
        moveAmount = Vector3.zero;
        animator.SetFloat("x", moveAmount.x);
        animator.SetFloat("z", moveAmount.z);
        animator.SetFloat("y", moveAmount.y);

        CanvasManager.Instance.showStunnedView(3);
        yield return new WaitForSeconds(3);
        canMove = true;
    }

    IEnumerator startReverseControlRoutine()
    {
        reverseControl = true;
        CanvasManager.Instance.showControlReverseView();
        yield return new WaitForSeconds(5);
        reverseControl = false;
    }

    public void SetGroundedState(bool _grounded, float _y)
    {
        currentFloorY = _y;
        grounded = _grounded;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            return;
        }

        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        this.playAudioClip("complaints");
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    public void GetStunned(float duration)
    {
        canMove = false;
        moveAmount = Vector3.zero;
        animator.SetFloat("x", moveAmount.x);
        animator.SetFloat("z", moveAmount.z);
        animator.SetFloat("y", moveAmount.y);
        animator.SetTrigger("died");
        CanvasManager.Instance.showStunnedView((int) duration);

        IEnumerator Stunned()
        {
            yield return new WaitForSeconds(duration);
            GetComponent<Rigidbody>().isKinematic = false;

            canMove = true;
        }

        StartCoroutine(Stunned());
    }

    public void GetDiapered(float duration)
    {
        print("GetDiapered");
        canMove = false;
        moveAmount = Vector3.zero;
        animator.SetFloat("x", moveAmount.x);
        animator.SetFloat("z", moveAmount.z);
        animator.SetFloat("y", moveAmount.y);
        CanvasManager.Instance.showStunnedView((int) duration);

        IEnumerator Stunned()
        {
            yield return new WaitForSeconds(duration);
            GetComponent<Rigidbody>().isKinematic = false;

            canMove = true;
        }

        StartCoroutine(Stunned());
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine)
            return;
        
        GetDiapered(2.0f);
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

    public void playAudioClip (string audioClipType, int givenClipIndex = -1) {
        int audioClipIndex = this.audioManager_Baby.playAudioClip(audioClipType, givenClipIndex);
        // givenClipIndex is set only when it's not the local player
        if(givenClipIndex == -1 && audioClipIndex != -1) {
            Hashtable hash = new Hashtable();
            hash.Add("audioClipIndex", audioClipIndex);
            hash.Add("audioClipType", audioClipType);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
		}
    }
}
