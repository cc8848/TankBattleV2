using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;

//监控敌方坦克是否全部销毁
public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;           
       
    public Text m_MessageText;
    public GameObject m_EnemyTankPrefab;
	public GameObject m_PlayerTankPrefab;
	public EnemyManager[] m_EnimyTanks;
	public PlayerManager m_PlayerTank;
	public CameraControll m_CameraControl;

	private static int m_PlayerWins; //玩家赢的次数
	private static int m_enemyWins; //敌人赢的次数

    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
	private TankManager m_RoundWinner;
	private TankManager m_GameWinner;       

    private void Awake()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

		//m_PlayerTank = GameObject.FindGameObjectWithTag ("Player").Get;

		SpawnAllTanks ();
		SetCameraTarget ();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks() {
        for (int i = 0; i < m_EnimyTanks.Length; i++) {
			m_EnimyTanks[i].m_Instance = Instantiate(m_EnemyTankPrefab, m_EnimyTanks[i].m_SpawnPoint.position, m_EnimyTanks[i].m_SpawnPoint.rotation) as GameObject;
            m_EnimyTanks[i].m_EnemyNumber = i + 1;
            m_EnimyTanks[i].Setup();
        }

		if (m_PlayerTank.m_Instance == null) {
			m_PlayerTank.m_Instance = Instantiate (m_PlayerTankPrefab, m_PlayerTank.m_SpawnPoint.position, m_PlayerTank.m_SpawnPoint.rotation) as GameObject;
			m_PlayerTank.Setup ();
		}
    }

	void SetCameraTarget() {
		
		Transform[] target = new Transform[1];

		target[0] = m_PlayerTank.m_Instance.transform;
		m_CameraControl.m_Targets = target;

	}

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null) {
			m_PlayerWins = 0;
			m_enemyWins = 0;
            //TODO SceneManager.LoadScene(0);
        } else {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
		ResetAllTanks ();
		DisableTankControl ();

		//m_CameraControl.SetStartPositionAndSize ();

		m_RoundNumber++;
		m_MessageText.text = "第" + m_RoundNumber + "局";

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
		EnableTankControl ();
		m_MessageText.text = string.Empty;

		while (!SomeOneWin()) 
		{
			yield return null;	
		}
        
    }

	private IEnumerator RoundEnding()
	{
		DisableTankControl ();
		m_RoundWinner = GetRoundWinner ();
		m_GameWinner = GetGameWinner ();
		string message = EndMessage ();
		m_MessageText.text = message;

		yield return m_EndWait;
	}


	private TankManager GetRoundWinner() {
		if (!m_PlayerTank.m_Instance.activeSelf) {
			m_enemyWins++;
			return new EnemyManager ();
		} else {
			m_PlayerWins++;
			return new PlayerManager ();
		}
	}

	private TankManager GetGameWinner()
	{
		if (m_PlayerWins == m_NumRoundsToWin) {
			return new PlayerManager();
		} else if (m_enemyWins == m_NumRoundsToWin) {
			return new EnemyManager();
		}

		return null;
	}

    private bool SomeOneWin()
    {
        int numEnemysLeft = 0;

        for (int i = 0; i < m_EnimyTanks.Length; i++)
        {
			if (m_EnimyTanks[i].m_Instance != null && m_EnimyTanks[i].m_Instance.activeSelf)
				numEnemysLeft++;
        }

		return numEnemysLeft == 0 || m_PlayerTank.m_Instance.activeSelf == false;
    }


    private string EndMessage()
    {
        string message = "";

		if (m_RoundWinner != null) {
			if (m_RoundWinner is PlayerManager) {
				message = "祝贺你！你赢了这一局！";
			} else if (m_RoundWinner is EnemyManager) {
				message = "敌方坦克赢了这一局！";
			}
		}  

        message += "\n\n\n";
		message += "你赢了 " + m_PlayerWins + " 局\n";
		message += "敌方坦克赢了 " + m_enemyWins + " 局\n";


		if (m_GameWinner != null) {
			if (m_GameWinner is PlayerManager) {
				message = "牛逼！你是最后赢家！";
			} else if (m_GameWinner is EnemyManager) {
				message = "敌方坦克赢得比赛！";
			}
		}
        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_EnimyTanks.Length; i++)
        {
            m_EnimyTanks[i].Reset();
        }

		if (m_PlayerTank.m_Instance.activeSelf == false) {
			m_PlayerTank.Reset ();
		}
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_EnimyTanks.Length; i++)
        {
            m_EnimyTanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_EnimyTanks.Length; i++)
        {
            m_EnimyTanks[i].DisableControl();
        }
    }
}