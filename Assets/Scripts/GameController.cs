using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Sprite tileSprite;

    public GameObject GameControllerObj;
    public TileController[] tileList;
    public GameObject TileGridManager;
    public AudioSource GameFinishedAudioSource;
    public Button EndTurnButton;
    public Text gameStateText;
    public string currentPlayer = "P1";
    public bool gameFinishedFlag = false;
    public bool restartFlag = false;

    public TileController lastTile;
    public Vector2Int firstTilePos;
    List<TileController> validTiles;
    public int moveCount;

    void Start()
    {
        SetTileList();
        moveCount = 0;
        validTiles = new List<TileController>();
        gameStateText.text = currentPlayer + "'s Turn";
        EndTurnButton.interactable = false;
        EndTurnButton.onClick.AddListener(EndTurn);
    }

    public void SetTileList()
    {
        tileList = TileGridManager.GetComponentsInChildren<TileController>();
    }

    public void ChangeTurn()
    {
        currentPlayer = (currentPlayer == "P1") ? "P2" : "P1";
        gameStateText.text = currentPlayer + "'s Turn";
        lastTile = null;
    }

    public void EndTurn()
    {
        moveCount = 0;
        HideHighlight();
        UnblockTiles();
        EndTurnButton.interactable = false;
        if (restartFlag)
        {
            RestartGame();
        }
        else
        {
            gameFinishedFlag = IsGameOver();
            if (gameFinishedFlag)
                GameOver();
            else
                ChangeTurn();

        }
        
    }

    public void FindValidTiles(TileController tile)
    {
        Vector2Int diff;
        string direction = "null";
        if (moveCount > 1)
        {
            diff = firstTilePos - lastTile.tilePos;
            direction = (diff.x == 0) ? "v" : "h"; // horizontal or vertical
        }
            
        validTiles.Clear();
        foreach (TileController t in tileList)
        {
            if (t.active && 
                ((t.tilePos.x == lastTile.tilePos.x) || (t.tilePos.y == lastTile.tilePos.y)) &&
                (System.Math.Abs(t.tilePos.x - lastTile.tilePos.x) <= 1) &&
                (System.Math.Abs(t.tilePos.y - lastTile.tilePos.y) <= 1)
                )
            {
                // direction
                if (moveCount > 1)
                {
                    if (direction == "v")
                    {
                        if (t.tilePos.x == firstTilePos.x)
                            validTiles.Add(t);
                    }
                    else
                    {
                        if (t.tilePos.y == firstTilePos.y)
                            validTiles.Add(t);
                    }
                }
                else
                    validTiles.Add(t);
                
            }
        }
    }

    public void UpdateGameState()
    {
        if (moveCount > 0)
            EndTurnButton.interactable = true;
        HideHighlight();
        FindValidTiles(lastTile);
        foreach (TileController t in validTiles)
        {
            t.Highlight();
            t.UnblockTile();
        }
    }

    public void HideHighlight()
    {
        foreach (TileController t in tileList)
        {
            t.HideHighlight();
        }
    }

    public void BlockTiles()
    {
        foreach (TileController t in tileList)
        {
            t.BlockTile();
        }
    }

    public void UnblockTiles()
    {
        foreach (TileController t in tileList)
        {
            t.UnblockTile();
        }
    }

    public bool IsGameOver()
    {
        // game is over if there's no active tile left
        foreach (TileController t in tileList)
        {
            if (t.active)
                return false;
        }
        return true;
    }

    public void GameOver()
    {
        gameStateText.text = "Winner : " + ((currentPlayer == "P1") ? "P2" : "P1");
        EndTurnButton.GetComponentInChildren<Text>().text = "Restart Game";
        GameFinishedAudioSource.PlayOneShot(GameFinishedAudioSource.clip);
        restartFlag = true;
        EndTurnButton.interactable = true;
    }

    public void RestartGame()
    {
        currentPlayer = "P1";
        gameStateText.text = currentPlayer + "'s Turn";
        gameFinishedFlag = false;
        ResetTiles();
        restartFlag = false;
        EndTurnButton.GetComponentInChildren<Text>().text = "End Turn";
        lastTile = null;
    }
    
    private void ResetTiles()
    {
        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<TileController>().ResetTile();
            tileList[i].GetComponentInParent<TileController>().UnblockTile();
        }
    }
}
