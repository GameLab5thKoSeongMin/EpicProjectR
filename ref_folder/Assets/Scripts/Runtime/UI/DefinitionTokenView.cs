using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefinitionTokenView : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private AuditDragSource dragSource;

    public void Bind(DefinitionBase definition)
    {
        Bind(definition, definition != null ? definition.DisplayName : string.Empty);
    }

    public void Bind(DefinitionBase definition, string label)
    {
        BindVisual(definition, label);
    }

    public void BindCaseDefinition(DefinitionBase definition)
    {
        BindCaseDefinition(
            definition,
            definition != null ? definition.DisplayName : string.Empty);
    }

    public void BindCaseDefinition(
        DefinitionBase definition,
        string label)
    {
        BindVisual(definition, label);

        AuditDragSource source = GetDragSource();
        if (source != null)
            source.BindDefinition(definition);
    }

    private void BindVisual(DefinitionBase definition, string label)
    {
        if (labelText != null)
            labelText.text = label;

        if (iconImage == null)
            return;

        Sprite sprite = definition != null ? definition.IconSprite : null;
        iconImage.sprite = sprite;
        iconImage.enabled = sprite != null;
    }

    private AuditDragSource GetDragSource()
    {
        if (dragSource == null)
            dragSource = GetComponent<AuditDragSource>();

        return dragSource;
    }
}
