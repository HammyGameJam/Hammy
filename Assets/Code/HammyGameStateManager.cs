using TMPro;
using UnityEngine;

public enum EGameState
{
    INTRO,
    FREE_ROAM,
    KAREN_CHASE,
    BREAK_FREE,
    GAME_OVER,
    GAME_OVER_WIN
}

public struct GameStateChangedEvent : IGameEvent
{
    public EGameState previous;
    public EGameState next;
}


[DisallowMultipleComponent]
public class HammyGameStateManager : MonoBehaviour
{
    public float m_FreeRoamTime;

    public GameObject m_Karen;

    public NarrativeLine m_DestroyTheHouseLine;
    public NarrativeLine m_OwnerComesHomeLine;
    public NarrativeLine m_CaughtByOwnerLine;

    public int m_PointsToWin = 20;
    public GameObject m_WinBlinds;
    public GameObject m_WinWindow;
    public GameObject cc;

    public NarrativeLine m_AWindowHasOpenedLine;
    public NarrativeLine m_WellDoneHammy;


    public EGameState GameState { get; private set; }



    private static HammyGameStateManager m_inst = null;

    private Timer m_FreeRoamTimer = new Timer();


    public static HammyGameStateManager Get()
    {
        return m_inst;
    }
    
    public float GetFreeTimeLeft()
    {
        return m_FreeRoamTimer.TimeLeft();
    }

    public void EndIntro()
    {
        NarrativeAudioManager.Get().PlayLine(m_DestroyTheHouseLine);
        m_FreeRoamTimer.Start(m_FreeRoamTime);
        ChangeState(EGameState.FREE_ROAM);
    }

    public void Win()
    {
        NarrativeAudioManager.Get().PlayLine(m_WellDoneHammy);
        m_Karen.SetActive(false);
        ChangeState(EGameState.GAME_OVER_WIN);
        GamestateManager.Instance.gameplay_gameover_win();
    }



    

    private void Awake()
    {
        Debug.Assert(m_inst == null, "Should only be one GameState script in the scene.");
        if (m_inst == null)
        {
            m_inst = this;
        }

        GameEvent<MasterHandTouchedTargetEvent>.Register(OnKarenTouch);
    }

    private void OnDestroy()
    {
        GameEvent<MasterHandTouchedTargetEvent>.Unregister(OnKarenTouch);
    }

    private void Start()
    {
        GameState = EGameState.INTRO;
    }

    private void ChangeState(EGameState state)
    {
        GameStateChangedEvent e;
        e.previous = GameState;
        e.next = state;

        GameState = state;

        GameEvent<GameStateChangedEvent>.Post(e);
    }

    private void OnKarenTouch(MasterHandTouchedTargetEvent e)
    {
        NarrativeAudioManager.Get().PlayLine(m_CaughtByOwnerLine);
        m_Karen.SetActive(false);
        ChangeState(EGameState.GAME_OVER);
    }

    private void UpdateIntro()
    {
    }

    private void UpdateFreeRoam()
    {
        if (m_FreeRoamTimer.Elapsed())
        {
            m_FreeRoamTimer.Invalidate();

            NarrativeAudioManager.Get().PlayLine(m_OwnerComesHomeLine);

            // Let Karen roam free
            m_Karen.SetActive(true);
            HamsterBallSimulator.Get().AddGameObjectToMainPhysicsSim(m_Karen);

            ChangeState(EGameState.KAREN_CHASE);
        }
    }

    private void UpdateKarenChase()
    {
        if (PointTrackerScript.Get().Points >= m_PointsToWin)
        {
            m_WinBlinds.SetActive(false);
            m_WinWindow.SetActive(true);
            cc.SetActive(true);
            ChangeState(EGameState.BREAK_FREE);

            NarrativeAudioManager.Get().PlayLine(m_AWindowHasOpenedLine);
        }
    }

    private void UpdateBreakFree()
    {

    }

    private void UpdateGameOver()
    {
    }

    private void Update()
    {
        switch (GameState)
        {
            case EGameState.INTRO:
                break;

            case EGameState.FREE_ROAM:
                UpdateFreeRoam();
                break;

            case EGameState.KAREN_CHASE:
                UpdateKarenChase();
                break;

            case EGameState.BREAK_FREE:
                UpdateBreakFree();
                break;

            case EGameState.GAME_OVER:
                UpdateGameOver();
                break;
        }
    }
}
