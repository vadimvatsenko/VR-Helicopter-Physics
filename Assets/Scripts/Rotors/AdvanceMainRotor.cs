using System;
using InputSystem;
using UnityEngine;

namespace Rotors
{
    public class AdvanceMainRotor : MonoBehaviour, IRotor
    {
        [SerializeField] protected Transform lFirstRotor;
        [SerializeField] protected Transform lSecondRotor;
        [SerializeField] protected Transform rFirstRotor;
        [SerializeField] protected Transform rSecondRotor;

        [SerializeField] protected float maxPitch = 35f;

        public Action<float> OnRotate;

        public void UpdateRotor(float dps, BaseHeliInput input)
        {
            float pitch = input.CollectiveInput * maxPitch;
            OnRotate?.Invoke(pitch);
            // оберт 
            transform.Rotate(Vector3.up, dps);
            if (lFirstRotor && rFirstRotor && lSecondRotor && rSecondRotor)
            {
                lFirstRotor.localRotation = Quaternion.Euler(0f, 0f, pitch);
                lSecondRotor.localRotation = Quaternion.Euler(0f, 0f, pitch);
                rFirstRotor.localRotation = Quaternion.Euler(0f, 0f, -pitch);
                rSecondRotor.localRotation = Quaternion.Euler(0f, 0f, -pitch);
            }
        }
    }
}
