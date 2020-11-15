using System;
using TMPro;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected int length;
    [SerializeField] protected TMP_Text[] displayLabels;
    [SerializeField] protected ItemData[] itemData;

    public TMP_Text[] DisplayLabels => displayLabels;

    protected enum Elements
    {
    }

    protected void UpdateDisplay()
    {
        foreach (int i in Enum.GetValues(typeof(Elements)))
        {
            displayLabels[i].text = itemData[i];
        }
    }

    protected void Validate(Type type)
    {
        if (Enum.GetValues(type).Length < length - 1)
        {
            length = Enum.GetValues(type).Length;

            throw new
                Exception($"Length have to be less {nameof(type)} length than {Enum.GetValues(type).Length}");
        }

        if (displayLabels.Length != length)
            Array.Resize(ref displayLabels, length);

        if (itemData.Length != length)
            Array.Resize(ref itemData, length);
    }
}