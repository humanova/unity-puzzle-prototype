using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    [Header("Other components")]
    public GameController gameController;
    public Button tileButton;
    public bool active = true;
    public Vector2Int tilePos;
    private Color oCol; //original color

    public void Start()
    {
        gameController = GetComponentInParent<GameController>();
        tileButton = GetComponentInChildren<Button>();
        tileButton.onClick.AddListener(UpdateTile);
        oCol = tileButton.GetComponent<Image>().color;
    }

    public void UpdateTile()
    {
        tileButton.enabled = false;
        tileButton.GetComponent<Image>().enabled = false;
        active = false;
        gameController.lastTile = this;
        gameController.moveCount++;

        if (gameController.moveCount == 1)
        {
            gameController.firstTilePos = tilePos;
        }
        gameController.BlockTiles();
        gameController.UpdateGameState();
    }

    public void BlockTile()
    {
        tileButton.interactable = false;
    }

    public void UnblockTile()
    {
        tileButton.interactable = true;
    }

    public void ResetTile()
    {
        tileButton.enabled = true;
        tileButton.GetComponent<Image>().enabled = true;
        active = true;
        UnblockTile();
    }

    public void Highlight()
    {
        tileButton.GetComponent<Image>().color = Color.green;
    }

    public void HideHighlight()
    {
        tileButton.GetComponent<Image>().color = oCol;
    }
}
