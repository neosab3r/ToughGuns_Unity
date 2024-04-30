using System.Threading.Tasks;
using OLS_HyperCasual;
using UnityEngine;

public class BulletModel: PoolableModel<BulletView>
{
    public float LifeTime { get; private set; }
    public bool IsUse { get; private set; }
    public int WeaponDamage { get; private set; }
    public Transform CachedTransform { get; private set; }

    private Vector3 direction;

    private static float MoveSpeed = 10f;
    private static float DefaultBulletLifeTime = 5f;
    
    public BulletModel(BulletView view)
    {
        View = view;
        CachedTransform = View.transform;
    }

    public void ShowBullet(int damage, Vector3 position, Quaternion rotation)
    {
        CachedTransform.position = position;
        CachedTransform.rotation = rotation;
        direction = Vector3.zero - CachedTransform.right;
        View.gameObject.SetActive(true);
        WeaponDamage = damage;
        LifeTime = DefaultBulletLifeTime;
        IsUse = true;
    }

    public void HideBullet()
    {
        View.gameObject.SetActive(false);
        IsUse = false;
    }
    
    public void UpdatePosition(float deltaTime)
    {
        CachedTransform.Translate(deltaTime * MoveSpeed * direction);
        LifeTime -= deltaTime;
    }
}