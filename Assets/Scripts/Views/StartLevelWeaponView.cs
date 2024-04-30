using System;
using UnityEngine;

public class StartLevelWeaponView : MonoBehaviour
{
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public Transform TransformPointToShoot { get; private set; }
    [field: SerializeField] public int WeaponDamage { get; private set; }
    [field: SerializeField] public ERateFire RateFire { get; private set; }
    [field: SerializeField] public EEffectShootWeaponType EffectShoot { get; private set; }
}