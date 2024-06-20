using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// UI ������ ���� �Ŵ���
/// </summary>
public class UIManager : SingletonMonoBase<UIManager>
{
    /// <summary>
    /// ��� UI�� ��ϵǾ��ִ� Dictionary
    /// </summary>
    static Dictionary<Type,UIBase> s_UIs = new Dictionary<Type,UIBase>();

    /// <summary>
    /// ���� Ȱ��ȭ���� UI
    /// </summary>
    Stack<UIBase> _ui = new Stack<UIBase>();

    public event Action<bool> onUIChange;

    /// <summary>
    /// UIManager�� UI�� ����ϴ� �Լ�
    /// </summary>
    public void RegisterUI(UIBase ui)
    {
        if(!s_UIs.TryAdd(ui.GetType(),ui))
        {
            Debug.LogWarning("�̹� ��ϵ� UI�Դϴ�.");
        }
    }

    /// <summary>
    /// UIManager�� ��ϵǾ��ִ� ��ųʸ��� Type�� Ű�� UI��ü�� ��ȯ���ִ� �Լ�
    /// </summary>
    public T Get<T>()
        where T : UIBase
    {
        return (T)s_UIs[typeof(T)];
    }

    /// <summary>
    /// UIManager�� ��ϵǾ��ִ� Ȱ��ȭ Stack�� ����ϴ� �Լ�
    /// </summary>
    public void PushUI(UIBase ui)
    {
        if (_ui.Count != 0)
        {
            _ui.Peek().InputActionEnable = false;
        }

        _ui.Push(ui);
        ui.InputActionEnable = true;
        ui.SortingOrder = _ui.Count;
        onUIChange?.Invoke(_ui.Count == 0);
    }

    /// <summary>
    /// UIManager�� ��ϵǾ��ִ� Ȱ��ȭ Stack���� �������ִ� �Լ�
    /// </summary>
    public void PopUI(UIBase ui)
    {
        if (_ui.Peek() != ui)
        {
            Debug.LogWarning("�ش� UI�� �ֻ���� �ƴմϴ�.");
            return;
        }
        _ui.Pop();
        onUIChange?.Invoke(_ui.Count == 0);
        if (_ui.Count != 0)
        {
            _ui.Peek().InputActionEnable = true;
        }
    }
}
