using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewQTEEvent : IGameEvent
{
   public QTEData m_qte_data;
}

public class QTEEndEvent : IGameEvent
{
   public QTEData m_qte_data;
   public bool m_win;
}

[DisallowMultipleComponent, DefaultExecutionOrder(-1)]
public class QTEManager : MonoBehaviour
{
   private static QTEManager _instance;
   public static QTEManager Instance { get {
         if (_instance == null) {
            _instance = GameObject.FindObjectOfType<QTEManager>();
            if (_instance == null) {
               Debug.LogError("No QTE Manager. Prepare for more errors...");
               _instance = (new GameObject("QTE Manager", typeof(QTEManager))).GetComponent<QTEManager>();
            }
         }
         return _instance;
      } }

   public QTEData m_current_qte { get; private set; }

   public float m_mash_amount { get; private set; }

   public void play_qte(QTEData qte_data)
   {
      Debug.Assert(m_current_qte == null);
      m_current_qte = qte_data;

      switch (m_current_qte.m_type) {
         case QTEType.Mash:
            m_mash_amount = 0.0f;
            break;
         default:
            Debug.LogError(string.Format("No QTE Play for Type {0}", m_current_qte.m_type.ToString()));
            break;
      }

      NewQTEEvent e = new NewQTEEvent();
      e.m_qte_data = m_current_qte;
      GameEvent<NewQTEEvent>.Post(e);
   }

   private void Update()
   {
      if (m_current_qte != null) {
         switch(m_current_qte.m_type) {
            case QTEType.Mash:
               process_mash();
               break;
            default:
               Debug.LogError(string.Format("No QTE Process for Type {0}", m_current_qte.m_type.ToString()));
               break;
         }
      }
   }

   private void process_mash()
   {
      float input = Input.GetAxisRaw(m_current_qte.m_input);
      if (input == 1 && Input.GetButtonDown(m_current_qte.m_input)) {
         Debug.Log("Mash!");
         float power = m_current_qte.m_mash_power_curve.Evaluate(m_mash_amount);
         m_mash_amount += m_current_qte.m_mash_power * power;
      } else {
         float power = m_current_qte.m_mash_falloff_curve.Evaluate(m_mash_amount);
         m_mash_amount = Mathf.Max(m_mash_amount - (m_current_qte.m_mash_falloff * Time.deltaTime * power), 0f);
      }

      //Debug.Log(m_mash_amount);
      if (m_mash_amount >= 1) {
         Debug.Log("Win!");

         QTEEndEvent e = new QTEEndEvent();
         e.m_qte_data = m_current_qte;
         e.m_win = true;
         GameEvent<QTEEndEvent>.Post(e);

         m_current_qte = null;
      }
   }
}
