using UnityEngine;

[DisallowMultipleComponent]
public class AuditDragRoot : MonoBehaviour
{
    [SerializeField] private Transform rootOverride;

    public Transform Root => rootOverride != null ? rootOverride : transform;
}
