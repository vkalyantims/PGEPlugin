using UnityEngine;

public class PassthroughProjectionSurface : MonoBehaviour
{
    public MeshFilter projectionObject;
    public OVRPassthroughLayer ptLayer;
    // Start is called before the first frame update
    private void Start()
    {
        
        ptLayer.AddSurfaceGeometry(projectionObject.gameObject, true);
        

        if (ptLayer != null)
        {
            // The MeshRenderer component renders the quad as a blue outline
            // we only use this when Passthrough isn't visible
            var quadOutline = projectionObject.GetComponent<MeshRenderer>();
            quadOutline.enabled = false;
        }
    }
}