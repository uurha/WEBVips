using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleClick : MonoBehaviour, IPointerClickHandler
{
    [Space(6f)]
    [SerializeField] private UnityEvent onSettingTrue = default;

    private Toggle _toggle = default;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_toggle.isOn)
        {
            onSettingTrue?.Invoke();
        }
    }
}