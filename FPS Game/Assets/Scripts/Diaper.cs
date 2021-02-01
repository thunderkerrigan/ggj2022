using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class Diaper : MonoBehaviour
{
    public PlayerController launcherPlayerController;
	public string launcherAudioType;
	public int launcherAudioClipIndex = -1;
	public string targetAudioType;
	public int targetAudioClipIndex = -1;
    [SerializeField] private AudioClip[] ImpactSounds;
    [SerializeField] private AudioClip ThrowSound;

    PhotonView PV;
    private void OnCollisionEnter(Collision other)
    {
        if (ImpactSounds.Length > 0)
        {
            SoundyManager.Play(ImpactSounds[Random.Range(0, ImpactSounds.Length - 1)], other.transform.position);
        }
        other.gameObject.GetComponent<IDamageable>()?.TakeDamage(60);
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            PhotonNetwork.Destroy(gameObject);

            
            PlayerController targetPlayerController =  other.gameObject.GetComponent<PlayerController>();
            
        }
        else
        {
            StartCoroutine(Despawn());
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        SoundyManager.Play(ThrowSound, transform);
    }

    private void FixedUpdate()
    {
        // TODO: destroy when out of bounds
        // var bounds = new Bounds(Vector3.zero, new Vector3(20,20,20));
        // if (!bounds.Contains(gameObject.transform.position))
        // {
        //     PhotonNetwork.Destroy(gameObject);
        // }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(gameObject);
    }
}
