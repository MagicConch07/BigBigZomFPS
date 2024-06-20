using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindowEnum
{
    LevelUp
}

public class UIManager : MonoSingleton<UIManager>
{
    public Dictionary<WindowEnum, IWindowPanel> panelDictionary;

    [SerializeField] private GameObject _death;
    [SerializeField] private GameObject _clear;
    [SerializeField] private GameObject _settings;
    [SerializeField] private InputReader _inputReader;

    //[SerializeField] private Transform _canvasTrm;

    private void Awake()
    {
        /* panelDictionary = new Dictionary<WindowEnum, IWindowPanel>();
        foreach (WindowEnum windowEnum in Enum.GetValues(typeof(WindowEnum)))
        {
            IWindowPanel panel = _canvasTrm
                .GetComponent($"{windowEnum.ToString()}Panel") as IWindowPanel;
            panelDictionary.Add(windowEnum, panel);
        } */
    }

    void OnEnable()
    {
        _inputReader.OnSettingsEvent += HandleSettingsEvent;
        _death.SetActive(false);
        _clear.SetActive(false);
        _settings.SetActive(false);
    }

    void OnDisable()
    {
        _inputReader.OnSettingsEvent -= HandleSettingsEvent;
    }

    private void HandleSettingsEvent(bool isRun)
    {
        if (isRun)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _settings.SetActive(true);
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _settings.SetActive(false);
        }
    }

    public void DeadUI()
    {
        _death.SetActive(true);
    }

    public void ClaerUI()
    {
        _clear.SetActive(true);
    }

    public void Open(WindowEnum target)
    {
        if (panelDictionary.TryGetValue(target, out IWindowPanel panel))
        {
            panel.Open();
        }
    }

    public void Close(WindowEnum target)
    {
        if (panelDictionary.TryGetValue(target, out IWindowPanel panel))
        {
            panel.Close();
        }
    }

    public void UISelect()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OutUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Test()
    {
        Debug.Log($"Test");
    }
}
