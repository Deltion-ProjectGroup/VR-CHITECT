using System.Collections;
using UnityEngine;
using Valve.VR;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject leftHandGO, rightHandGO;
    [SerializeField] SteamVR_Action_Boolean interactButton;
    [SerializeField] SteamVR_Action_Boolean teleportButton;
    [SerializeField] SteamVR_Action_Boolean snapButton;
    [SerializeField] SteamVR_Action_Boolean duplicateButton;
    LineRenderer pointer;
    bool teleporting;
    GameObject lastHoveredSnapObject;
    Transform vertIndicator;
    [SerializeField] GameObject indicatorGO;
    public static Vector3 nearestVert;
    [SerializeField] LayerMask snapMask;
    public static bool canInteract = true;
    // Start is called before the first frame update
    void Start()
    {
        pointer = rightHandGO.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        VertSnapping();


        RaycastHit hitPoint;
        if (Physics.Raycast(rightHandGO.transform.position, rightHandGO.transform.forward, out hitPoint))
        {
            pointer.SetPosition(0, rightHandGO.transform.position);
            pointer.SetPosition(1, hitPoint.point);
            Interaction(hitPoint);
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
    void Interaction(RaycastHit hitPoint)
    {
        if (duplicateButton.GetStateDown(InputMan.leftHand) && Placer.placer.canSetObject && canInteract)
        {
            if(hitPoint.transform.tag == "Interactable")
            {
                GameObject clonedObject = Instantiate(hitPoint.transform.gameObject.GetAbsoluteParent());
                clonedObject.GetComponent<PlacedObject>().objectPlacedOn = null;
                clonedObject.GetComponent<PlacedObject>().objectsPlacedOnTop = new System.Collections.Generic.List<GameObject>();
                Placer.placer.SetTrackingObject(clonedObject);
            }
        }

        if (interactButton.GetStateDown(InputMan.rightHand) && canInteract)
        {
            if (lastHoveredSnapObject != null)
            {
                Placer.placer.vertSnapping = true;
                lastHoveredSnapObject.GetComponent<PlacedObject>().Interact();
            }
            else
            {
                if (hitPoint.transform.tag == "Interactable")
                {
                    hitPoint.transform.gameObject.GetAbsoluteParent().GetComponent<Interactable>().Interact();
                }
            }
        }
    }
    void VertSnapping()
    {

        if (snapButton.GetState(InputMan.leftHand) && canInteract)
        {
            if (snapButton.GetStateDown(InputMan.leftHand))
            {
                vertIndicator = Instantiate(indicatorGO).transform;
            }
            RaycastHit hit;
            Ray ray = new Ray(rightHandGO.transform.position, rightHandGO.transform.forward);
            if (Physics.Raycast(ray, out hit, 1000f, snapMask, QueryTriggerInteraction.Ignore))
            {
                lastHoveredSnapObject = hit.transform.gameObject.GetAbsoluteParent();
                nearestVert = Vector3.zero;
                float nearestVertDistance = Mathf.Infinity;
                if(hit.transform.childCount > 0)
                {
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
                }
                else
                {
                    foreach (Vector3 vert in hit.transform.GetComponent<MeshFilter>().mesh.vertices)
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
                    Placer.placer.offset = Placer.CalculateOffset(hit.transform.TransformPoint(nearestVert), hit.transform.position);
                    vertIndicator.position = hit.transform.TransformPoint(nearestVert);
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
                if (vertIndicator != null)
                {
                    Destroy(vertIndicator.gameObject);
                }
            }
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
