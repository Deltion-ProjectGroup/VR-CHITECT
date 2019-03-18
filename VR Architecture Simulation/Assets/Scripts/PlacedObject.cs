using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlacedObject : Interactable
{
    public GameObject objectPlacedOn;
    public List<GameObject> objectsPlacedOnTop = new List<GameObject>();
    public Item itemData;
    public bool moveable;
    public ObjectTypes[] requiredObjectType;
    public ObjectTypes objectType = ObjectTypes.NonStackable;
    // Start is called before the first frame update

    public override void Interact()
    {
        if (CanPickup())
        {
            OnPickUp();
            Placer.placer.SetTrackingObject(gameObject);
        }
    }
    public void OnPlace()
    {
        RaycastHit hitData;
        if(Physics.Raycast(transform.position, Vector3.down, out hitData, 1))
        {
            objectPlacedOn = hitData.transform.gameObject.GetAbsoluteParent();
            objectPlacedOn.GetComponent<PlacedObject>().objectsPlacedOnTop.Add(gameObject);
        }
    }
    public bool CanPickup()
    {
        if(objectsPlacedOnTop.Count > 0)
        {
            return false;
        }
        return true;
    }
    public void OnPickUp()
    {
        objectPlacedOn.GetComponent<PlacedObject>().objectsPlacedOnTop.Remove(gameObject);
        objectPlacedOn = null;
    }
}

public enum ObjectTypes
{
    Wall, Floor, Ceiling, NonStackable, Stackable
}
