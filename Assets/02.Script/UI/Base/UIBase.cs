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
            onInputActionEnableChange?.Invoke(value);
        }
    }

    public event Action<bool> onInputActionEnableChange;

    Canvas _canvas;

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

        UIManager.Instance.PushUI(this);
        _canvas.enabled = true;
    }

    /// <summary>
    /// UI ��Ȱ��ȭ �Լ�
    /// </summary>
    public virtual void Hide()
    {
        if (!_canvas.enabled) return;

        UIManager.Instance.PopUI(this);
        _canvas.enabled = false;
    }
}
