using System.Collections;
using UnityEngine;

public abstract class UIMenu : MonoBehaviour
{

    public abstract void InstantClose();

    public abstract IEnumerator Open();
}
