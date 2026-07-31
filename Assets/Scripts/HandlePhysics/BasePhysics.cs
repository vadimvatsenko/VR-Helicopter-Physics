using Engines;
using InputSystem;
using UnityEngine;

namespace HandlePhysics
{
    public class BasePhysics : MonoBehaviour
    {
        [Header("Lift Properties")] 
        // maxLiftForce це коєфіціент тяги двигуна, зазвичай це параметр двигуна в документації до гелікоптера
        // У реальній авіації є поняття Thrust-to-Weight Ratio TWR — відношення тяги до ваги
        [SerializeField] private float maxLiftForce = 3.0f;
        // максимальна висота польоту.
        [SerializeField] private float maxAltitude = 200f;
        // тертя повітря, коф втрат
        [SerializeField] private float aerodynamicEfficiencyExponent = 0.66f;

        [Header("Tail Rotor Properties")] 
        [SerializeField] private float tailForce = 2f;
        
        [Header("Cyclic Properties")]
        [SerializeField] private float cyclingForce = 2f;
        [SerializeField] private float cyclicForceMultiplier = 1000f;

        [Header("Auto Level Properties")] 
        [SerializeField] private float autoLevelForce = 2f;

        // змінні для розрахунку нахилу та стабілізації
        private Vector3 _flatForward;
        private float _forwardDot;
        private Vector3 _flatRight;
        private float _rightDot;
        
        // це перший двигун, я передбачаю, що він один !!!
        private MainHeliEngine _heliEngine;
        private Rigidbody _rb;
        private BaseHeliInput _input;
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _input = GetComponent<BaseHeliInput>();
            _heliEngine = GetComponentInChildren<MainHeliEngine>();
        }
        
        public void UpdateAllPhysics()
        {
            CalculateAngles();
            AutoLevel();
            HandleLift();
            HandleCyclic();
            HandlePedals();
        }
        
        private void HandleLift() => AdvicePhysicsLift();
        
        private void AdvicePhysicsLift()
        {
            // 9.81 * 500 = 4905Н - базова сила тяжіння (F = m * g) (4905 Ньютонів)
            float gravityForce = Physics.gravity.magnitude * _rb.mass;

            // 4905 + (3 * 500) = 6405H - максимально можлива підйомна сила двигуна
            // 3 - з документації до хелікоптера
            float maxPossibleForce = gravityForce + (maxLiftForce * _rb.mass);

            // Нормалізація обертів двигуна від 0 до 1, щоб знати потужність від 0 до 100%
            // Приклад: 2000 RPM / 2700 MaxRPM = 0.74 (74% обертів)
            float mormalizedRpm = Mathf.Clamp01(_heliEngine.CurrentRpm / _heliEngine.MaxRpm);

            // Ефект щільності повітря за статтею Загордана.
            float currentAltitude = transform.position.y;
            // Приклад: 1 - 150 (поточна висота) / 200 (макс. висота) = 0.25 (коефіцієнт щільності)
            float airDensityFactor = Mathf.Clamp01(1f - (currentAltitude / maxAltitude));

            // Перемножуємо два чистих коефіцієнти: оберти двигуна та крок гвинта (ввід)
            // Приклад: 0.74 (оберти) * 0.50 (клавіатура на половину) = 0.37
            float rawPowerCoeff = mormalizedRpm * _input.CollectiveInput;

            // Нелінійний ККД лопатей: підносимо загальну потужність до степеня 2/3 (0.66)
            // Математика для 0.37: 0.37 * 0.37 = 0.1369 -> кубічний корінь з 0.1369 = 0.515
            float aerodynamicEfficiency = Mathf.Pow(rawPowerCoeff, aerodynamicEfficiencyExponent);

            // Фінальний вектор
            // Напрямок: завжди локальний верх вертольота (transform.up)
            // Величина: Максимальна сила * Ефективність лопатей * Щільність повітря
            // Приклад: 6405Н * 0.515 * 0.25 = 824.64 Ньютонів підйомної сили
            float finalLiftMagnitude = maxPossibleForce * aerodynamicEfficiency * airDensityFactor;
            Vector3 liftForceVector = transform.up * finalLiftMagnitude;
            _rb.AddForce(liftForceVector, ForceMode.Force);
        }
        
        private void HandleCyclic()
        {
            // AddRelativeTorque
            // обертає об'єкт навколо його власних, локальних осей.
            // Тобто осей, які прив'язані до корпусу вертольота і крутяться разом із ним.
            float cyclicZForce = -_input.CyclicInput.x * cyclingForce;
            _rb.AddRelativeTorque(Vector3.forward * cyclicZForce, ForceMode.Acceleration);
            
            float cyclicXForce = _input.CyclicInput.y * cyclingForce;
            _rb.AddRelativeTorque(Vector3.right * cyclicXForce, ForceMode.Acceleration);
            
            Vector3 forwardVector = _flatForward * _forwardDot;
            Vector3 rightVector = _flatRight * _rightDot;
            Vector3 finalCyclicDirection 
                = Vector3.ClampMagnitude(forwardVector + rightVector, 1f) * (cyclingForce * cyclicForceMultiplier) ;
            _rb.AddForce(finalCyclicDirection, ForceMode.Force);
        }
        
        private void HandlePedals()
        {
            // AddTorque обертання навколо осі, у даному випадку вісь Y 
            // Режим Acceleration каже фізичному рушію: "Мені байдуже, скільки важить цей вертоліт.
            // Просто почни крутити його навколо своєї осі із заданим прискоренням"
            _rb.AddTorque(transform.up * (_input.PedalInput * tailForce), ForceMode.Acceleration);
        }

        private void CalculateAngles()
        {
            _flatForward = transform.forward;
            _flatForward.y = 0;
            _flatForward.Normalize();
            
            _flatRight = transform.right;
            _flatRight.y = 0;
            _flatRight.Normalize();
            
            _forwardDot = Vector3.Dot(transform.up, _flatForward);
            _rightDot = Vector3.Dot(transform.up, _flatRight);
        }

        private void AutoLevel()
        {
            float rightForce = -_forwardDot * autoLevelForce;
            float forwardForce = _rightDot * autoLevelForce;
            
            _rb.AddRelativeTorque(Vector3.right * rightForce, ForceMode.Acceleration);
            _rb.AddRelativeTorque(Vector3.forward * forwardForce, ForceMode.Acceleration);
        }
    }
}