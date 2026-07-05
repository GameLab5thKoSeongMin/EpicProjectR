using System;
using UnityEngine;

[Serializable]
public class CaseDataEntry
{
    [SerializeField] public string key;
    [SerializeField] public DocumentTextSource value = new DocumentTextSource();
    [SerializeField] public DocumentTextSource codeValue = new DocumentTextSource();
}
