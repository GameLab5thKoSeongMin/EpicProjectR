using UnityEngine;

public abstract class NewspaperEffectDefinition : ScriptableObject
{
    [SerializeField] private string effectId;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite iconSprite;

    public string EffectId => effectId;
    public string DisplayName => displayName;
    public Sprite IconSprite => iconSprite;
}
