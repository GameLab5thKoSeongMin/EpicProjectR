using System.Collections.Generic;
using UnityEngine;

public class UnderwritingCaseApplicationView : MonoBehaviour
{
    [SerializeField] private Transform documentRoot;
    [SerializeField] private Transform additionalSubmissionDocumentRoot;

    private readonly List<GameObject> additionalSubmissionDocuments =
        new List<GameObject>();

    public void Bind(UnderwritingCase underwritingCase)
    {
        ClearAdditionalSubmissionDocuments();
        ClearChildren(documentRoot);

        if (underwritingCase?.insuranceType == null)
            return;

        IReadOnlyList<GameObject> prefabs = underwritingCase.insuranceType.DocumentPrefabs;

        if (prefabs == null)
            return;

        for (int i = 0; i < prefabs.Count; i++)
        {
            GameObject prefab = prefabs[i];

            if (prefab == null)
                continue;

            MarineInsuranceDocumentMetadata metadata =
                prefab.GetComponentInChildren<MarineInsuranceDocumentMetadata>(true);

            if (metadata != null && !metadata.IsSubmitted(underwritingCase))
                continue;

            GameObject instance = Instantiate(prefab, documentRoot);
            IUnderwritingCaseDocumentView view = instance.GetComponent<IUnderwritingCaseDocumentView>();
            view?.Bind(underwritingCase);
        }
    }

    public GameObject SubmitAdditionalDocument(GameObject prefab)
    {
        Transform root = additionalSubmissionDocumentRoot != null
            ? additionalSubmissionDocumentRoot
            : documentRoot;

        if (prefab == null || root == null)
            return null;

        GameObject instance = Instantiate(prefab, root);
        additionalSubmissionDocuments.Add(instance);
        return instance;
    }

    private void ClearAdditionalSubmissionDocuments()
    {
        for (int i = additionalSubmissionDocuments.Count - 1; i >= 0; i--)
        {
            GameObject document = additionalSubmissionDocuments[i];

            if (document != null)
                Destroy(document);
        }

        additionalSubmissionDocuments.Clear();
    }

    private void ClearChildren(Transform root)
    {
        if (root == null)
            return;

        for (int i = root.childCount - 1; i >= 0; i--)
            Destroy(root.GetChild(i).gameObject);
    }
}
