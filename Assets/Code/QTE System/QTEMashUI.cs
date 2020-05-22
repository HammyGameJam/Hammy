using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class QTEMashUI : MonoBehaviour
{
   public Slider m_meter;

   [System.NonSerialized]
   public bool m_listen = false;

   private void Awake()
   {
      GameEvent<NewQTEEvent>.Register(on_new_qte);
      transform.GetComponentInParent<CanvasGroup>().alpha = 0f;
   }

   public void on_new_qte(NewQTEEvent e)
   {
      if (e.m_qte_data.m_type == QTEType.Mash) {
         // Our Type
         m_listen = true;
         GameEvent<QTEEndEvent>.Register(on_qte_end);
         transform.GetComponentInParent<CanvasGroup>().alpha = 1.0f;
         GameEvent<NewQTEEvent>.Unregister(on_new_qte);
        }
   }

   public void on_qte_end(QTEEndEvent e)
   {
      m_listen = false;
      if (e.m_win) {
         m_meter.value = 1.0f;
      }
      transform.parent.GetComponent<Animation>().Play();
      GameEvent<QTEEndEvent>.Unregister(on_qte_end);
   }

   private void LateUpdate()
   {
      m_meter.value = QTEManager.Instance.m_mash_amount;
   }
}
