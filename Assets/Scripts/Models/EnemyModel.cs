using System.Collections.Generic;
using OLS_HyperCasual;
using UnityEngine;

public class EnemyModel : BaseModel<EnemyView>
{
    private int countHp;
    private EEnemyType enemyType;


    public bool IsDeath { get; private set; }

    private const double damageBodyRatio = 1;
    private const double damageHeadRatio = 2;

    public EnemyModel(EnemyView view)
    {
        IsDeath = false;
        View = view;
        countHp = view.CountHP;
    }

    public bool IsContains(Vector3 position, out string nameBody)
    {
        foreach (var bound in View.BulletColliders)
        {
            var isContains = bound.bounds.Contains(position);
            if (isContains)
            {
                nameBody = "mixamorig:Head";
                return true;
            }
        }

        nameBody = "";
        return false;
    }

    public bool IsContains(Vector3 position)
    {
        foreach (var bound in View.WeaponColliders)
        {
            if (bound.bounds.Contains(position))
            {
                return true;
            }
        }

        return false;
    }

    public void ReduceHP(string nameBody, int damage)
    {
        var result = ResultDamage(nameBody, damage);
        countHp -= result;

        if (countHp <= 0)
        {
            countHp = 0;
            IsDeath = true;
        }
    }

    public void ClearData()
    {
        View.SetActiveCollider(false);
        View.SetDeathState();
    }

    private int ResultDamage(string nameBody, int damage)
    {
        switch (nameBody)
        {
            case "mixamorig:Body":
            {
                damage = (int) (damage * damageBodyRatio);
                break;
            }
            case "mixamorig:Spine1":
            {
                damage = (int) (damage * damageHeadRatio);
                break;
            }
        }

        return damage;
    }
}