using System.Collections.Generic;
using System.Linq;
using InputSystem;
using Rotors;
using UnityEngine;

// клас відповідає тільки за візуальне оновлення лопатей гелікоптера
// у даному випадку оберт навколо своєї вісі
namespace Controllers
{
    public class RotorController : MonoBehaviour
    {
        private List<IRotor> _rotors;

        private void Start()
        {
            _rotors = GetComponentsInChildren<IRotor>().ToList();
        }
        public void UpdateRotor(BaseHeliInput input, float rpm)
        {
            _rotors.ForEach(r => r.UpdateRotor(rpm, input));
        }
    }
}
