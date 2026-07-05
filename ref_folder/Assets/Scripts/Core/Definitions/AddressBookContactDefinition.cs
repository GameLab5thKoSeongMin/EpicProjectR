using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Underwriting/Content/Address Book Contact")]
public class AddressBookContactDefinition : DefinitionBase
{
    [SerializeField] private string tabName;
    [TextArea]
    [SerializeField] private List<string> firstConnectionLines = new List<string>();
    [SerializeField] private string responseKey;

    public string TabName =>
        !string.IsNullOrWhiteSpace(tabName) ? tabName : DisplayName;

    public IReadOnlyList<string> FirstConnectionLines => firstConnectionLines;
    public string ResponseKey => responseKey;

    public int ConversationKey => GetStableConversationKey(Id);

    private static int GetStableConversationKey(string value)
    {
        unchecked
        {
            const int offset = (int)2166136261;
            const int prime = 16777619;
            int hash = offset;
            string source = !string.IsNullOrWhiteSpace(value) ? value : "address-book-contact";

            for (int i = 0; i < source.Length; i++)
                hash = (hash ^ source[i]) * prime;

            if (hash >= 0)
                hash = -hash - 1000;

            return hash;
        }
    }
}
