using System;
using UnityEngine;

[Serializable]
public class EnemyManager : TankManager
{
    public Color m_EnemyColor;            
    public Transform m_SpawnPoint;         
    [HideInInspector] public int m_EnemyNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;                     


    private EnemyMovement m_Movement;       
    private EnemyShooting m_Shooting;
    private GameObject m_CanvasGameObject;


    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<EnemyMovement>();
		m_Shooting = m_Instance.GetComponent<EnemyShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        //m_Movement.m_PlayerNumber = m_PlayerNumber;
        //m_Shooting.m_PlayerNumber = m_PlayerNumber;

		m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_EnemyColor) + ">ENEMY " + m_EnemyNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
			renderers[i].material.color = m_EnemyColor;
        }
		//DisableControl ();
    }


    public void DisableControl()
    {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
