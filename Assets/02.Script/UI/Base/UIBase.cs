using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class UIBase : MonoBehaviour
{
    /// <summary>
    /// 해당 캔버스 sortingOrder 프로퍼티
    /// </summary>
    public int SortingOrder
    {
        get => _canvas.sortingOrder;
        set => _canvas.sortingOrder = value;
    }

    bool _inputActionEnable;
    public bool InputActionEnable
    {
        get => _inputActionEnable;
        set
        {
            _inputActionEnable = value;
            OnInputActionEnableChange?.Invoke(value);
        }
    }

    public event Action<bool> OnInputActionEnableChange;

    protected Canvas _canvas;

    /// <summary>
    /// Awake에서 UIManager Dictionary에 등록 / virtual
    /// </summary>
    protected virtual void Awake()
    {
        UIManager.Instance.RegisterUI(this);
        _canvas = GetComponent<Canvas>();
    }

    /// <summary>
    /// UI 활성화 함수
    /// </summary>
    public virtual void Show()
    {
        if (_canvas.enabled) return;

        SoundManager.Instance.PlaySound(SFX.UI_OnOff);
        UIManager.Instance.PushUI(this);
        _canvas.enabled = true;
    }

    /// <summary>
    /// UI 비활성화 함수
    /// </summary>
    public virtual void Hide()
    {
        if (!_canvas.enabled) return;

        SoundManager.Instance.PlaySound(SFX.UI_OnOff);
        UIManager.Instance.PopUI(this);
        _canvas.enabled = false;
    }
}
