using System;
using OLS_HyperCasual;
using UnityEngine;

public class StartLevelWeaponModel : PoolableModel<StartLevelWeaponView>
{
    private readonly Rigidbody cachedRigidbody;
    private const float speed = 4f;
    
    public EWeaponsType WeaponType;
    public ERateFire RateFire;
    public EEffectShootWeaponType EffectShootWeaponType { get; private set; }
    public int WeaponDamage { get; private set; }
    public bool isUse { get; private set; }
    public int CellIndex;
    public Transform CachedTransform { get; private set; }
    public Transform CachedPointToShootTransform { get; private set; }
    
    public StartLevelWeaponModel(StartLevelWeaponView view)
    {
        View = view;
        isUse = true;
        WeaponDamage = View.WeaponDamage;
        RateFire = View.RateFire;
        EffectShootWeaponType = View.EffectShoot;
        CachedTransform = View.transform;
        CachedPointToShootTransform = View.TransformPointToShoot;
        cachedRigidbody = View.Rigidbody;
        cachedRigidbody.maxAngularVelocity = 7f;
        cachedRigidbody.maxLinearVelocity = 4f;
    }

    public void DeathWeapon()
    {
        isUse = false;
        View.gameObject.SetActive(false);
    }

    public void SetKinematic()
    {
        cachedRigidbody.isKinematic = false;
        cachedRigidbody.AddTorque(0, 0, 4f, ForceMode.Impulse);
    }

    public void UpdatePosition()
    {
        if (isUse == false)
        {
            return;
        }
        
        cachedRigidbody.velocity = Vector3.zero;
        cachedRigidbody.AddRelativeForce(-4f, 0, 0, ForceMode.VelocityChange);

        if (CachedTransform.right.x > 0)
        {
            cachedRigidbody.AddTorque(0, 0, speed, ForceMode.Impulse);
        }
        else
        {
            cachedRigidbody.AddTorque(0, 0, -speed, ForceMode.Impulse);
        }
    }

    public void SetBorderPosition(Vector3 position, bool isRight = true)
    {
        if (isRight)
        {
            CachedTransform.position = new Vector3(position.x, CachedTransform.position.y, CachedTransform.position.z);
        }
        else
        {
            CachedTransform.position = new Vector3(CachedTransform.position.x, position.y, CachedTransform.position.z);
        }
    }

    public void Shoot()
    {

    }
}