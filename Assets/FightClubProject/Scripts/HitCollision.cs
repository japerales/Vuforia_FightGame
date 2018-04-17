using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollision : MonoBehaviour {

    public string hitByCollidersWithTag;
    public string hitComment;
    public PlayerController player;
    private float consecutiveHits = 0;
    private float resetTime;
	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(hitByCollidersWithTag))
        {
            PlayerController otherPC = other.GetComponentInParent<PlayerController>();
            other.gameObject.tag = "Untagged";
            otherPC.EnemyHitByCurrentAttack = true;
            player.React();
            /*consecutiveHits++;
            resetTime = Time.time + 1f;
            if (!(consecutiveHits>1))
                player.React();
            Debug.Log("GOLPES CONSECUTIVOS: " + consecutiveHits);
            */
        }
    }

    private void Update()
    {
        if (Time.time > resetTime)
            consecutiveHits = 0;
    }
}
