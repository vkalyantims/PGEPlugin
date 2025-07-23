using UnityEngine;

public class BinocularTool : BaseTool
{
    Binocular binocular;

    [SerializeField]
    private RenderTexture renderTexture;
    protected override void Awake()
    {
        base.Awake();
        binocular = FindFirstObjectByType<Binocular>();
    }
    protected override void OnActivated()
    {
        binocular.Enable();
        //Camera.main.targetTexture = renderTexture;
        //Camera.main.gameObject.AddComponent<FieldOfView>();
    }

    protected override void OnDeactivated()
    {
        binocular.Disable();
        //Destroy(Camera.main.gameObject.GetComponent<FieldOfView>());
        //Camera.main.targetTexture = null;

    }
}
