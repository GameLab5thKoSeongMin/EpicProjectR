using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Underwriting/Content/Supply")]
public class SupplyDefinition : TaggedContentDefinition
{
    public override bool CanBeGivenTo(AdventurerDocument adventurer)
    {
        return adventurer != null && adventurer.CanReceiveContent(this);
    }
}
