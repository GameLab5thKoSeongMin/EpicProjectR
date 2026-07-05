using UnityEngine;

public abstract class MarineInsuranceCaseDefinition : ScriptableObject
{
    public abstract UnderwritingCase CreateCase(int seed);
}
