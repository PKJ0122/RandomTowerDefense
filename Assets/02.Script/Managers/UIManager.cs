using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// UI 관리를 위한 매니저
/// </summary>
public class UIManager : SingletonMonoBase<UIManager>
{
    /// <summary>
    /// 모든 UI가 등록되어있는 Dictionary
    /// </summary>
    static Dictionary<Type,UIBase> s_UIs = new Dictionary<Type,UIBase>();

    /// <summary>
    /// 현재 활성화중인 UI
    /// </summary>
    Stack<UIBase> _ui = new Stack<UIBase>();

    public event Action<bool> onUIChange;

    /// <summary>
    /// UIManager에 UI를 등록하는 함수
    /// </summary>
    public void RegisterUI(UIBase ui)
    {
        if(!s_UIs.TryAdd(ui.GetType(),ui))
        {
            Debug.LogWarning("이미 등록된 UI입니다.");
        }
    }

    /// <summary>
    /// UIManager에 등록되어있는 딕셔너리에 Type를 키로 UI객체를 반환해주는 함수
    /// </summary>
    public T Get<T>()
        where T : UIBase
    {
        return (T)s_UIs[typeof(T)];
    }

    /// <summary>
    /// UIManager에 등록되어있는 활성화 Stack에 등록하는 함수
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
    /// UIManager에 등록되어있는 활성화 Stack에서 제거해주는 함수
    /// </summary>
    public void PopUI(UIBase ui)
    {
        if (_ui.Peek() != ui)
        {
            Debug.LogWarning("해당 UI가 최상단이 아닙니다.");
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
