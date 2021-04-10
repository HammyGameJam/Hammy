using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public struct LevelLoadedEvent : IGameEvent
{

}

[DisallowMultipleComponent]
public class FadeManager : MonoBehaviour
{
   private static FadeManager _instance;
   public static FadeManager Instance { get { return _instance; } }

   private Animator m_animator;

   private void Awake()
   {
      if (_instance == null) {
         _instance = this;
         DontDestroyOnLoad(gameObject);
      } else if (_instance != this) {
         Destroy(this.gameObject , 1.0f/30.0f);
         return;
      }

      m_animator = GetComponent<Animator>();
   }

   public void start_fade(int[] scene_index)
   {
      StartCoroutine("do_fade", scene_index);
   }

   private IEnumerator do_fade(int[] scene_index)
   {
      Scene[] current_scenes = SceneManager.GetAllScenes();
      m_animator.SetTrigger("fade in");

      yield return null;
      yield return new WaitForSecondsRealtime(m_animator.GetAnimatorTransitionInfo(0).duration);

      AsyncOperation[] async_operation_loads = new AsyncOperation[scene_index.Length];
      for (int i = 0; i < scene_index.Length; ++i) {
         async_operation_loads[i] = SceneManager.LoadSceneAsync(scene_index[i], LoadSceneMode.Additive);
      }
      float time = Time.unscaledTime;

      foreach (var async_operation_load in async_operation_loads) {
         while (!async_operation_load.isDone) {
            yield return null;
         }
      }
      SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(scene_index[0]));

      AsyncOperation[] async_operation_unloads = new AsyncOperation[current_scenes.Length];
      for (int i = 0; i < current_scenes.Length; ++i) {
         async_operation_unloads[i] = SceneManager.UnloadSceneAsync(current_scenes[i]);
      }

      foreach (var async_operation_unload in async_operation_unloads) {
         while (!async_operation_unload.isDone) {
            yield return null;
         }
      }

       LightProbes.Tetrahedralize();

      if ((time + 2) > Time.unscaledTime) {
         yield return new WaitForSecondsRealtime((time + 2) - Time.unscaledTime);
      }

      m_animator.SetTrigger("fade out");

      GameEvent<LevelLoadedEvent>.Post();
      yield return new WaitForSecondsRealtime(m_animator.GetAnimatorTransitionInfo(0).duration);

      Debug.Log("Load Complete!");
   }
}
