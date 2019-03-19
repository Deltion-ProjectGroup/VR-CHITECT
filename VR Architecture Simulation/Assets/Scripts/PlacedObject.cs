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
        Placer.placer.SetTrackingObject(gameObject);
    }
    public void OnPlace()
    {
        print("ONPLACE");
        RaycastHit hitData;
        if(Physics.Raycast(transform.position, Vector3.down, out hitData, 1))
        {
            print(hitData.transform.gameObject.GetAbsoluteParent().name);
            objectPlacedOn = hitData.transform.gameObject.GetAbsoluteParent();
            objectPlacedOn.GetComponent<PlacedObject>().objectsPlacedOnTop.Add(gameObject);
        }
    }
    public void OnPickUp()
    {
        if (objectPlacedOn)
        {
            print("PICKUP" + gameObject.name);
            objectPlacedOn.GetComponent<PlacedObject>().objectsPlacedOnTop.Remove(gameObject);
            objectPlacedOn = null;
        }
    }
}

public enum ObjectTypes
{
    Wall, Floor, Ceiling, NonStackable, Stackable
}
