using System;
using System.Collections.Generic;
using OLS_HyperCasual;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    [field: SerializeField, HideInInspector] public ZoneEnemyView zoneEnemyView { get; private set; }
    [field: SerializeField] public List<Collider> BulletColliders { get; private set; }
    [field: SerializeField] public List<Collider> WeaponColliders { get; private set; }
    [field: SerializeField] public Animator Animator{ get; private set; }
    [field: SerializeField] public Slider Slider{ get; private set; }
    [field: SerializeField] public MeshRenderer Mesh { get; private set; }
    [field: SerializeField] public int CountHP { get; private set; }

    private void Start()
    {
        var entry = BaseEntryPoint.GetInstance();
        entry.SubscribeOnBaseControllersInit(() =>
        {
            entry.GetController<EnemyController>().AddView(this);
        });
    }

    public void VisualizeDamage(bool state)
    {
        Mesh.materials[0].SetFloat("_MCIALO", state ? 1f : 0f);
    }

    public void SetDeathState()
    {
        
    }

    public void SetActiveCollider(bool b)
    {
        foreach (var collider in BulletColliders)
        {
            collider.enabled = b;
        }
    }
}