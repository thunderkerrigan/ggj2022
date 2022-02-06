
    using System.Collections;
    using System.Collections.Generic;
    using Doozy.Engine.Soundy;
    using Photon.Pun;
    using UnityEngine;

    public class StateEnemyDead :StateEnemyBase
    {
        [SerializeField] private float _timeToDestroy = 60f;
        [SerializeField] private List<Sprite> bloodSprites;
        [SerializeField] private AudioClip splashClip;
        public override void Enter()
        {
            Parent.animator.SetFloat("x", Random.Range(0, 100));

            Debug.Log("Enemy " + this.gameObject.name + " took damage");
            Parent.navMeshAgent.enabled = false;
            Parent.animator.Play("Rabbit_dead");
            Parent.bloodPool.sprite = bloodSprites[Random.Range(0, bloodSprites.Count)];
            Parent.rigidbody.isKinematic = true;
            Parent.collider.enabled = false;
            SoundyManager.Play(splashClip, transform);
            StartCoroutine(DestroyAfter(_timeToDestroy));
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
           
        }
        
        private IEnumerator DestroyAfter(float time)
        {
            yield return new WaitForSeconds(time);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
