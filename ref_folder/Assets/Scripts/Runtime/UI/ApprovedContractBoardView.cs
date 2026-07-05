using System.Collections.Generic;
using UnityEngine;

public class ApprovedContractBoardView : MonoBehaviour
{
    [SerializeField] private ApprovedContractTracker tracker;
    [SerializeField] private ApprovedContractMapView mapView;
    [SerializeField] private ApprovedContractListView listView;

    private ApprovedContractTracker registeredTracker;

    private void OnEnable()
    {
        RegisterTracker();
        Refresh();
    }

    private void OnDisable()
    {
        UnregisterTracker();
    }

    public void Refresh()
    {
        IReadOnlyList<ApprovedContractRecord> contracts =
            tracker != null ? tracker.Contracts : null;

        if (mapView != null)
            mapView.Bind(contracts);

        if (listView != null)
            listView.Bind(contracts);
    }

    private void OnContractsChanged(IReadOnlyList<ApprovedContractRecord> contracts)
    {
        if (mapView != null)
            mapView.Bind(contracts);

        if (listView != null)
            listView.Bind(contracts);
    }

    private void RegisterTracker()
    {
        if (tracker == null)
            tracker = Object.FindFirstObjectByType<ApprovedContractTracker>();

        if (registeredTracker == tracker)
            return;

        UnregisterTracker();
        registeredTracker = tracker;

        if (registeredTracker != null)
            registeredTracker.ContractsChanged += OnContractsChanged;
    }

    private void UnregisterTracker()
    {
        if (registeredTracker == null)
            return;

        registeredTracker.ContractsChanged -= OnContractsChanged;
        registeredTracker = null;
    }
}
