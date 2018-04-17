using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class BasicAI : MonoBehaviour {

    private float distanceWithPlayer;
    private bool facingEnemy;
    private PlayerController playerController;
    private Animator anim;
    public float DistanceToMoveForward;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        
	}
	
	// Update is called once per frame
	void Update () {

        
        playerController.LookForward();

        distanceWithPlayer = Vector3.Magnitude(transform.position - playerController.enemy.position);
        //usamos Vector3.dot para saber si estan de frente. El valor será siempre positivo si están de frente
        /*Vector3 relativeEnemyPosition = transform.InverseTransformPoint(playerController.enemy.position);
        float DotIAtoFighter = Vector3.Dot(Vector3.forward, relativeEnemyPosition.normalized);
        facingEnemy = DotIAtoFighter >= DotFacingValue ? true : false;
        Debug.Log("iS FACING ENEMY?: " + facingEnemy + ", DOT POSITION: " + DotIAtoFighter);
        */
            if (distanceWithPlayer > DistanceToMoveForward)
            {
                anim.SetFloat("Speed", 1);
            }
            else {
                anim.SetFloat("Speed", 0);

                if (!playerController.isAttacking)
                {
                    int randomAttack = (int)Random.Range(0, 2);

                    switch (randomAttack) {
                        case 0: playerController.Punch(); break;
                        case 1: playerController.Kick(); break;
                    }

                }
            }
	}
}
