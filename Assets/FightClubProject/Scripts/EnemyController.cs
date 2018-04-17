using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Transform player;
    public float lookAtSpeed = 7f;
    private Animator anim;
    public float Health = 100;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Vector3 direction = player.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), lookAtSpeed * Time.deltaTime);
        }
    }

    public void enemyReact()
    {
        Health -= 10;
    }
}
