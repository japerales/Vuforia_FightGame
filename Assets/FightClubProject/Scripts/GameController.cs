using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
public class GameController : Singleton<GameController> {


    //Inner Logic
    [HideInInspector]
    private bool gameStarted; //controls when the game starts.
    public bool TargetReady; //controls when the targets are enabled
    //UI
    public GameObject FightCanvas;
    //Players
    private GameObject[] Fighters;
    //Audio
    public AudioSource Commentator;
    public AudioSource Music;
    public AudioClip[] soundclips;
    //FightVariables
    [HideInInspector]
    public int round = 1;
    [HideInInspector]
    public int roundsWonByPlayer1 = 0;
    [HideInInspector]
    public int roundsWonByPlayer2 = 0;
    //WaitTimes
    public float WaitCheckKOInterval = 0.5f; //interval of time to check if any of the fighters are KO
    public float WaitTimeToNextRound = 3.0f; //wait time just after one fighter is KO until next round;
    public float WaitToAnnounceWinner = 1.0f;

    private void OnEnable()
    {
        DefaultTrackableEventHandler.OnTrackableBehaviorRegister += GetFighters;
        DefaultTrackableEventHandler.OnTrackingFoundEvent += GameStart;
    }

    private void OnDisable()
    {
        DefaultTrackableEventHandler.OnTrackableBehaviorRegister -= GetFighters;
        DefaultTrackableEventHandler.OnTrackingFoundEvent -= GameStart;
    }

    // Use this for initialization
    void Start () {
        Fighters = new GameObject[2];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GetFighters(TrackableBehaviour trackableBehaviour)
    {
        if (TargetReady)
        {
            int i = 0;
            foreach (Transform transform in trackableBehaviour.transform)
            {
                Fighters[i] = transform.gameObject;
                i++;
            }
        }
    }

    public void EnableFighters()
    {
        if (TargetReady)
        {
            foreach (GameObject go in Fighters)
            {
                if (go.GetComponent<PlayerController>())
                    go.GetComponent<PlayerController>().enabled = true;

                if (go.GetComponent<BasicAI>())
                    go.GetComponent<BasicAI>().enabled = true;
            }
        }
    }

    public void DisableFighters()
    {
        if (TargetReady)
        {
            foreach (GameObject go in Fighters)
            {
                if (go.GetComponent<PlayerController>())
                    go.GetComponent<PlayerController>().enabled = false;

                if (go.GetComponent<BasicAI>())
                    go.GetComponent<BasicAI>().enabled = false;
            }
        }
    }

    private void PlaySound(AudioSource source, int clip)
    {
        source.clip = soundclips[clip];
        source.Play();
    }

    private void EnableFightUI()
    {
        FightCanvas.SetActive(true);
        //enlazamos las barras de vida con sus personajes
        HealthBarChange[] bars = FightCanvas.GetComponentsInChildren<HealthBarChange>();
        
        for (int i=0; i<bars.Length;i++)
        {
            if (Fighters != null && Fighters[i].GetComponent<PlayerController>())
            {
                bars[i].setPlayerController(Fighters[i].GetComponent<PlayerController>());
                bars[i].setPlayerBarName(Fighters[i].gameObject.name);
            }
        }
    }

    private void DisableFightUI()
    {
        FightCanvas.SetActive(false);
    }

    IEnumerator RoundAnnouncement(int round)
    {
        //Prepare yourself
        PlaySound(Commentator, 3);
        yield return new WaitForSeconds(soundclips[3].length + 0.5f);

        //round...
        switch (round)
        {
            case 1: PlaySound(Commentator, 1); break;
            case 2: PlaySound(Commentator, 2); break;
            case 3: PlaySound(Commentator, 7); break;
        }
        yield return new WaitForSeconds(1.5f);
        //3,2,1...FIGHT!
        PlaySound(Commentator, 6);
        yield return new WaitForSeconds(soundclips[6].length);
    }


    IEnumerator GameLoop()
    {
        PlayerController player1 = Fighters[0].GetComponent<PlayerController>();
        PlayerController player2 = Fighters[1].GetComponent<PlayerController>();
        

        while (round <= 3 && roundsWonByPlayer1<2 && roundsWonByPlayer2<2)
        {
            yield return StartCoroutine(RoundAnnouncement(round));
            EnableFighters();
            //wait time during fight
            while (player1.health > 0 && player2.health > 0)
            {
                yield return new WaitForSeconds(WaitCheckKOInterval);
            }
            //better this way, sometimes double KO can happens.
            if (player1.health > 0)
            {
                roundsWonByPlayer1++;
                IncrementPlayerWinUI(1, roundsWonByPlayer1);
            }
            if (player2.health > 0)
            {
                roundsWonByPlayer2++;
                IncrementPlayerWinUI(2, roundsWonByPlayer2);
            }
            yield return new WaitForSeconds(WaitTimeToNextRound);

            //reset players only if there isn't a match winner
            if (roundsWonByPlayer1 < 2 && (roundsWonByPlayer2 < 2))
            {
                player1.ResetPosition();
                player2.ResetPosition();
            }
            DisableFighters(); //until next round
            round++;
        }

        //WIN LOGIC
        yield return new WaitForSeconds(WaitToAnnounceWinner);
        if (roundsWonByPlayer1 == 2) //if player 1 wins...
            PlaySound(Commentator, 4); //YOU WIN!!
        if (roundsWonByPlayer2 == 2) //if player 2 loses
            PlaySound(Commentator, 5); //YOU LOSE!!

        //AFTER FIGHT LOGIC
        SceneManager.LoadScene("Menu");

    }

    //cant use TargetReady cause has a different job
    public void GameStart()
    {
        //the game is enabled only once
        if (!gameStarted)
        {
            gameStarted = true;
            EnableFightUI();
            PlaySound(Music, 0);

            StartCoroutine(GameLoop());
        }
    }

    public void IncrementPlayerWinUI(int player, int win)
    {
        WinLight[] winLights = FightCanvas.GetComponentsInChildren<WinLight>();

        foreach (WinLight winLight in winLights)
        {
            if (winLight.Player == player)
                winLight.setWin(win);
        }
    }

}
