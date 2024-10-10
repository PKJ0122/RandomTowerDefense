using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : PoolObject
{
    Canvas _canvas;
    Slider _hpbar;

    Boss _boss;
    Vector3 _bossHeight;


    protected override void Awake()
    {
        base.Awake();
        _hpbar = transform.Find("Slider - BossHp").GetComponent<Slider>();
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;
        _canvas.enabled = false;
    }

    private void Update()
    {
        if (!_canvas.enabled || _boss == null) return;

        transform.position = _boss.transform.position + _bossHeight;
    }

    public void Show(Boss boss)
    {
        _canvas.enabled = true;
        _hpbar.maxValue = boss.Hp;
        _hpbar.value = boss.Hp;

        _boss = boss;
        _bossHeight = new Vector3(0, _boss.GetComponent<Collider>().bounds.size.y + 0.1f);
        boss.OnHpChange += v => HpBarSet(v);
        boss.OnRelasePool += () =>
        {
            boss.OnHpChange -= v => HpBarSet(v);
            Hide();
        };
    }

    void HpBarSet(float hp)
    {
        _hpbar.value = hp;
    }

    public void Hide()
    {
        _canvas.enabled = false;
        _boss = null;
    }
}