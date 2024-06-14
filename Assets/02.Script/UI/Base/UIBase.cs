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
        UIManager.Instance.PushUI(this);
        _canvas.enabled = true;
    }

    /// <summary>
    /// UI ��Ȱ��ȭ �Լ�
    /// </summary>
    public virtual void Hide()
    {
        UIManager.Instance.PopUI(this);
        _canvas.enabled = false;
    }
}
