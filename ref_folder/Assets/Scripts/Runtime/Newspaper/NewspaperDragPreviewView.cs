using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewspaperDragPreviewView : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text labelText;

    public void Bind(Sprite iconSprite, string displayName)
    {
        if (iconImage != null)
        {
            iconImage.sprite = iconSprite;
            iconImage.enabled = iconSprite != null;
            iconImage.preserveAspect = true;
            iconImage.raycastTarget = false;
        }

        if (labelText != null)
        {
            labelText.text = displayName;
            labelText.raycastTarget = false;
        }
    }
}
