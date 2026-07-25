using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UISystem : MonoBehaviour
    {
        [Header("Fader Properties")]
        [SerializeField] private RawImage imageFader;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeOutDuration;
        
        private UIScreen[] _screens;
        private UIScreen _previousScreen;
        private UIScreen _currentScreen;

        public Action OnSwitchScreen;

        private void Start()
        {
            _screens = GetComponentsInChildren<UIScreen>(true);
            _previousScreen = _screens[0];
            
            imageFader.gameObject.SetActive(true);
            FadeIn();
        }

        private void FadeIn() => imageFader.CrossFadeAlpha(0f, fadeInDuration, false);
        
        private void FadeOut() => imageFader.CrossFadeAlpha(1f, fadeOutDuration, false);
        
        public void SwitchScreen(UIScreen newScreen)
        {
            _previousScreen = _currentScreen;
            _currentScreen = newScreen;
            _currentScreen.gameObject.SetActive(true);
            
            OnSwitchScreen?.Invoke();
        }
        
        public void GoToPreviousScreen() => SwitchScreen(_previousScreen);

        public void LoadScene(int index) => StartCoroutine(LoadSceneRoutine(index));

        private IEnumerator LoadSceneRoutine(int sceneIndex) 
        {
            yield return SceneManager.LoadSceneAsync(sceneIndex);
        }
    }
}
