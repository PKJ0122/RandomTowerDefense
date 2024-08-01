using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// UI ������ ���� �Ŵ���
/// </summary>
public class UIManager : SingletonMonoBase<UIManager>
{
    /// <summary>
    /// ��� UI�� ��ϵǾ��ִ� Dictionary
    /// </summary>
    Dictionary<Type, UIBase> _uis = new Dictionary<Type, UIBase>();

    /// <summary>
    /// ���� Ȱ��ȭ���� UI
    /// </summary>
    Stack<UIBase> _ui = new Stack<UIBase>();

    public event Action<bool> OnUIChange;


    protected override void Awake()
    {
        base.Awake();
        
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "Game") return; 

        GameManager.Instance.OnGameEnd += v =>
        {
            for (int i = _ui.Count-1; i >= 0; i--)
            {
                _ui.Peek().Hide();
            }
        };
    }


    /// <summary>
    /// UIManager�� UI�� ����ϴ� �Լ�
    /// </summary>
    public void RegisterUI(UIBase ui)
    {
        if (!_uis.TryAdd(ui.GetType(), ui))
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
        return (T)_uis[typeof(T)];
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
        OnUIChange?.Invoke(_ui.Count == 0);
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
        OnUIChange?.Invoke(_ui.Count == 0);
        if (_ui.Count != 0)
        {
            _ui.Peek().InputActionEnable = true;
        }
    }

    public bool UIPeekCheck(UIBase ui)
    {
        if (_ui.Count == 0)
        {
            return false;
        }

        return _ui.Peek() == ui;
    }
}
