using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DocumentTextSource
{
    [SerializeField] private DocumentValueMode mode;

    [TextArea]
    [SerializeField] private string directValue;

    [SerializeField] private List<string> poolValues = new List<string>();

    public DocumentValueMode Mode => mode;
    public string DirectValue => directValue;
    public IReadOnlyList<string> PoolValues => poolValues;

    public static DocumentTextSource Direct(string value)
    {
        DocumentTextSource source = new DocumentTextSource();
        source.mode = DocumentValueMode.Direct;
        source.directValue = value ?? string.Empty;
        return source;
    }

    public string Resolve(System.Random random)
    {
        if (mode == DocumentValueMode.Pool && poolValues != null && poolValues.Count > 0)
        {
            int index = random != null
                ? random.Next(0, poolValues.Count)
                : UnityEngine.Random.Range(0, poolValues.Count);

            return poolValues[index] ?? string.Empty;
        }

        return directValue ?? string.Empty;
    }

    public DocumentTextSource CreateResolved(System.Random random)
    {
        return Direct(Resolve(random));
    }
}
