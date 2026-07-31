using Engines;
using Rotors;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private MainHeliEngine mainHeliEngine;
        [SerializeField] private MainRotor mainRotor;
        [SerializeField] private TextMeshProUGUI textHp;
        [SerializeField] private TextMeshProUGUI textRpm;
        [SerializeField] private TextMeshProUGUI textBladesAngle;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button restartButton;

        private void OnEnable()
        {
            mainHeliEngine.OnChangeCurrentHp += OnTextHpChanged;
            mainHeliEngine.OnChangeCurrentRpm += OnTextRpmChanged;
            mainRotor.OnRotate += OnTextBladesAngleChanged;
            exitButton.onClick.AddListener(OnExitButton);
            restartButton.onClick.AddListener(OnRestartButton);
        }

        private void OnDisable()
        {
            mainHeliEngine.OnChangeCurrentHp -= OnTextHpChanged;
            mainHeliEngine.OnChangeCurrentRpm -= OnTextRpmChanged;
            mainRotor.OnRotate -= OnTextBladesAngleChanged;
            exitButton.onClick.RemoveListener(OnExitButton);
            restartButton.onClick.RemoveListener(OnRestartButton);
        }

        private void OnTextHpChanged(float text) => textHp.text = $"HP : {text.ToString()}";
        private void OnTextRpmChanged(float text) => textRpm.text = $"RPM: {text.ToString()}";

        private void OnTextBladesAngleChanged(float text) =>
            textBladesAngle.text = $"Blades Angle: {text.ToString()}";
        
        private void OnExitButton() => Application.Quit();
        private void OnRestartButton() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}