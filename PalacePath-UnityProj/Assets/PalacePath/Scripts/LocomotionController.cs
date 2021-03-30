using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Note to self: Ctrl K C - comment out 
//              Ctrl K U - uncomment

public class LocomotionController : MonoBehaviour
{
    public XRController leftTeleportRay; //references to our teleportation rays, which we will assign
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton; //a button to turn on teleporting, set this to the same button you are using for teleporting
    public float activationThreshold = 0.1f; // below the threshold we use to activate the ray (which we've set to 0.1f)

    // public XRRayInteractor leftInteractorRay;
    // public XRRayInteractor rightInteractorRay;

    // public bool EnableLeftTeleport { get; set; } = true;
    // public bool EnableRightTeleport { get; set; } = true;

    // Update is called once per frame
    void Update()
    {
        // Vector3 pos = new Vector3();
        // Vector3 norm = new Vector3();
        // int index = 0;
        // bool validTarget = false;

        if(leftTeleportRay)
        {
            // bool isLeftInteractorRayHovering = leftInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            // leftTeleportRay.gameObject.SetActive(EnableLeftTeleport && CheckIfActivated(leftTeleportRay) && !isLeftInteractorRayHovering);
            leftTeleportRay.gameObject.SetActive(CheckIfActivated(leftTeleportRay));
        }

        if (rightTeleportRay)
        {
            // bool isRightInteractorRayHovering = rightInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            // rightTeleportRay.gameObject.SetActive(EnableRightTeleport && CheckIfActivated(rightTeleportRay) && !isRightInteractorRayHovering);
            rightTeleportRay.gameObject.SetActive(CheckIfActivated(rightTeleportRay));
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
