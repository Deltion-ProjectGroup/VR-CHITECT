using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour
{
    public GameObject leftHandGO, rightHandGO;
    public SteamVR_Action_Boolean interactButton;
    public SteamVR_Action_Boolean teleportButton;
    public SteamVR_Action_Boolean snapButton;
    LineRenderer pointer;
    bool teleporting;
    public GameObject lastHoveredSnapObject;
    public Transform vertIndicator;
    public LayerMask snapMask;
    public static bool canInteract;
    // Start is called before the first frame update
    void Start()
    {
        pointer = rightHandGO.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (snapButton.GetState(InputMan.leftHand) && Placer.placer.trackingObj == null)
        {
            RaycastHit hit;
            Ray ray = new Ray(rightHandGO.transform.position, rightHandGO.transform.forward);
            if (Physics.Raycast(ray, out hit, 1000f, snapMask, QueryTriggerInteraction.Ignore))
            {
                lastHoveredSnapObject = hit.transform.gameObject;
                Vector3 nearestVert = Vector3.zero;
                float nearestVertDistance = Mathf.Infinity;
                foreach (Transform child in hit.transform)
                {
                    foreach (Vector3 vert in child.GetComponent<MeshFilter>().mesh.vertices)
                    {
                        if (Vector3.Distance(hit.point, hit.transform.TransformPoint(vert)) < nearestVertDistance)
                        {
                            nearestVert = vert;
                            nearestVertDistance = Vector3.Distance(hit.point, hit.transform.TransformPoint(vert));
                        }
                    }
                }
                if (nearestVert != Vector3.zero)
                {
                    vertIndicator.position = hit.transform.TransformPoint(nearestVert);
                    Placer.placer.offset = vertIndicator.position - hit.transform.position;
                    //to - from
                }
            }
        }
        else
        {
            if (snapButton.GetStateUp(InputMan.leftHand))
            {
                lastHoveredSnapObject = null;
                Placer.placer.offset = Vector3.zero;
                Placer.placer.vertSnapping = false;
            }
        }



        RaycastHit hitPoint;
        if (Physics.Raycast(rightHandGO.transform.position, rightHandGO.transform.forward, out hitPoint))
        {
            pointer.SetPosition(0, rightHandGO.transform.position);
            pointer.SetPosition(1, hitPoint.point);
            if (interactButton.GetStateDown(InputMan.rightHand) && canInteract)
            {
                if(lastHoveredSnapObject != null)
                {
                    Placer.placer.vertSnapping = true;
                    Placer.placer.SetTrackingObject(lastHoveredSnapObject);
                }
                else
                {
                    if (hitPoint.transform.tag == "Interactable")
                    {
                        hitPoint.transform.GetComponent<Interactable>().Interact();
                    }
                }
            }
            if (teleportButton.GetStateUp(InputMan.leftHand))
            {
                if(hitPoint.transform.tag == "Ground")
                {
                    if (!teleporting)
                    {
                        StartCoroutine(Teleport(hitPoint.point));
                    }
                }
            }
        }
        else
        {
            if (interactButton.GetStateDown(InputMan.rightHand) && lastHoveredSnapObject != null)
            {
                Placer.placer.vertSnapping = true;
                Placer.placer.SetTrackingObject(lastHoveredSnapObject);
            }
            pointer.SetPosition(0, rightHandGO.transform.position);
            pointer.SetPosition(1, rightHandGO.transform.forward);
        }
    }
    IEnumerator Teleport(Vector3 newPosition)
    {
        teleporting = true;
        UIManager.uiManager.ScreenFade(true);
        yield return new WaitForSeconds(UIManager.uiManager.fadeAnimation.clip.length);
        transform.position = newPosition;
        UIManager.uiManager.ScreenFade(false);
        yield return new WaitForSeconds(UIManager.uiManager.fadeAnimation.clip.length);
        teleporting = false;
    }
}
