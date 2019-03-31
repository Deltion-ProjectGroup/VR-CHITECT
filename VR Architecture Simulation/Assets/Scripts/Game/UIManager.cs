using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

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
    [SerializeField] SteamVR_Input_Sources shopToggleSource, settingsToggleSource;
    public GameObject leftDialog, rightDialog;
    public enum DialogSource { leftHand, rightHand}
    // Start is called before the first frame update
    private void Awake()
    {
        uiManager = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (shopToggleButton.GetStateDown(InputMan.GetHand(shopToggleSource)) && canToggle)
        {
            ToggleMenu(shop);
        }
        if (settingsToggleButton.GetStateDown(InputMan.GetHand(settingsToggleSource)) && canToggle)
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
            print("CLOSED");
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
                print("ACTIVATED");
                menu.SetActive(true);
                canToggle = false;
                Player.canInteract = false;
                StartCoroutine(menu.GetComponent<UIMenu>().Open());
            }
            else
            {
                print("CANNOT INTERACT");
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
    public void EnableDialogUI(DialogEvent dialogText, DialogSource source)
    {
        GameObject wantedSource;
        if(source == DialogSource.leftHand)
        {
            wantedSource = leftDialog;
        }
        else
        {
            wantedSource = rightDialog;
        }
        wantedSource.SetActive(true);
        wantedSource.GetComponent<DialogSystem>().StartDialog(dialogText);
    }
}
