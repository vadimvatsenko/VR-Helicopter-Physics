using System.Collections.Generic;
using InputSystem;
using UnityEngine;

namespace Rotors
{
    public class RotorBlur : MonoBehaviour, IRotor
    {
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
    
        [Header("Rotor Blur Properties")] 
        [SerializeField] private List<GameObject> rotorsList = new List<GameObject>();
        
        [SerializeField] private GameObject blur;
        [SerializeField] private Material blurMaterial;
        [Space()] 
        [SerializeField] private List<Texture2D> blurTextures = new List<Texture2D>();
        [Space()] 
        [SerializeField] private float maxDps = 2700f;
        
    
        private void OnEnable()
        {
            blurMaterial.SetTexture(BaseMap, blurTextures[0]);
        }

        private void OnDisable()
        {
            blurMaterial.SetTexture(BaseMap, blurTextures[0]);
        }
        
        public void UpdateRotor(float dps, BaseHeliInput input)
        {
            // Шаг 1: Приводим текущую скорость к диапазону от 0.0 до 1.0
            float normalizedDps = Mathf.InverseLerp(0f, maxDps, dps);
            //Debug.Log(normalizedDps);
            // 2. Масштабирует нормализованную скорость под размер коллекции текстур и округляет её вниз 
            // с помощью Mathf.FloorToInt, чтобы получить точный целочисленный индекс. 
            // Метод Mathf.Clamp страхует от выхода за пределы диапазона [0, blurTextures.Count - 1], 
            // предотвращая падение ошибки ArgumentOutOfRangeException.

            int blurTextureIndex = Mathf.FloorToInt(normalizedDps * (blurTextures.Count - 1));
            // чтобы не выйти за диапазон текстур
            blurTextureIndex = Mathf.Clamp(blurTextureIndex, 0, blurTextures.Count - 1);

            Debug.Log(blurTextureIndex);
            
            if (blurMaterial && blurTextures.Count > 0)
            {
                blurMaterial.SetTexture(BaseMap, blurTextures[blurTextureIndex]);
            }
            
            if (blurTextureIndex > 2 && blurTextures.Count > 0)
            {
                
                HandleVisibleBlades(false);
            }
            else
            {
                HandleVisibleBlades(true);
            }
        }
        

        private void HandleVisibleBlades(bool visible)
        {
            foreach (var blade in rotorsList)
            {
                blade.SetActive(visible);
            }
            
            blur.gameObject.SetActive(!visible);
        }
    
    }
}
