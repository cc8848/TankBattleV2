using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/**
 * 坦克对准玩家后就可以开火
 **/
public class EnemyShooting : MonoBehaviour {


	public Rigidbody m_Shell;            
	public Transform m_FireTransform; 
	public Slider m_AimSlider;           
	public AudioSource m_ShootingAudio;
	public AudioClip m_FireClip;

	public float m_MinLaunchForce = 15f; 
	public float m_MaxLaunchForce = 30f; 
	public float m_MaxChargeTime = 0.75f;

	private float m_CurrentLaunchForce;  
	private float m_ChargeSpeed;         
	//private bool m_Fired;

	private void OnEnable()
	{
		m_CurrentLaunchForce = m_MinLaunchForce;
		m_AimSlider.value = m_MinLaunchForce;
	}

	// Use this for initialization
	void Start () {
		m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartFire(float waitTime) {
		if (!gameObject.activeSelf) {
			return;
		}
		StartCoroutine (TankShooting (waitTime));
	}
		
	private IEnumerator TankShooting(float waitTime) {
		//积蓄火力,火力大小由waitTime决定
		m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
		m_AimSlider.value = m_CurrentLaunchForce;
		//Debug.Log ("===charging===");
		yield return new WaitForSeconds (waitTime);
		Fire ();
	}


	private void Fire()
	{
		// Instantiate and launch the shell.
		//m_Fired = true;
		//Debug.Log ("===Fire===");
		Rigidbody shellInstance = Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
		shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
		shellInstance.tag = "Enemy";

		m_ShootingAudio.clip = m_FireClip;
		m_ShootingAudio.Play ();

		m_CurrentLaunchForce = m_MinLaunchForce;
	}

}
