using System;
using UnityEngine;

[Serializable]
public class CaseDataSpriteEntry
{
    [SerializeField] public string key;
    [SerializeField] public DocumentSpriteSource value = new DocumentSpriteSource();
}
