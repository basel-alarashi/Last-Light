using UnityEngine;

namespace LastLight.Player
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "LastLight/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [Header("Weapon Stats")]
        public string weaponName = "Fists";
        public float damage = 15f;
        public float attackRange = 2f;
        public float attackCooldown = 0.8f;

        [Header("Knockback")]
        public float knockbackForce = 3f;
    }
}