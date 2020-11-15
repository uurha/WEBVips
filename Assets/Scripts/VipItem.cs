using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VipItem : Item
{
    private new enum Elements {
        None      = -1,
        Number    = 0,
        Nickname  = 1,
        StartDate = 2,
        DueDate   = 3,
    }

    private void OnValidate()
    {
        base.Validate(typeof(Elements));
    }
}