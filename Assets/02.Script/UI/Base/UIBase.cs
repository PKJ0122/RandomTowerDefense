using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class UIBase : MonoBehaviour
{
    /// <summary>
    /// �ش� ĵ���� sortingOrder ������Ƽ
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
    /// Awake���� UIManager Dictionary�� ��� / virtual
    /// </summary>
    protected virtual void Awake()
    {
        UIManager.Instance.RegisterUI(this);
        _canvas = GetComponent<Canvas>();
    }

    /// <summary>
    /// UI Ȱ��ȭ �Լ�
    /// </summary>
    public virtual void Show()
    {
        if (_canvas.enabled) return;

        SoundManager.Instance.PlaySound(SFX.UI_OnOff);
        UIManager.Instance.PushUI(this);
        _canvas.enabled = true;
    }

    /// <summary>
    /// UI ��Ȱ��ȭ �Լ�
    /// </summary>
    public virtual void Hide()
    {
        if (!_canvas.enabled) return;

        SoundManager.Instance.PlaySound(SFX.UI_OnOff);
        UIManager.Instance.PopUI(this);
        _canvas.enabled = false;
    }
}
