using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class TreeGazeTracker : MonoBehaviour
{
    [Tooltip("Put Tree Transform here")]
    public Transform treeTransform;

    [Tooltip("Degrees per segment (1 -> 360 segments)")]
    [Range(1, 45)]
    public int segmentSize = 1;

    [Tooltip("How often (seconds) we sample the user’s view")]
    public float sampleInterval = 0.1f;

    [Tooltip("Half-angle of the cone in front of the user to count as ‘looking’")]
    [Range(0, 180)]
    public float maxViewAngle = 45f;

    // internal
    private bool[] seenSegments;
    private int totalSegments;
    private Coroutine sampleCoroutine;

    void Awake()
    {
        if (treeTransform == null)
            Debug.LogError("TreeReviewTracker: Tree Transform not assigned!", this);

        totalSegments = Mathf.CeilToInt(360f / segmentSize);
        seenSegments = new bool[totalSegments];
    }

    void OnEnable()
    {
        sampleCoroutine = StartCoroutine(SampleLoop());
    }

    void OnDisable()
    {
        if (sampleCoroutine != null)
            StopCoroutine(sampleCoroutine);
    }

    private IEnumerator SampleLoop()
    {
        while (true)
        {
            RecordCurrentSegment();
            yield return new WaitForSeconds(sampleInterval);
        }
    }

    private void RecordCurrentSegment()
    {
        if (treeTransform == null) return;

        // 1) direction to tree (ignore Y)
        Vector3 toTree = treeTransform.position - transform.position;
        toTree.y = 0f;
        if (toTree.sqrMagnitude < Mathf.Epsilon) return;

        // 2) view‐cone test
        if (Vector3.Angle(transform.forward, toTree) > maxViewAngle)
            return;

        // 3) compute yaw 0–360
        float rawAngle = Vector3.SignedAngle(Vector3.forward, toTree, Vector3.up);
        if (rawAngle < 0f) rawAngle += 360f;

        // 4) mark segment
        int seg = Mathf.FloorToInt(rawAngle / segmentSize);
        seenSegments[seg] = true;
    }

    /// <summary>
    /// Returns 0–100% of the 360° that have been looked at.
    /// </summary>
    [ContextMenu("Print area covered")]
    public float GetReviewedPercent()
    {
        int count = 0;
        foreach (bool saw in seenSegments)
            if (saw) count++;

        float coveredDeg = count * segmentSize;
        Debug.Log(Mathf.Min(coveredDeg / 360f * 100f, 100f).ToString() + " degrees");
        return Mathf.Min(coveredDeg / 360f * 100f, 100f);
    }



#if UNITY_EDITOR
    // visualize in SceneView
    void OnDrawGizmosSelected()
    {
        if (treeTransform == null || seenSegments == null) return;

        float radius = 1f;
        for (int i = 0; i < seenSegments.Length; i++)
        {
            Gizmos.color = seenSegments[i] ? Color.green : Color.red;
            float ang = (i + 0.5f) * segmentSize * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(ang), 0f, Mathf.Cos(ang));
            Gizmos.DrawLine(treeTransform.position, treeTransform.position + dir * radius);
        }
    }
#endif
}
