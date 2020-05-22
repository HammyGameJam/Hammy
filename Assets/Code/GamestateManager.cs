using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GamestateManager : MonoBehaviour
{
   private static GamestateManager _instance;
   public static GamestateManager Instance { get { return _instance; } }

   public GameObject m_camera;
   public GameObject m_controller;
   public GameObject m_ball;
   public GameObject m_cage;

   private void Awake()
   {
      if (_instance == null) {
         _instance = this;
         DontDestroyOnLoad(gameObject);
      } else if (_instance != this) {
         Debug.LogWarning("More than one Gamestate Manager!");
         Destroy(gameObject);
      }

      m_camera.SetActive(false);
      m_controller.SetActive(false);
      m_ball.SetActive(false);
      m_cage.SetActive(false);

      GameEvent<GameStateChangedEvent>.Register(on_game_state_change);
   }

   public void start_gameplay()
   {
      m_camera.SetActive(true);
      m_controller.SetActive(true);
      m_ball.SetActive(true);
      m_cage.SetActive(true);
      SceneManager.MoveGameObjectToScene(m_cage, SceneManager.GetSceneByName("World"));


      HammyGameStateManager.Get().EndIntro();
   }

   public void on_game_state_change(GameStateChangedEvent e)
   {
      if (e.next == EGameState.GAME_OVER || e.next == EGameState.GAME_OVER_WIN) {
         GameEvent<GameStateChangedEvent>.Unregister(on_game_state_change);
         gameplay_gameover();
      }
   }

   public void gameplay_gameover()
   {
      SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
      Destroy(gameObject);
   }

   public void gameplay_gameover_win()
   {
      SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
      Destroy(gameObject);
   }
}
