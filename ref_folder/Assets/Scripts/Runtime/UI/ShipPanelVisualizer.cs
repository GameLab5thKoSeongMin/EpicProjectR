using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipPanelVisualizer : MonoBehaviour, IUnderwritingCaseDocumentView
{
    [SerializeField] private string shipSpriteKey = "ship.sprite";
    [SerializeField] private Image shipVisualImage;
    [SerializeField] private string damageLevelKey = "hull.damage.level";
    [SerializeField] private List<GameObject> damageEffectImages = new List<GameObject>();

    void IUnderwritingCaseDocumentView.Bind(UnderwritingCase underwritingCase)
    {
        Bind(underwritingCase);
    }

    public void Bind(UnderwritingCase underwritingCase)
    {
        SetShipSprite(underwritingCase?.GetSprite(shipSpriteKey));

        string damageCode = underwritingCase != null
            ? underwritingCase.GetCode(damageLevelKey)
            : string.Empty;

        ApplyDamageCode(damageCode);
    }

    public void ApplyDamageCode(string damageCode)
    {
        string normalizedCode = string.IsNullOrEmpty(damageCode) ? "none" : damageCode;
        int activeCount = GetActiveDamageImageCount(normalizedCode);

        SetAllDamageImagesActive(false);

        if (activeCount <= 0 || damageEffectImages == null || damageEffectImages.Count == 0)
            return;

        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < damageEffectImages.Count; i++)
        {
            if (damageEffectImages[i] != null)
                availableIndexes.Add(i);
        }

        activeCount = Mathf.Min(activeCount, availableIndexes.Count);

        for (int i = 0; i < activeCount; i++)
        {
            int pickedAvailableIndex = UnityEngine.Random.Range(0, availableIndexes.Count);
            int damageImageIndex = availableIndexes[pickedAvailableIndex];
            availableIndexes.RemoveAt(pickedAvailableIndex);
            damageEffectImages[damageImageIndex].SetActive(true);
        }
    }

    private void SetShipSprite(Sprite sprite)
    {
        if (shipVisualImage == null)
            return;

        shipVisualImage.sprite = sprite;
        shipVisualImage.enabled = sprite != null;
        shipVisualImage.preserveAspect = true;
    }

    private void SetAllDamageImagesActive(bool isActive)
    {
        if (damageEffectImages == null)
            return;

        for (int i = 0; i < damageEffectImages.Count; i++)
        {
            if (damageEffectImages[i] != null)
                damageEffectImages[i].SetActive(isActive);
        }
    }

    private int GetActiveDamageImageCount(string damageCode)
    {
        if (MatchesDamageCode(damageCode, "upper", "high"))
            return 3;

        if (MatchesDamageCode(damageCode, "middle", "medium"))
            return 2;

        if (MatchesDamageCode(damageCode, "lower", "low"))
            return 1;

        return 0;
    }

    private bool MatchesDamageCode(string value, string firstCode, string secondCode)
    {
        return string.Equals(value, firstCode, StringComparison.OrdinalIgnoreCase) ||
               string.Equals(value, secondCode, StringComparison.OrdinalIgnoreCase);
    }
}
