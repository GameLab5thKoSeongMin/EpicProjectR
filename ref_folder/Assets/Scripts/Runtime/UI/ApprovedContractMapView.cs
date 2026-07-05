using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ApprovedContractMapPoint
{
    [FormerlySerializedAs("portName")]
    [SerializeField] private string stableCode;
    [SerializeField] private GameObject pointObject;

    public string StableCode => stableCode;
    public GameObject PointObject => pointObject;
}

public class ApprovedContractMapView : MonoBehaviour
{
    [SerializeField] private Transform markerRoot;
    [SerializeField] private GameObject shipMarkerPrefab;
    [SerializeField] private List<ApprovedContractMapPoint> mapPoints =
        new List<ApprovedContractMapPoint>();

    private readonly List<GameObject> spawnedMarkers = new List<GameObject>();

    public void Bind(IReadOnlyList<ApprovedContractRecord> contracts)
    {
        ClearMarkers();

        if (contracts == null || shipMarkerPrefab == null)
            return;

        Transform root = markerRoot != null ? markerRoot : transform;

        for (int i = 0; i < contracts.Count; i++)
        {
            ApprovedContractRecord contract = contracts[i];

            if (contract == null)
                continue;

            if (!TryGetPortPosition(contract.DeparturePortCode, out Vector3 departure) ||
                !TryGetPortPosition(contract.DestinationPortCode, out Vector3 destination))
            {
                continue;
            }

            Vector3 markerPosition = Vector3.Lerp(departure, destination, contract.Progress);

            GameObject instance = Instantiate(shipMarkerPrefab, root);
            spawnedMarkers.Add(instance);

            ApprovedContractShipMarkerView markerView =
                instance.GetComponentInChildren<ApprovedContractShipMarkerView>(true);

            if (markerView != null)
                markerView.Bind(contract, markerPosition);
            else
                SetMarkerPosition(instance, markerPosition);
        }
    }

    public void ClearMarkers()
    {
        for (int i = spawnedMarkers.Count - 1; i >= 0; i--)
        {
            GameObject marker = spawnedMarkers[i];

            if (marker != null)
                Destroy(marker);
        }

        spawnedMarkers.Clear();
    }

    private bool TryGetPortPosition(string stableCode, out Vector3 position)
    {
        position = Vector3.zero;

        if (!string.IsNullOrWhiteSpace(stableCode))
        {
            for (int i = 0; i < mapPoints.Count; i++)
            {
                ApprovedContractMapPoint point = mapPoints[i];

                if (point == null)
                    continue;

                if (IsSameStableCode(point.StableCode, stableCode) &&
                    point.PointObject != null)
                {
                    position = point.PointObject.transform.position;
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsSameStableCode(string left, string right)
    {
        return string.Equals(
            (left ?? string.Empty).Trim(),
            (right ?? string.Empty).Trim(),
            StringComparison.OrdinalIgnoreCase);
    }

    private void SetMarkerPosition(GameObject instance, Vector3 worldPosition)
    {
        if (instance != null)
            instance.transform.position = worldPosition;
    }

}
