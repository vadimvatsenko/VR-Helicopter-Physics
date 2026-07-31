using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class TimeScreen : UIScreen
    {
        [SerializeField] private float screenTime = 2f;
        
        public Action onTimeComplete;

        private float _time;

        public override void ShowScreen()
        {
            base.ShowScreen();
            screenTime = Time.time;
            
            StartCoroutine(WaitForTimeRoutine());
        }
        
        private IEnumerator WaitForTimeRoutine()
        {
            yield return new WaitForSeconds(screenTime);
            onTimeComplete?.Invoke();
        }
    }
}