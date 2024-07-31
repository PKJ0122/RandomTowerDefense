using UnityEngine;
using UnityEngine.UI;

public class ESCUI : UIBase
{
    Button _ok;
    Button _close;


    protected override void Awake()
    {
        base.Awake();
        _ok = transform.Find("Panel/Image/Button - Ok").GetComponent<Button>();
        _close = transform.Find("Panel/Image/Button - Close").GetComponent<Button>();
        _ok.onClick.AddListener(() => Application.Quit());
        _close.onClick.AddListener(Hide);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_canvas.enabled)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }

    public override void Show()
    {
        base.Show();
        SortingOrder = 9999;
    }
}
