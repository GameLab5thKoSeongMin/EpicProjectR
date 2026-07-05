using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Marine Insurance/Insurance Type")]
public class MarineInsuranceTypeDefinition : DefinitionBase
{
    [SerializeField] private string insuranceTypeName;
    [SerializeField] private List<GameObject> documentPrefabs = new List<GameObject>();
    [SerializeField] private List<MarineInsuranceRejectionReasonDefinition> rejectionReasons =
        new List<MarineInsuranceRejectionReasonDefinition>();

    public string InsuranceTypeName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(insuranceTypeName))
                return insuranceTypeName;

            return DisplayName;
        }
    }

    public IReadOnlyList<GameObject> DocumentPrefabs => documentPrefabs;
    public IReadOnlyList<MarineInsuranceRejectionReasonDefinition> RejectionReasons => rejectionReasons;
}
