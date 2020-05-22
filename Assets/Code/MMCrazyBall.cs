using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class MMCrazyBall : MonoBehaviour
{

   private CameraShakeInstance m_shake;
   void Start()
   {
      m_shake = GetComponent<CameraShaker>().StartShake(100.0f, 0.5f, 0.1f);
   }
}
