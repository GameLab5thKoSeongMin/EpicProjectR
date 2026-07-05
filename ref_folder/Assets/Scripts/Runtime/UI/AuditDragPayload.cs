using System;

[Serializable]
public class AuditDragPayload
{
    public DefinitionBase definition;
    public AdventurerDocument adventurer;

    [NonSerialized] public NewspaperEffectDefinition newspaperEffect;

    public bool HasData
    {
        get
        {
            return definition != null ||
                   adventurer != null ||
                   newspaperEffect != null;
        }
    }
}
