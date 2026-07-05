public class DialogueRuntimeContext
{
    public UnderwritingCase underwritingCase;
    public AdventurerDocument adventurer;
    public int adventurerIndex = -1;
    public DialogueClaim claim;
    public string responseKey;

    public DialogueRuntimeContext(
        UnderwritingCase underwritingCase,
        AdventurerDocument adventurer,
        int adventurerIndex,
        DialogueClaim claim)
    {
        this.underwritingCase = underwritingCase;
        this.adventurer = adventurer;
        this.adventurerIndex = adventurerIndex;
        this.claim = claim;
    }

    public DialogueRuntimeContext(
        UnderwritingCase underwritingCase,
        AdventurerDocument adventurer,
        int adventurerIndex,
        DialogueClaim claim,
        string responseKey)
        : this(
            underwritingCase,
            adventurer,
            adventurerIndex,
            claim)
    {
        this.responseKey = responseKey;
    }
}
