using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class Positions : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI transforms;

    private GameObject rigParent,rig,customTransform,mainCam;
    private void Start()
    {
        rigParent = GameObject.Find("RigParent");
        rig = GameObject.Find("OVRCameraRig");
        customTransform = FindFirstObjectByType<CustomPositionLoader>().gameObject;
        mainCam = Camera.main.gameObject;
        SubsystemManager.GetInstances(subsystems);
    }
    List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();


    public void Recenter()
    {
        foreach (var xr in subsystems)
            xr.TryRecenter();
    }

    public void RecenterOVR()
    {
        OVRManager.display.RecenterPose();
    }
    [ContextMenu("Print Positions")]
    public void PrintPositions()
    {
        
        transforms.text = "";
        transforms.text += "Global Transforms \n";
        transforms.text += "RigParent: " + "Positon: " + rigParent.transform.position + " Rotation: " + rigParent.transform.eulerAngles +"\n";

        transforms.text += "OVRCameraRig: " + "Positon: " + rig.transform.position + " Rotation: " + rig.transform.eulerAngles + "\n";
        transforms.text += "CustomPosition: " + "Positon: " + customTransform.transform.position + " Rotation: " + customTransform.transform.eulerAngles + "\n";
        transforms.text += Camera.main.name + " :Positon :" + mainCam.transform.position + " Rotation: " + mainCam.transform.eulerAngles + "\n";
        
        
        transforms.text += " \nLocal Transforms \n";
        transforms.text += "RigParent: " + "Positon: " + rigParent.transform.localPosition + " Rotation: " + rigParent.transform.localEulerAngles +"\n";

        transforms.text += "OVRCameraRig: " + "Positon: " + rig.transform.localPosition + " Rotation: " + rig.transform.localEulerAngles + "\n";
        transforms.text += "CustomPosition: " + "Positon: " + customTransform.transform.localPosition + " Rotation: " + customTransform.transform.localEulerAngles + "\n";
        transforms.text += Camera.main.name + " :Positon :" + mainCam.transform.localPosition + " Rotation: " + mainCam.transform.localEulerAngles;

    }
}
