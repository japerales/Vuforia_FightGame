using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {


    public Transform enemy;
    [HideInInspector]
    public bool isAttacking = false;
    private Animator anim;
    private float speed;
    public float lookAtSpeed = 5f;
    public float health = 100;
    private float maxHealth;
    public string damageTag;
    public bool isControlledByPlayer;
    [SerializeField]
    public AnimStateColliderMediator[] stateColliderMediator;
    private AudioSource Audio;
    public AudioClip[] SoundClips;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public bool canMove;
    [HideInInspector]
    public bool EnemyHitByCurrentAttack;
    // Use this for initialization
    void Start () {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        maxHealth = health;
        Audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
	}

    private void PlaySound(int clip)
    {
        Audio.clip = SoundClips[clip];
        Audio.Play();
    }

	// Update is called once per frame
	void Update () {
    
    
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !anim.IsInTransition(0))
        {
            isAttacking = false;
            if (isControlledByPlayer)
            {
                LookForward();
            }
        }
        else
        {
                //if the state isn't idle lets check if its an attack from the mediator list
                foreach (AnimStateColliderMediator mediator in stateColliderMediator)
                {
                    AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
                    if (info.IsName(mediator.stateName))
                    {
                        if (info.normalizedTime < 1f && !anim.IsInTransition(0) && !EnemyHitByCurrentAttack)
                            mediator.collider.gameObject.tag = damageTag;
                        else if (anim.IsInTransition(0)) //animation finished
                        {
                            EnemyHitByCurrentAttack = false;
                            mediator.collider.gameObject.tag = "Untagged";
                        }
                    }
                }
        }
        if (isControlledByPlayer)
        {
            if (!isAttacking && health >0f)
            {
                speed = InputAndroid.Instance.GetXAxis();
                anim.SetFloat("Speed", speed);

                if (InputAndroid.Instance.GetButtonDown("Punch"))
                    Punch();

                if (InputAndroid.Instance.GetButtonDown("Kick"))
                    Kick();
            }
           
        }

    }

    public void Punch() {
        isAttacking = true;
        anim.SetFloat("Speed", 0.0f);
        anim.SetTrigger("Punch");
        PlaySound(0);
    }

    public void Kick()
    {
        isAttacking = true;
        anim.SetFloat("Speed", 0.0f);
        anim.SetTrigger("Kick");
        PlaySound(1);
    }

    public void React() {
        isAttacking = true;
        anim.SetFloat("Speed", 0.0f);
        health -= 10;
        if (health <= 0)
        {
            Knockout();
            PlaySound(2);
        }
        else
        {
            anim.SetTrigger("Reaction");
            PlaySound(3);
        }
    }

    public void Knockout()
    {
        anim.SetTrigger("KO");
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        health = maxHealth;
        anim.SetFloat("Speed", 0);
        anim.Play("Base Layer.Idle");
    }

    public void LookForward()
    {
        Vector3 direction = enemy.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), lookAtSpeed * Time.deltaTime);
    }
}
