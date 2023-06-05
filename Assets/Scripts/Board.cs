using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public GameOver gameOver;
    
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    //public Vector3Int spawnPositionNext;
    public Piece activePiece { get; private set; }
    public NextPiece NextPiece { get; private set; }
    public Tilemap tilemap { get; private set; }
    public Vector2Int boardSize = new Vector2Int(10, 20);

    Score refScore;
    int ScoreNum, HighScore;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.NextPiece = GetComponentInChildren<NextPiece>();
        refScore = FindObjectOfType<Score>();

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }
    private void Start()
    {
        SpawnPiece();
        
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, this.spawnPosition ,data);
        //this.NextPiece.Initialize(this, this.spawnPositionNext, data);//data change
        
        if(IsValidPosition(this.activePiece, this.spawnPosition))
        {
            Set(this.activePiece);
            //SetNext(this.NextPiece);
            
        }
        else
        {
            
            refScore.HighScoreText(HighScore);
            //refScore.SetScoreText(ScoreNum);
            gameOver.GameisOver();
            GameOver();
            HighScore = 0;

        }

    }
    private void GameOver()
    {
        
        this.tilemap.ClearAllTiles();
        ScoreNum = 0;
        refScore.SetScoreText(ScoreNum);

    }
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
        
    }
    public void SetNext(NextPiece nextPiece)
    {
        for (int i = 0; i < nextPiece.cells.Length; i++)
        {
            Vector3Int tilePosition = nextPiece.cells[i] + nextPiece.position;
            this.tilemap.SetTile(tilePosition, nextPiece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void ClearNext(NextPiece nextPiece)
    {
        for (int i = 0; i < nextPiece.cells.Length; i++)
        {
            Vector3Int tilePosition = NextPiece.cells[i] + NextPiece.position;
            this.tilemap.SetTile(tilePosition, null);
            

        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }
        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while(row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
                IncrementScore();
            }
            else
            {
                row++;
                
            }
            
        }
    }
    public void IncrementScore()
    {
        ScoreNum += 10;
        refScore.SetScoreText(ScoreNum);
        HighScore += 10;
        refScore.HighScoreText(HighScore);
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }

        //day hang phia tren xuong duoi hang da bi xoa
        while(row < bounds.yMax)
        {
            for(int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);
                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }
            row++;
        }
    }
}
