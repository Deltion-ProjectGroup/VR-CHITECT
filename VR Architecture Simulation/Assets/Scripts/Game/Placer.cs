using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Placer : MonoBehaviour
{
    public static Placer placer;
    public GameObject trackingObj;
    [SerializeField] LayerMask placementMask;
    public Vector3 offset;
    [SerializeField] Transform hand;
    bool snappingPosition;
    bool snappingRotation;
    public bool vertSnapping;
    [SerializeField] SteamVR_Action_Boolean positionSnapButton, placeButton;
    [SerializeField] SteamVR_Action_Vector2 rotateButton;
    [SerializeField] SteamVR_Action_Boolean rotatePress, rotationSnapButton;
    [SerializeField] SteamVR_Action_Boolean snapButton;
    public int rotateTurnAmount;
    public bool canPlace;
    [SerializeField] Color canPlaceColor, cannotPlaceColor;
    [SerializeField] Material placementMaterial;
    [HideInInspector] public PlacementPart[] ogPartData;
    public bool canSetObject = true;

    [Range(0.1f, 1)]
    public float gritTileSize;
    public sbyte divisionAmount;
    public GameObject tile;
    public Vector2 tileSize;
    public Vector2 gridTileSize;
    public List<GameObject> allTiles = new List<GameObject>();
    GameObject[] extraTrackingObjects;
    public List<PlacementPart> extraTrackingObjectsOGData = new List<PlacementPart>();


    [SerializeField] SteamVR_Input_Sources placeSource, destroySource, rotateSource, rotateSnapSource, positionSnapSource;

    [SerializeField] AudioSource placerAudioSource, mainAudioSource;
    [SerializeField] AudioClip destroySound, errorPlaceSound, placeSound, rotateSound;

    public static bool isEnabled = true;
    //-----------------------------------------------------------------------

    public delegate void DelegateVoid();
    public static DelegateVoid OnRotate; //
    public static DelegateVoid OnSetObject; //
    public static DelegateVoid OnToggleRotationSnap; //
    public static DelegateVoid OnTogglePositionSnap; //
    public static DelegateVoid OnDestroyObject; //
    public static DelegateVoid OnPlaceObject; //


    public float rotateModifier;
    // Start is called before the first frame update
    void Awake()
    {
        placer = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (trackingObj)
        {
            if (placeButton.GetStateDown(InputMan.GetHand(placeSource)) && canPlace)
            {
                if (isEnabled)
                {
                    StartCoroutine(PlaceTrackingObject());
                }
            }
            else
            {
                if (isEnabled)
                {
                    if (placeButton.GetStateDown(InputMan.GetHand(destroySource)))
                    {
                        DestroyPlacingObject();
                        return;
                    }
                    if (placeButton.GetStateDown(InputMan.GetHand(placeSource)))
                    {
                        mainAudioSource.clip = errorPlaceSound;
                        mainAudioSource.Play();
                    }
                    //Snapping
                    ToggleGridSnap();
                    ToggleRotationSnap();

                    //PlacementCheck
                    Rotate();
                }
                RaycastHit hit;
                Ray ray = new Ray(hand.position, hand.forward);
                if (Physics.Raycast(ray, out hit, 1000, placementMask))
                {
                    ChangePosition(hit);
                }
                CheckPlacable(hit);
            }
        }
    }
    public void DestroyPlacingObject()
    {
        Player.canInteract = true;
        offset = Vector3.zero;
        //UIManager.uiManager.ToggleMenu(UIManager.uiManager.properties);
        Destroy(trackingObj);
        trackingObj = null;
        canSetObject = true;
        mainAudioSource.clip = destroySound;
        mainAudioSource.Play();
        if(OnDestroyObject != null)
        {
            OnDestroyObject();
        }
    }
    public static Vector3 CalculateOffset(Vector3 vertWorldPosition, Vector3 ownerPosition)
    {
        return vertWorldPosition - ownerPosition;
    }
    void ChangePosition(RaycastHit hitData)
    {
        Vector3 hitPoint = hitData.point;
        if (vertSnapping)
        {

            Vector3 nearestVert = Vector3.zero;
            float nearestVertDistance = Mathf.Infinity;
            if (hitData.transform.gameObject.GetComponent<MeshFilter>())
            {
                foreach(Vector3 vert in hitData.transform.gameObject.GetComponent<MeshFilter>().mesh.vertices)
                {
                    if(Vector3.Distance(hitData.point, hitData.transform.TransformPoint(vert)) < nearestVertDistance)
                    {
                        nearestVert = vert;
                        nearestVertDistance = Vector3.Distance(hitData.point, hitData.transform.TransformPoint(vert));
                    }
                }
            }
            if (nearestVert != Vector3.zero)
            {
                hitPoint = hitData.transform.TransformPoint(nearestVert);
            }
            hitPoint -= offset;
        }
        else
        {
            print(hitData.transform.gameObject.GetAbsoluteParent().gameObject);
            if (snappingPosition && hitData.transform.gameObject.GetComponentInParent<PlacedObject>().objectType != ObjectTypes.Wall)
            {
                hitPoint.x = Mathf.RoundToInt(hitPoint.x / gritTileSize) * gritTileSize;
                hitPoint.z = Mathf.RoundToInt(hitPoint.z / gritTileSize) * gritTileSize;
            }
        }
        trackingObj.transform.position = hitPoint;
    }
    void Rotate()
    {
        float rotateAmount = rotateButton.GetAxis(InputMan.GetHand(rotateSource)).x;
        if (rotateAmount != 0)
        {
            if (snappingRotation)
            {
                if (rotatePress.GetStateDown(InputMan.GetHand(rotateSource)))
                {
                    placerAudioSource.clip = rotateSound;
                    placerAudioSource.Play();
                    rotateAmount = Mathf.RoundToInt(rotateAmount);
                    rotateAmount *= rotateTurnAmount;
                    trackingObj.transform.Rotate(new Vector3(0, rotateAmount, 0));
                    if (OnRotate != null)
                    {
                        OnRotate();
                    }
                }
            }
            else
            {
                Debug.Log("ROTATE");
                trackingObj.transform.Rotate(new Vector3(0, rotateAmount * rotateModifier * Time.deltaTime, 0));
                if (OnRotate != null)
                {
                    OnRotate();
                }
            }
            if (vertSnapping)
            {
                offset = CalculateOffset(trackingObj.transform.TransformPoint(Player.nearestVert), trackingObj.transform.position);
            }
        }
    }
    public void SetTrackingObject(GameObject thisObject)
    {
        //thisObject = thisObject.GetAbsoluteParent();
        if(trackingObj == null && canSetObject)
        {
            extraTrackingObjects = thisObject.GetComponent<PlacedObject>().objectsPlacedOnTop.ToArray();
            canSetObject = false;
            trackingObj = thisObject;
            trackingObj.GetComponent<PlacedObject>().OnPickUp();
            List<PlacementPart> allObjectMaterials = new List<PlacementPart>();
            if(thisObject.transform.childCount > 0)
            {
                GameObject[] allChildren = thisObject.GetAllChildren();
                for (int i = 0; i < allChildren.Length; i++)
                {
                    GameObject thisChild = allChildren[i];
                    if (thisChild.GetComponent<PartData>())
                    {
                        thisChild.GetComponent<Collider>().enabled = false;
                        allObjectMaterials.Add(new PlacementPart(thisChild));
                        allObjectMaterials[allObjectMaterials.Count - 1].part.GetComponent<MeshRenderer>().material = placementMaterial;

                    }
                }
            }
            else
            {
                thisObject.GetComponent<Collider>().enabled = false;
                allObjectMaterials.Add(new PlacementPart(thisObject));
                allObjectMaterials[allObjectMaterials.Count - 1].part.GetComponent<MeshRenderer>().material = placementMaterial;
            }
            foreach(GameObject extraObject in extraTrackingObjects)
            {
                extraObject.transform.SetParent(trackingObj.transform);
                if(extraObject.transform.childCount > 0)
                {
                    GameObject[] allChilds = extraObject.GetAllChildren();
                    foreach(GameObject child in allChilds)
                    {
                        if (child.GetComponent<MeshRenderer>())
                        {
                            extraTrackingObjectsOGData.Add(new PlacementPart(child));
                            child.GetComponent<MeshRenderer>().material = placementMaterial;
                        }
                    }
                }
                else
                {
                    extraTrackingObjectsOGData.Add(new PlacementPart(extraObject));
                    extraObject.GetComponent<MeshRenderer>().material = placementMaterial;
                }
            }
            ogPartData = allObjectMaterials.ToArray();
            Player.canInteract = false;
            //UIManager.uiManager.properties.GetComponent<PropertiesMenu>().targetRN = trackingObj;
            //UIManager.uiManager.ToggleMenu(UIManager.uiManager.properties);
            //UIManager.uiManager.properties.GetComponent<PropertiesMenu>().Initialize(trackingObj);
            if(OnSetObject != null)
            {
                OnSetObject();
            }
        }
    }
    IEnumerator PlaceTrackingObject()
    {
        print("PLACED");
        offset = Vector3.zero;
        GameObject oldTracker = trackingObj;
        foreach(PlacementPart partData in ogPartData)
        {
            partData.ResetMaterial();
        }
        foreach(PlacementPart partData in extraTrackingObjectsOGData)
        {
            partData.ResetMaterial();
        }
        extraTrackingObjectsOGData = new List<PlacementPart>();
        foreach(GameObject extraObject in extraTrackingObjects)
        {
            extraObject.transform.parent = null;
        }
        //UIManager.uiManager.ToggleMenu(UIManager.uiManager.properties);
        trackingObj.GetComponent<PlacedObject>().OnPlace();
        trackingObj = null;
        mainAudioSource.clip = placeSound;
        mainAudioSource.Play();
        if(OnPlaceObject != null)
        {
            OnPlaceObject();
        }
        yield return null;
        foreach (PlacementPart partData in ogPartData)
        {
            partData.part.GetComponent<Collider>().enabled = true;
        }
        Player.canInteract = true;
        canSetObject = true;
    }
    void ToggleGridSnap()
    {
        if (positionSnapButton.GetStateDown(InputMan.GetHand(positionSnapSource)))
        {
            snappingPosition = !snappingPosition;
            if (OnTogglePositionSnap != null)
            {
                OnTogglePositionSnap();
            }
            //ToggleGrid(true);
        }
    }
    void ToggleRotationSnap()
    {
        if (rotationSnapButton.GetStateDown(InputMan.GetHand(rotateSnapSource)))
        {
            snappingRotation = snappingRotation.ToggleBool();
            if (snappingRotation)
            {
                Vector3 snappedRotation = trackingObj.transform.eulerAngles;
                snappedRotation.y = Mathf.RoundToInt(snappedRotation.y / rotateTurnAmount) * rotateTurnAmount;
                trackingObj.transform.eulerAngles = snappedRotation;
                if (OnToggleRotationSnap != null)
                {
                    OnToggleRotationSnap();
                }
            }
        }
        else
        {
            if (Input.GetButtonUp("RotationSnap"))
            {
                snappingRotation = snappingRotation.ToggleBool();
            }
        }
    }
    public void ChangeTileDivision(float newAmount)
    {
        gritTileSize = newAmount;
        gritTileSize = Mathf.Clamp(gritTileSize, 0.1f, 1);
        //CalculateTilePositions(GameObject.FindGameObjectsWithTag("Ground"));
    }
    public void ChangeSnapRotation(int newAmount)
    {
        rotateTurnAmount = newAmount;
    }
    bool CheckPosition(GameObject hitObject)
    {
        if(hitObject != null)
        {
            for (int i = 0; i < trackingObj.GetComponent<PlacedObject>().requiredObjectType.Length; i++)
            {
                if (trackingObj.GetComponent<PlacedObject>().requiredObjectType[i] == hitObject.GetComponent<PlacedObject>().objectType)
                {
                    return true;
                }
            }
        }
        return false;
    }
    void CheckPlacable(RaycastHit hitData)
    {
        placementMaterial.SetColor("_BaseColor", cannotPlaceColor);
        canPlace = false;
        Collider[] collisions = Physics.OverlapBox(trackingObj.transform.position + trackingObj.GetComponent<BoxCollider>().center, trackingObj.GetComponent<BoxCollider>().size / 2, trackingObj.transform.rotation);
        foreach(Collider col in collisions)
        {
            if(col.gameObject.GetAbsoluteParent() != trackingObj)
            {
                print("COLLIDED WITH" + col.name);
                return;
            }
        }
        if(hitData.transform != null)
        {
            if (vertSnapping)
            {
                if(Physics.Raycast(trackingObj.transform.position, Vector3.down, out hitData))
                {
                    foreach(ObjectTypes type in trackingObj.GetComponent<PlacedObject>().requiredObjectType)
                    {
                        if(type != hitData.transform.GetComponent<PlacedObject>().objectType)
                        {
                            return;
                        }
                    }
                }
            }
            else
            {
                bool found = false;
                foreach (ObjectTypes type in trackingObj.GetComponent<PlacedObject>().requiredObjectType)
                {
                    if (type == hitData.transform.gameObject.GetComponentInParent<PlacedObject>().objectType)
                    {
                        found = true;
                    }
                }
                if (!found)
                {
                    print("NOT FOUND");
                    return;
                }
            }
        }
        else
        {
            return;
        }
        canPlace = true;
        placementMaterial.SetColor("_BaseColor", canPlaceColor);
    }
    [System.Serializable]
    public class PlacementPart
    {
        public GameObject part;
        public Material ogMaterial;


        public void ResetMaterial()
        {
            if(part != null)
            {
                part.GetComponent<MeshRenderer>().material = ogMaterial;
            }
        }

        public PlacementPart(GameObject thisPart)
        {
            part = thisPart;
            ogMaterial = part.GetComponent<MeshRenderer>().material;
        }
    }
}
