using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
	Transform player;
	TankHealth playerHealth;
	EnemyShooting enemyShooting;
	NavMeshAgent navAgent;

	void Awake() {
		Debug.Log ("===EnemyMovement Awake===");
	}

	void Start() {
		Debug.Log ("===EnemyMovement Start===");
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		playerHealth = player.GetComponent <TankHealth> ();
		enemyShooting = transform.GetComponent <EnemyShooting> ();
		navAgent = GetComponent <NavMeshAgent> ();

		InvokeRepeating ("ReadyToFire", 4f, 3f);
	}

	void Update() {
		if (playerHealth != null && playerHealth.m_CurrentHealth > 0) {
			navAgent.enabled = true;
			navAgent.SetDestination (player.position);
		} else {
			navAgent.enabled = false;
			//CancelInvoke ();
		}
	}

	void ReadyToFire() {
		//Debug.Log ("===ReadyToFire===");
		if (playerHealth != null && playerHealth.m_CurrentHealth > 0) {
			enemyShooting.StartFire (3f);
		}
	}
}


