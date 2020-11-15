using System;
using JetBrains.Annotations;

[Serializable]
public class ItemData
{
    public string value = default;

    public static implicit operator string([NotNull] ItemData data) => data.value;
}