using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject difficultCanvas;
    [SerializeField] private GameObject easyLevel;
    [SerializeField] private GameObject mediumLevel;
    [SerializeField] private GameObject hardLevel;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject controls;

    private void Awake()
    {
        ShowMenu();
    }
    
    private void DisableAll()
    {
        menuCanvas.SetActive(false); 
        creditsCanvas.SetActive(false);
        difficultCanvas.SetActive(false);
        winCanvas.SetActive(false);
        easyLevel.SetActive(false);
        mediumLevel.SetActive(false); 
        hardLevel.SetActive(false);
        controls.SetActive(false);

        UIController uiController = FindObjectOfType<UIController>();
        if (uiController != null)
        {
            uiController.DesactiveMovesCanvas(); 
            uiController.DesactiveLoseCanvas();
        }
    }

    private void CommonLevelStart(GameObject level)
    {
        Time.timeScale = 1;
        DisableAll();
        level.SetActive(true);

        UIController uiController = FindObjectOfType<UIController>();
        if (uiController != null)
        {
            uiController.ActivateMovesCanvas();
        }

        RotationRing rotationRing = FindObjectOfType<RotationRing>();
        if (rotationRing != null)
        {
            rotationRing.SetCanInteract(true);
        }
        
        RandomRingRotation[] rings = FindObjectsOfType<RandomRingRotation>();
        foreach (RandomRingRotation ring in rings)
        {
            ring.SetRandomRotation();
        }
    }

    public void ShowMenu()
    {
        DisableAll();
        menuCanvas.SetActive(true);
    
        UIController uiController = FindObjectOfType<UIController>();
        if (uiController != null)
        {
            uiController.ResetMoves();
        }

        RotationRing rotationRing = FindObjectOfType<RotationRing>();
        if (rotationRing != null)
        {
            rotationRing.SetCanInteract(false);
        }
    }

    public void ShowWin()
    {
        DisableAll();
        winCanvas.SetActive(true);
        RotationRing rotationRing = FindObjectOfType<RotationRing>();
        if (rotationRing != null)
        {
            rotationRing.SetCanInteract(false);
        }
        RandomRingRotation[] rings = FindObjectsOfType<RandomRingRotation>();
        foreach (RandomRingRotation ring in rings)
        {
            ring.SetRandomRotation();
        }
    }
    public void ShowControls()
    {
        DisableAll();
        controls.SetActive(true);
    }
    public void ShowCredits()
    {
        DisableAll();
        creditsCanvas.SetActive(true);
    }
    
    public void ShowDifficult()
    {
        DisableAll();
        difficultCanvas.SetActive(true);
    }

    public void StartEasyLevel()
    {
        CommonLevelStart(easyLevel);
    }
    
    public void StartMediumLevel()
    {
        CommonLevelStart(mediumLevel);
    }
    
    public void StartHardLevel()
    {
        CommonLevelStart(hardLevel);
    }
}
