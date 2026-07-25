using System;
using InputSystem;
using UnityEngine;
using UnityEngine.EventSystems;

// клас відповідає за оновлення поточної кінської сила та швидкості оберту ЦД
namespace Engines
{
    public class MainHeliEngine : MonoBehaviour
    {
        [Header("Engine Characteristics")]
        [SerializeField] private float maxHp = 140f;
        [SerializeField] float maxRpm = 2700f;
        public float MaxRpm => maxRpm;
        
        // час у секундах, за який двигун розкручується з 0 до 100%
        [SerializeField] private float engineResponseTime = 5.0f;
        // Коефіцієнт опору повітря при максимальному кроці гвинта
        [SerializeField] private float rotorDragFactor = 0.3f;
        
        
        [SerializeField] private AnimationCurve powerCurve 
            = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        
        public Action<float> OnChangeCurrentHp;
        public Action<float> OnChangeCurrentRpm;
        
        private float _lastSentHp;
        private float _lastSentRpm;
        
        // поточна кінська сила
        public float CurrentHp { get; private set; }
        // поточний обертальний момент
        public float CurrentRpm { get; private set; }
        
        public void UpdateEngine(BaseHeliInput input)
        {
            float targetPowerFactor = powerCurve.Evaluate(input.ThrottleInput);
            
            float targetHp = targetPowerFactor * maxHp;
            float targetRpm = targetPowerFactor * MaxRpm;
            
            float hpChangeRate = maxHp / engineResponseTime;
            float rpmChangeRate = MaxRpm / engineResponseTime;

            CurrentHp = Mathf.MoveTowards(CurrentHp, targetHp, hpChangeRate * Time.deltaTime);
            CurrentRpm = Mathf.MoveTowards(CurrentRpm, targetRpm, rpmChangeRate * Time.deltaTime);
            
            // 2. Рахуємо навантаження: наскільки крок гвинта забирає потужність
            // Чим більший collectiveInput (крок), тим сильніше гальмуються оберти
            float hpRatio = maxHp > 0 ? (CurrentHp / maxHp) : 0f;
            float loadPenalty = input.CollectiveInput * rotorDragFactor; 
            
            // Якщо потужності не вистачає на покриття навантаження, цільові оберти знижуються
            float effectiveTargetRpm = targetRpm * Mathf.Clamp01(1f - loadPenalty + (hpRatio * 0.2f));

            // 3. Плавно змінюємо поточні оберти до підсумкових з урахуванням навантаження
            CurrentRpm = Mathf.MoveTowards(CurrentRpm, effectiveTargetRpm, rpmChangeRate * Time.deltaTime);
            
            if (!Mathf.Approximately(CurrentHp, _lastSentHp))
            {
                _lastSentHp = CurrentHp;
                OnChangeCurrentHp?.Invoke(CurrentHp);
            }

            if (!Mathf.Approximately(CurrentRpm, _lastSentRpm))
            {
                _lastSentRpm = CurrentRpm;
                OnChangeCurrentRpm?.Invoke(CurrentRpm);
            }
        }
    }
}