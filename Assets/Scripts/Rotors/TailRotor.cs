using InputSystem;
using UnityEngine;

namespace Rotors
{
    public class TailRotor : MonoBehaviour, IRotor
    {
        [Header("Tail Rotor Properties")] 
        [SerializeField] private float tailRotorSpeed = 1.5f;
        [SerializeField] private Transform lRotor;
        [SerializeField] private Transform rRotor;
        // максимальний кут леза
        [SerializeField] private float maxPitch = 15f;
        public void UpdateRotor(float dps, BaseHeliInput input)
        {
            transform.Rotate(Vector3.right, dps * tailRotorSpeed, Space.Self);
            
            if (lRotor && rRotor)
            {
                lRotor.localRotation = Quaternion.Euler(0f, input.PedalInput * maxPitch, 0f );
                rRotor.localRotation = Quaternion.Euler(0f, -input.PedalInput * maxPitch, 0f);
            }
        }
    }
}