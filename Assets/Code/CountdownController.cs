using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    //public Text countdownText;
    public TextMeshProUGUI countdownText;
    public GameObject wreck;
    private TextMeshProUGUI wreckText;

    private void OnGameStateChange(GameStateChangedEvent e)
    {
        if (e.next == EGameState.KAREN_CHASE)
        {
            countdownText.color = Color.red;
            countdownText.text = "Your owner is here. RUN!";
        }
    }

    void Start()
    {
        GameEvent<GameStateChangedEvent>.Register(OnGameStateChange);
        countdownText.text = "";
        wreckText = wreck.GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        GameEvent<GameStateChangedEvent>.Unregister(OnGameStateChange);
    }

    void Update()
    {
        if (HammyGameStateManager.Get().GameState == EGameState.FREE_ROAM)
        {
            wreckText.enabled = true;
            countdownText.text = "Owner arrives in : " + HammyGameStateManager.Get().GetFreeTimeLeft().ToString("F0");
        }
    }
}