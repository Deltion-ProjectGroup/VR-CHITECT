using UnityEngine;

public class PartData : MonoBehaviour
{
    public MaterialData[] availableMaterials;
    public Sprite partIcon;
    public string partname;
}
[System.Serializable]
public class MaterialData
{
    public Material thisMaterial;
    public Color iconColor;
}
