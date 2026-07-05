using System;
using UnityEngine;

[Serializable]
public class ApprovedContractRecord
{
    [SerializeField] private string contractorName;
    [SerializeField] private string departurePort;
    [SerializeField] private string departurePortCode;
    [SerializeField] private string destinationPort;
    [SerializeField] private string destinationPortCode;
    [SerializeField] private int premium;
    [SerializeField] private int compensationAmount;
    [SerializeField] private int startTurn;
    [SerializeField] private int durationTurns = 1;
    [SerializeField] private int elapsedTurns;

    public string ContractorName => contractorName;
    public string DeparturePort => departurePort;
    public string DeparturePortCode => departurePortCode;
    public string DestinationPort => destinationPort;
    public string DestinationPortCode => destinationPortCode;
    public int Premium => premium;
    public int CompensationAmount => compensationAmount;
    public int StartTurn => startTurn;
    public int DurationTurns => durationTurns;
    public int ElapsedTurns => elapsedTurns;
    public bool IsArrived => elapsedTurns >= durationTurns;

    public float Progress
    {
        get
        {
            if (durationTurns <= 0)
                return 1f;

            return Mathf.Clamp01(elapsedTurns / (float)durationTurns);
        }
    }

    public string StatusText => IsArrived ? "도착" : "항해 중";

    public ApprovedContractRecord(
        string contractorName,
        string departurePort,
        string departurePortCode,
        string destinationPort,
        string destinationPortCode,
        int premium,
        int compensationAmount,
        int startTurn,
        int durationTurns)
    {
        this.contractorName = contractorName;
        this.departurePort = departurePort;
        this.departurePortCode = departurePortCode;
        this.destinationPort = destinationPort;
        this.destinationPortCode = destinationPortCode;
        this.premium = Mathf.Max(0, premium);
        this.compensationAmount = Mathf.Max(0, compensationAmount);
        this.startTurn = Mathf.Max(0, startTurn);
        this.durationTurns = Mathf.Max(1, durationTurns);
        elapsedTurns = 0;
    }

    public void AdvanceTurns(int turns)
    {
        if (turns <= 0 || IsArrived)
            return;

        elapsedTurns = Mathf.Min(durationTurns, elapsedTurns + turns);
    }
}
