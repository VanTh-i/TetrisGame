using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public TetrominoData data { get; private set; }

    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;
    private float stepTime;
    protected internal float lockTime;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if(this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for(int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        this.board.Clear(this);

        this.lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
            
        }
        
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }


        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            HardDrop();
            //Move(Vector2Int.down);
        }

        //rotate
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate(1);
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    HardDrop();
        //}
 

        if(Time.time >= this.stepTime)
        {
            Step();
        }

        this.board.Set(this);
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        if(this.lockTime >= this.lockDelay)
        {
            Lock();
            //this.board.ClearNext(nextPiece);
        }
    }
    public void Lock()
    {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }
    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        Lock();

    }
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.IsValidPosition(this, newPosition);

        if (valid)
        {
            position = newPosition;
            //moveTime = Time.time + moveDelay;
            this.lockTime = 0f; // reset
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];
            int x, y;

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));

                    break;
            }
            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallkickIndex = GetWallKicksIndex(rotationIndex, rotationDirection);

        for(int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallkickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }
    private int GetWallKicksIndex(int rotationIndex, int rotationDirection)
    {
        int wallkicksIndex = rotationIndex * 2;

        if(rotationDirection < 0)
        {
            wallkicksIndex--;
        }
        return Wrap(wallkicksIndex, 0, this.data.wallKicks.GetLength(0));
    }

    //wrap 3 ve den 0 neu index vuot qua 3 va nguoc lai
    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
