using UnityEngine;
using LastLight.Systems;
using LastLight.Core;

namespace LastLight.Player
{
    /// <summary>
    /// Controls the player's torch light source.
    /// Manages fuel, toggling and light properties.
    /// </summary>
    public class PlayerLight : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LightData lightData;

        [Header("References")]
        [SerializeField] private Light torchLight;

        private bool _isLightOn = false;

        public bool IsLightOn => _isLightOn;
        public LightData Data => lightData;

        private void Awake()
        {
            ValidateReferences();
            InitializeLight();
        }

        private void Update()
        {
            if (lightData == null || torchLight == null) return;

            HandleToggleInput();
            HandleFuelDrain();
        }

        private void HandleToggleInput()
        {
            if (Input.GetKeyDown(KeyCode.F))
                ToggleLight();
        }

        private void ToggleLight()
        {
            if (!_isLightOn && !lightData.HasFuel)
            {
                Debug.Log("[PlayerLight] No fuel to ignite torch.");
                return;
            }

            _isLightOn = !_isLightOn;
            torchLight.enabled = _isLightOn;

            if (_isLightOn)
            {
                Debug.Log("[PlayerLight] Firing OnLightIgnited event.");
                GameEvents.TriggerLightIgnited();
            }
            else
            {
                Debug.Log("[PlayerLight] Firing OnLightExtinguished event.");
                GameEvents.TriggerLightExtinguished();
            }

            Debug.Log($"[PlayerLight] Torch {(_isLightOn ? "ON" : "OFF")}");
        }

        private void HandleFuelDrain()
        {
            if (!_isLightOn) return;

            lightData.DrainFuel(lightData.fuelDrainRate * Time.deltaTime);

            if (!lightData.HasFuel)
                ExtinguishLight();
        }

        private void ExtinguishLight()
        {
            _isLightOn = false;
            torchLight.enabled = false;
            Debug.Log("[PlayerLight] Torch ran out of fuel.");
        }

        private void InitializeLight()
        {
            if (torchLight == null) return;

            torchLight.type = LightType.Point;
            torchLight.range = lightData.lightRadius;
            torchLight.intensity = lightData.lightIntensity;
            torchLight.color = lightData.lightColor;
            torchLight.enabled = false;
        }

        private void ValidateReferences()
        {
            if (lightData == null)
                Debug.LogError("[PlayerLight] LightData is not assigned.");
            if (torchLight == null)
                Debug.LogError("[PlayerLight] TorchLight is not assigned.");
        }

        /// <summary>
        /// Called from inventory/crafting when adding fuel.
        /// </summary>
        public void AddFuel(float amount)
        {
            lightData.AddFuel(amount);
            Debug.Log($"[PlayerLight] Fuel added. Current: {lightData.currentFuel:F1}");
        }
    }
}