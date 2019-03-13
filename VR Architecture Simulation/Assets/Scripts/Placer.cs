using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Placer : MonoBehaviour
{
    public static Placer placer;
    GameObject trackingObj;
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
            if (placeButton.GetStateDown(InputMan.rightHand) && canPlace)
            {
                StartCoroutine(PlaceTrackingObject());
            }
            else
            {
                if (placeButton.GetStateDown(InputMan.leftHand))
                {
                    DestroyPlacingObject();
                    return;
                }
                //Snapping
                ToggleGridSnap();
                ToggleRotationSnap();

                //PlacementCheck
                Rotate();
                RaycastHit hit;
                Ray ray = new Ray(hand.position, hand.forward);
                if (Physics.Raycast(ray, out hit, 1000, placementMask))
                {
                    ChangePosition(hit);
                }
                CheckPlacable();
            }
        }
    }
    public void DestroyPlacingObject()
    {
        Player.canInteract = true;
        offset = Vector3.zero;
        UIManager.uiManager.ToggleMenu(UIManager.uiManager.properties);
        Destroy(trackingObj);
        trackingObj = null;
        canSetObject = true;
    }
    public static Vector3 CalculateOffset(Vector3 vertLocalPosition, Transform vertOwner, Vector3 ownerPosition)
    {

        Vector3 vertWorldPos = vertOwner.TransformPoint(vertLocalPosition);
        return vertWorldPos - ownerPosition;
    }
    void ChangePosition(RaycastHit hitData)
    {
        Vector3 hitPoint = hitData.point;
        if (vertSnapping)
        {

            Vector3 nearestVert = Vector3.zero;
            float nearestVertDistance = Mathf.Infinity;
            foreach (Transform child in hitData.transform)
            {
                foreach (Vector3 vert in child.GetComponent<MeshFilter>().mesh.vertices)
                {
                    if (Vector3.Distance(hitData.point, hitData.transform.TransformPoint(vert)) < nearestVertDistance)
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
            if (snappingPosition && hitData.transform.gameObject.GetComponent<PlacedObject>().objectType != ObjectTypes.Wall)
            {
                hitPoint.x = Mathf.RoundToInt(hitPoint.x / gritTileSize) * gritTileSize;
                hitPoint.z = Mathf.RoundToInt(hitPoint.z / gritTileSize) * gritTileSize;
            }
        }
        trackingObj.transform.position = hitPoint;
    }
    void Rotate()
    {
        float rotateAmount = rotateButton.GetAxis(InputMan.rightHand).x;
        if (snappingRotation)
        {
            if (rotatePress.GetStateDown(InputMan.rightHand))
            {
                rotateAmount = Mathf.RoundToInt(rotateAmount);
                rotateAmount *= rotateTurnAmount;
                trackingObj.transform.Rotate(new Vector3(0, rotateAmount, 0));
            }
        }
        else
        {
            if (rotatePress.GetState(InputMan.rightHand))
            {
                trackingObj.transform.Rotate(new Vector3(0, rotateAmount, 0));
            }
        }
        if (vertSnapping)
        {
            offset = CalculateOffset(Player.nearestVert, trackingObj.transform, trackingObj.transform.position);
        }
    }
    public void SetTrackingObject(GameObject thisObject)
    {
        if(trackingObj == null && canSetObject)
        {
            canSetObject = false;
            trackingObj = thisObject;
            trackingObj.GetComponent<Collider>().enabled = false;
            List<PlacementPart> allObjectMaterials = new List<PlacementPart>();
            for(int i = 0; i < thisObject.transform.childCount; i++)
            {
                allObjectMaterials.Add(new PlacementPart(thisObject.transform.GetChild(i).gameObject));
                allObjectMaterials[allObjectMaterials.Count - 1].part.GetComponent<MeshRenderer>().material = placementMaterial;
            }
            ogPartData = allObjectMaterials.ToArray();
            UIManager.uiManager.properties.GetComponent<PropertiesMenu>().targetRN = trackingObj;
            UIManager.uiManager.ToggleMenu(UIManager.uiManager.properties);
            UIManager.uiManager.properties.GetComponent<PropertiesMenu>().Initialize(trackingObj);
            CheckPlacable();
        }
    }
    IEnumerator PlaceTrackingObject()
    {
        Player.canInteract = true;
        print("PLACED");
        offset = Vector3.zero;
        GameObject oldTracker = trackingObj;
        foreach(PlacementPart partData in ogPartData)
        {
            partData.ResetMaterial();
        }
        UIManager.uiManager.ToggleMenu(UIManager.uiManager.properties);
        trackingObj = null;
        yield return null;
        canSetObject = true;
        oldTracker.GetComponent<Collider>().enabled = true;
    }
    void ToggleGridSnap()
    {
        if (positionSnapButton.GetStateDown(InputMan.leftHand))
        {
            snappingPosition = true;
            //ToggleGrid(true);
        }
        else
        {
            if (positionSnapButton.GetStateUp(InputMan.leftHand))
            {
                snappingPosition = false;
                //ToggleGrid(false);
            }
        }
    }
    void ToggleRotationSnap()
    {
        if (rotationSnapButton.GetStateDown(InputMan.rightHand))
        {
            snappingRotation = snappingRotation.ToggleBool();
            if (snappingRotation)
            {
                Vector3 snappedRotation = trackingObj.transform.eulerAngles;
                snappedRotation.y = Mathf.RoundToInt(snappedRotation.y / rotateTurnAmount) * rotateTurnAmount;
                trackingObj.transform.eulerAngles = snappedRotation;
            }
        }
        else
        {
            if (rotationSnapButton.GetStateUp(InputMan.rightHand))
            {
                snappingRotation = snappingRotation.ToggleBool();
            }
        }
    }
    void CalculateTilePositions(GameObject[] groundTiles)
    {
        //UIManager.uiManager.settings.GetComponent<Options>().UpdateGridDivision(divisionAmount);
        DeleteCurrentTiles();
        foreach (GameObject groundTile in groundTiles)
        {
            tileSize.x = Mathf.Abs(groundTile.GetComponent<Collider>().bounds.max.x - groundTile.GetComponent<Collider>().bounds.min.x);
            tileSize.y = Mathf.Abs(groundTile.GetComponent<Collider>().bounds.max.z - groundTile.GetComponent<Collider>().bounds.min.z);
            gridTileSize.x = tileSize.x / divisionAmount;
            gridTileSize.y = tileSize.y / divisionAmount;


            for (int hor = 0; hor < divisionAmount; hor++)
            {
                for (int ver = 0; ver < divisionAmount; ver++)
                {
                    Vector2 newPos = Vector2.zero;
                    newPos.x += (hor * gridTileSize.x);
                    newPos.y += (ver * gridTileSize.y);
                    newPos += gridTileSize / 2;
                    newPos.x += groundTile.GetComponent<Collider>().bounds.min.x;
                    newPos.y += groundTile.GetComponent<Collider>().bounds.min.z;

                    Vector3 newTilePos = new Vector3(newPos.x, groundTile.transform.position.y, newPos.y);
                    GameObject newTile = Instantiate(tile, newTilePos, Quaternion.identity);

                    Vector3 tileSize = new Vector3(gridTileSize.x, newTile.transform.localScale.y, gridTileSize.y);
                    tileSize.x -= 0.05f;
                    tileSize.z -= 0.05f;

                    newTile.transform.localScale = tileSize;
                    newTile.SetActive(snappingPosition);
                    allTiles.Add(newTile);

                }
            }
        }
    }
    void DeleteCurrentTiles()
    {
        foreach (GameObject tile in allTiles)
        {
            Destroy(tile);
        }
        allTiles = new List<GameObject>();
    }
    void ToggleGrid(bool show)
    {
        foreach (GameObject tile in allTiles)
        {
            tile.SetActive(show);
        }
    }
    public void ChangeTileDivision(float changeAmount)
    {
        gritTileSize += changeAmount;
        gritTileSize = Mathf.Clamp(gritTileSize, 0.1f, 1);
        UIManager.uiManager.settings.GetComponent<Options>().UpdateGridDivision(gritTileSize);
        //CalculateTilePositions(GameObject.FindGameObjectsWithTag("Ground"));
    }
    public void ChangeSnapRotation(int changeAmount)
    {
        rotateTurnAmount += changeAmount;
        rotateTurnAmount = Mathf.Clamp(rotateTurnAmount, 0, 360);
        UIManager.uiManager.settings.GetComponent<Options>().UpdateRotationSnap(rotateTurnAmount);
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
    void CheckPlacable()
    {
        canPlace = false;
        Collider[] collisions = Physics.OverlapBox(trackingObj.transform.position + trackingObj.GetComponent<BoxCollider>().center, trackingObj.GetComponent<BoxCollider>().size / 2, trackingObj.transform.rotation);
        foreach(Collider col in collisions)
        {
            if(col.gameObject != trackingObj)
            {
                print("COLLIDED");
                placementMaterial.SetColor("_BaseColor", cannotPlaceColor);
                return;
            }
        }
        RaycastHit hitObject;
        if(Physics.Raycast(trackingObj.transform.position, -trackingObj.transform.up, out hitObject, 1000))
        {
            bool found = false; ;
            foreach (ObjectTypes type in trackingObj.GetComponent<PlacedObject>().requiredObjectType)
            {
                if(type == hitObject.transform.GetComponent<PlacedObject>().objectType)
                {
                    found = true;
                }
            }
            if (!found)
            {
                placementMaterial.SetColor("_BaseColor", cannotPlaceColor);
                print("NOT FOUND");
                return;
            }
        }
        canPlace = true;
        placementMaterial.SetColor("_BaseColor", canPlaceColor);
    }
    public class PlacementPart
    {
        public GameObject part;
        public Material ogMaterial;


        public void ResetMaterial()
        {
            part.GetComponent<MeshRenderer>().material = ogMaterial;
        }

        public PlacementPart(GameObject thisPart)
        {
            part = thisPart;
            ogMaterial = part.GetComponent<MeshRenderer>().material;
        }
    }
}
