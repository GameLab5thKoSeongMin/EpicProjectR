public abstract class TaggedContentDefinition : DefinitionBase
{
    public virtual bool CanBeGivenTo(AdventurerDocument adventurer)
    {
        return adventurer != null;
    }
}
