using OLS_HyperCasual;
using UnityEngine;
using UnityEngine.UI;

public class HPBarSnake : MonoBehaviour
{
    public int HP;
    public Slider Slider;

    private void Start()
    {
        Slider.value = 0;
        Slider.maxValue = HP;
        var entry = BaseEntryPoint.GetInstance();
        entry.SubscribeOnBaseControllersInit(() =>
        {
            entry.GetController<EnemyController>().AddSnakeHPBar(this);
        });
    }

    public void Damage(int weaponDamage)
    {
        HP -= weaponDamage;
        Slider.value += weaponDamage;
    }
}