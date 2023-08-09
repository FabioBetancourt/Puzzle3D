using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject movesCanvas;
    
    private int movesLeft = 10;

    private void Start()
    {
        UpdateMovesText();
    }
    
    public void ActivateMovesCanvas()
    {
        movesCanvas.SetActive(true);
    }
    
    public void DesactiveMovesCanvas()
    {
        movesCanvas.SetActive(false);
    }
        
    public void DesactiveLoseCanvas()
    {
        gameOverCanvas.SetActive(false);
    }

    // Decrease move counter and update UI
    public void DecrementMoves()
    {
        movesLeft--;
        UpdateMovesText();

        if (movesLeft <= 0)
        {
            HandleGameOver();
            Time.timeScale = 0;
            movesLeft = 1;
        }
    }

    // Update moves counter in the UI
    private void UpdateMovesText()
    {
        movesText.text = "Movements: " + movesLeft;
    }
    
    private void HandleGameOver()
    {
        if (!gameOverCanvas.activeSelf)
        {
            gameOverCanvas.SetActive(true);
        }
    }
    
    public void ResetMoves()
    {
        movesLeft = 10;
        UpdateMovesText();
    }
}