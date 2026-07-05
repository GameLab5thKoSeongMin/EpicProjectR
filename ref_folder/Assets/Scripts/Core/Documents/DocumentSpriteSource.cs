using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DocumentSpriteSource
{
    [SerializeField] private DocumentValueMode mode;
    [SerializeField] private Sprite directValue;
    [SerializeField] private List<Sprite> poolValues = new List<Sprite>();

    public DocumentValueMode Mode => mode;
    public Sprite DirectValue => directValue;
    public IReadOnlyList<Sprite> PoolValues => poolValues;

    public static DocumentSpriteSource Direct(Sprite value)
    {
        DocumentSpriteSource source = new DocumentSpriteSource();
        source.mode = DocumentValueMode.Direct;
        source.directValue = value;
        return source;
    }

    public Sprite Resolve(System.Random random)
    {
        if (mode == DocumentValueMode.Pool && poolValues != null && poolValues.Count > 0)
        {
            int index = random != null
                ? random.Next(0, poolValues.Count)
                : UnityEngine.Random.Range(0, poolValues.Count);

            return poolValues[index];
        }

        return directValue;
    }

    public DocumentSpriteSource CreateResolved(System.Random random)
    {
        return Direct(Resolve(random));
    }
}
