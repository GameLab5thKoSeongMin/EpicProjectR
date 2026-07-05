using System;
using System.Collections.Generic;

[Serializable]
public class AdventurerDocument
{
    public AdventurerIdentityDocument identityDocument = new AdventurerIdentityDocument();

    public string adventurerName
    {
        get => Identity.adventurerName;
        set => Identity.adventurerName = value;
    }

    public int age
    {
        get => Identity.age;
        set => Identity.age = value;
    }

    public UnityEngine.Sprite portraitSprite
    {
        get => Identity.portraitSprite;
        set => Identity.portraitSprite = value;
    }

    private AdventurerIdentityDocument Identity
    {
        get
        {
            if (identityDocument == null)
                identityDocument = new AdventurerIdentityDocument();

            return identityDocument;
        }
    }

    public IEnumerable<TaggedContentDefinition> EnumerateContents()
    {
        yield break;
    }

    public bool CanReceiveContent(TaggedContentDefinition content)
    {
        return false;
    }

    public bool AddContent(TaggedContentDefinition content)
    {
        return false;
    }
}
