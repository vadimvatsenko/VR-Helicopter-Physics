using System.Collections.Generic;
using Engines;
using HandlePhysics;
using InputSystem;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody), typeof(BaseHeliInput))]
    public class HeliController : BaseRbController
    {
        [SerializeField] private List<MainHeliEngine> engines;
        private BaseHeliInput _baseHeliInput;
        private RotorController _rotorController;
        private BasePhysics _basePhysics;

        private void Start()
        {
            _baseHeliInput = GetComponent<BaseHeliInput>();
            _basePhysics = GetComponent<BasePhysics>();
            _rotorController = GetComponentInChildren<RotorController>();
        }
        
        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleRotors();
            HandleUpdatePhysics();
        }
        
        private void HandleRotors()
        {
            foreach (var engine in engines)
            {
                _rotorController.UpdateRotor(_baseHeliInput, engine.CurrentRpm);
            }
        }

        protected virtual void HandleEngines()
        {
            engines.ForEach(e => e.UpdateEngine(_baseHeliInput));
        }

        protected virtual void HandleUpdatePhysics() => _basePhysics.UpdateAllPhysics();
    }
}