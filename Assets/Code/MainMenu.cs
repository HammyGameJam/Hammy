using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   public EZCameraShake.CameraShaker m_ball;

   public Animator m_ui;
   public Button m_play_button;
   public Button m_quit_button;
   public Button m_credits_button;
   public GameObject m_credit_page;
   public GameObject[] m_non_credit;

   //private bool moving = false;
   //public enum MMStates
   //{
   //   Quit = -1,
   //   Play = 0,
   //   Credits = 1
   //}
   //private MMStates m_state = MMStates.Play;

   private void Awake()
   {
      m_quit_button.interactable = false;
      m_credits_button.interactable = false;
      end_credits();
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.A)) {
         move_left();
      }
      if (Input.GetKeyDown(KeyCode.D)) {
         move_right();
      }

      Vector3 ball_default_rot = new Vector3(0.0f, 180.0f, 0.0f);
      ball_default_rot += m_ui.transform.rotation.eulerAngles;
      m_ball.RestRotationOffset = ball_default_rot;
   }

   public void move_left()
   {
      m_ui.SetTrigger("move left");
   }

   public void move_right()
   {
      m_ui.SetTrigger("move right");
   }

   public void start_game()
   {
      FadeManager.Instance.start_fade(new int[] {2, 1});
   }

   public void quit_game()
   {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
   }

   public void do_credits()
   {
      m_credit_page.SetActive(true);
      foreach (var ui in m_non_credit) {
         ui.SetActive(false);
      }
   }

   public void end_credits()
   {
      m_credit_page.SetActive(false);
      foreach (var ui in m_non_credit) {
         ui.SetActive(true);
      }
   }
}
