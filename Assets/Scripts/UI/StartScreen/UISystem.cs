using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UISystem : MonoBehaviour
    {
        [Header("Screens")] 
        [SerializeField] private UIScreen loginScreen;
        [SerializeField] private UIScreen registerScreen;
        
        [Header("Fader Properties")]
        [SerializeField] private RawImage imageFader;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeOutDuration;
        
        [Header("Buttons")]
        [SerializeField] private Button toRegisterBtn;
        [SerializeField] private Button toLoginBtn;
        [SerializeField] private Button backBtn;
        
        private UIScreen _previousScreen;
        private UIScreen _currentScreen;
        
        private void Start()
        {
            SwitchScreen(loginScreen);
            imageFader.gameObject.SetActive(true);
            FadeIn();
        }

        private void OnEnable()
        {
            toRegisterBtn.onClick.AddListener(() => SwitchScreen(registerScreen));
            backBtn.onClick.AddListener(GoToPreviousScreen);
        }

        private void OnDisable()
        {
            toRegisterBtn.onClick.RemoveAllListeners();
            backBtn.onClick.RemoveAllListeners();
        }

        private void FadeIn() => imageFader.CrossFadeAlpha(0f, fadeInDuration, false);
        
        private void FadeOut() => imageFader.CrossFadeAlpha(1f, fadeOutDuration, false);
        
        public void SwitchScreen(UIScreen newScreen)
        {
            _previousScreen = _currentScreen;
            _currentScreen = newScreen;
            
            if (_previousScreen != null)
            {
                //_previousScreen.gameObject.SetActive(false);
                _previousScreen.CloseScreen();
            }
            
            _currentScreen.gameObject.SetActive(true);
            _currentScreen.ShowScreen();
            
            
        }
        
        public void GoToPreviousScreen() => SwitchScreen(_previousScreen);

        public void LoadScene(int index) => StartCoroutine(LoadSceneRoutine(index));

        private IEnumerator LoadSceneRoutine(int sceneIndex) 
        {
            yield return SceneManager.LoadSceneAsync(sceneIndex);
        }
    }
}
