using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using EZCameraShake;
using Cinemachine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
   public QTEData m_start_qte;
   public CinemachineVirtualCamera m_camera_shake;
   public AnimationCurve m_shake_curve_mag;
   public AnimationCurve m_shake_curve_rough;

   public AnimationCurve m_animation_speed;

   private Animator m_animator;
   public PlayableDirector m_director;

   private bool m_qte_shack = false;
   private CameraShakeInstance m_shake_instance;

   public Animator m_cage;
   public Animator m_hamster;

    public NarrativeLine m_IntroMainLine;
    public NarrativeLine m_IntroEscapeLine;

   private void Awake()
   {
      GameEvent<LevelLoadedEvent>.Register(on_level_load);
      m_animator = GetComponent<Animator>();

      m_hamster.speed = m_cage.speed = m_animation_speed.Evaluate(0.0f);
   }

    private void OnDestroy()
    {
        GameEvent<LevelLoadedEvent>.Unregister(on_level_load);
    }

    public void on_level_load(LevelLoadedEvent e)
   {
      StartCoroutine("play_into_cinematic");
   }

   private IEnumerator play_into_cinematic()
   {
      m_director.Play();

      yield return new WaitForSecondsRealtime((float)m_director.duration);


        NarrativeAudioManager.Get().PlayLine(m_IntroMainLine);
        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitUntil( () => !NarrativeAudioManager.Get().IsPlaying() );

        QTEManager.Instance.play_qte(m_start_qte);
      m_qte_shack = true;
      GameEvent<QTEEndEvent>.Register(on_qte_end);
      m_shake_instance = new CameraShakeInstance(m_shake_curve_mag.Evaluate(0f), m_shake_curve_rough.Evaluate(0f));
      m_shake_instance.StartFadeIn(0.1f);

        NarrativeAudioManager.Get().PlayLine(m_IntroEscapeLine);
    }

   public void on_qte_end(QTEEndEvent e)
   {
      m_qte_shack = false;
      GameEvent<QTEEndEvent>.Unregister(on_qte_end);

      StartCoroutine("transition_to_game");
   }

   private IEnumerator transition_to_game()
   {
      m_cage.SetTrigger("Bust");
      m_hamster.SetTrigger("Bust");
      m_camera_shake.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0.3f;
      m_camera_shake.GetCinemachineComponent<CinemachineTrackedDolly>().m_ZDamping = 10.0f;

      m_cage.speed = 1.0f;
      m_hamster.speed = 1.0f;

      m_shake_instance.Magnitude = m_shake_curve_mag.Evaluate(1.0f);
      m_shake_instance.Roughness = m_shake_curve_rough.Evaluate(1.0f);
      m_shake_instance.StartFadeOut(1.0f);

      float time_left = 3f;
      while (time_left > 0.0f) {
         if (time_left >= 2f) {
            Vector3 pos = m_shake_instance.UpdateShake();
            m_camera_shake.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset = pos;
         } else {
            m_camera_shake.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset = Vector3.zero;
         }

         yield return null;
         time_left -= Time.deltaTime;
      }

      to_gameplay();
   }

   private void to_gameplay()
   {
      GamestateManager.Instance.start_gameplay();
      SceneManager.UnloadSceneAsync(1, UnloadSceneOptions.None);
   }

   private void Update()
   {
#if UNITY_EDITOR
      // SKIP
      if (Input.GetKeyDown(KeyCode.F1) && !m_qte_shack) {
         StopCoroutine("on_qte_end");

         to_gameplay();
      }
      if (Input.GetKeyDown(KeyCode.F2) && !m_qte_shack) {
         m_director.Stop();
         StopCoroutine("on_qte_end");

         QTEManager.Instance.play_qte(m_start_qte);
         m_qte_shack = true;
         GameEvent<QTEEndEvent>.Register(on_qte_end);
         m_shake_instance = new CameraShakeInstance(m_shake_curve_mag.Evaluate(0f), m_shake_curve_rough.Evaluate(0f));
         m_shake_instance.StartFadeIn(0.1f);
      }
#endif

      if (m_qte_shack) {
         m_shake_instance.Magnitude = m_shake_curve_mag.Evaluate(QTEManager.Instance.m_mash_amount);
         m_shake_instance.Roughness = m_shake_curve_rough.Evaluate(QTEManager.Instance.m_mash_amount);
         Vector3 pos = m_shake_instance.UpdateShake();
         m_camera_shake.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset = pos;

         m_hamster.speed = m_cage.speed = m_animation_speed.Evaluate(QTEManager.Instance.m_mash_amount);
      }
   }
}
