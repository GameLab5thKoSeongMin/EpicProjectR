using UnityEngine;

public abstract class DefinitionBase : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite iconSprite;

    [TextArea]
    [SerializeField] private string description;

    public string Id
    {
        get
        {
            if (string.IsNullOrWhiteSpace(id))
                return name;

            return id;
        }
    }

    public string DisplayName
    {
        get
        {
            if (string.IsNullOrWhiteSpace(displayName))
                return name;

            return displayName;
        }
    }

    public string Description => description;
    public Sprite IconSprite => iconSprite;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id))
            id = name;

        if (string.IsNullOrWhiteSpace(displayName))
            displayName = name;
    }
#endif
}
