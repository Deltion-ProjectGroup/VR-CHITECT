using UnityEngine;
using Valve.VR;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;
    [Header("Shop")]
    public GameObject shop;
    public GameObject settings;
    public GameObject properties;
    [SerializeField] GameObject[] allMenus;
    [SerializeField] SteamVR_Action_Boolean shopToggleButton;
    [SerializeField] SteamVR_Action_Boolean settingsToggleButton;
    public bool canToggle = true;
    [Header("ScreenFade")]
    public Animation fadeAnimation;
    [SerializeField] AnimationClip fadeAppear;
    [SerializeField] AnimationClip fadeRemove;
    // Start is called before the first frame update
    private void Awake()
    {
        uiManager = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (shopToggleButton.GetStateDown(InputMan.leftHand) && canToggle)
        {
            ToggleMenu(shop);
        }
        if (settingsToggleButton.GetStateDown(InputMan.rightHand) && canToggle)
        {
            ToggleMenu(settings);
        }
    }
    public void ScreenFade(bool appear)
    {
        if (appear)
        {
            fadeAnimation.clip = fadeAppear;
        }
        else
        {
            fadeAnimation.clip = fadeRemove;
        }
        fadeAnimation.Play();
    }
    public void ToggleMenu(GameObject menu)
    {
        if (menu.activeSelf)
        {
            menu.GetComponent<UIMenu>().InstantClose();
            Player.canInteract = true;
        }
        else
        {
            if (Player.canInteract)
            {
                foreach (GameObject thisMenu in allMenus)
                {
                    if (thisMenu != menu)
                    {
                        if (thisMenu.activeSelf)
                        {
                            thisMenu.GetComponent<UIMenu>().InstantClose();
                            break;
                        }
                    }
                }
                menu.SetActive(true);
                canToggle = false;
                Player.canInteract = false;
                StartCoroutine(menu.GetComponent<UIMenu>().Open());
            }
        }
    }
    public void DisableUI(GameObject toDisable)
    {
        toDisable.SetActive(false);
    }
    public void EnableUI(GameObject toEnable)
    {
        toEnable.SetActive(true);
    }
}
