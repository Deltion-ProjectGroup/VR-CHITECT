public class PlacedObject : Interactable
{
    public Item itemData;
    public bool moveable;
    public ObjectTypes[] requiredObjectType;
    public ObjectTypes objectType = ObjectTypes.NonStackable;
    // Start is called before the first frame update

    public override void Interact()
    {
        Placer.placer.SetTrackingObject(gameObject);
    }
}

public enum ObjectTypes
{
    Wall, Floor, Ceiling, NonStackable, Stackable
}
