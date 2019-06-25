using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playfieldWidth = 10;
    public int playfieldHeight = 20;
    public float tickSpeed = 1;
    public GameObject wallLeftPrefab;
    public GameObject wallRightPrefab;
    public GameObject floorPrefab;
    
    public GameObject[] pieceGallery;


    private GameObject activePiece = null;


    void Start()
    {
        DrawScene();

        SpawnPiece();

        // start ticking
        StartCoroutine("Tick");
    }

    private void Update()
    {
        // tmp debug control
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // rotate block right; TODO: move this code to where it should be (maybe controller script?)
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotatePiece(ref activePiece);
        }
    }

    IEnumerator Tick()
    {
        //Debug.Log("Tick");

        // update piece (move down if possible)
        // if not possible spawn new piece and update activePiece


        yield return new WaitForSeconds(tickSpeed);
        StartCoroutine("Tick");
    }

    void DrawScene()
    {
        GameObject FloorTiles = GameObject.FindGameObjectWithTag("FloorTiles");
        GameObject WallLeftTiles = GameObject.FindGameObjectWithTag("WallLeftTiles");
        GameObject WallRightTiles = GameObject.FindGameObjectWithTag("WallRightTiles");

        // draw floor
        for (int i = 0; i < playfieldWidth; i++)
        {
            GameObject floorTile = GameObject.Instantiate(floorPrefab, new Vector3(i, -1, 0), Quaternion.identity);
            floorTile.transform.SetParent(FloorTiles.transform);
        }
        
        // draw walls
        for (int i = -1; i < playfieldHeight; i++)
        {
            GameObject wallTileLeft = GameObject.Instantiate(wallLeftPrefab, new Vector3(-1, i, 0), Quaternion.identity);
            GameObject wallTileRight = GameObject.Instantiate(wallRightPrefab, new Vector3(playfieldWidth, i, 0), Quaternion.identity);
            wallTileLeft.transform.SetParent(WallLeftTiles.transform);
            wallTileRight.transform.SetParent(WallRightTiles.transform);
        }

    }

    void SpawnPiece()
    {
        // spawn random piece in the middle of top playing field border
        GameObject piece = pieceGallery[Random.Range(0, pieceGallery.Length)];
        Vector3 location = new Vector3((float)(playfieldWidth - 1) / 2, (float)playfieldHeight, (float)0);
        activePiece = GameObject.Instantiate(piece, location, Quaternion.identity);

        // random rotation

        // find bottom most tile
        Transform lowestTile = activePiece.transform.GetChild(0);
        foreach(Transform tile in activePiece.transform)
        {
            if (tile.position.y < lowestTile.position.y)
                lowestTile = tile;
        }
        //Debug.Log(lowestTile.position);
        // move piece so that bottom most tile is just above playgroundHeight
        float offset = activePiece.transform.position.y - lowestTile.position.y;
        activePiece.transform.position = new Vector3(activePiece.transform.position.x, activePiece.transform.position.y + offset, activePiece.transform.position.z);

        // snap to grid fn
        SnapPieceToGrid(ref activePiece);
    }

    void SnapPieceToGrid(ref GameObject piece)
    { // move the piece so that tiles within it allign to the grid
        Transform tile = piece.transform.GetChild(0);
        float offsetX = tile.position.x % 1;
        float offsetY = tile.position.y % 1;
        float newX;
        float newY;

        if (offsetX < 0.5)
            newX = piece.transform.position.x - offsetX;
        else
            newX = piece.transform.position.x + 1 - offsetX;

        if (offsetY < 0.5)
            newY = piece.transform.position.y - offsetY;
        else
            newY = piece.transform.position.y + 1 - offsetY;

        piece.transform.position = new Vector3(newX, newY, piece.transform.position.z);
    }

    void RotatePiece(ref GameObject piece)
    {
        piece.transform.Rotate(0, 0, -90);
        SnapPieceToGrid(ref piece); // not really needed, but its here just in case piece moves to 1.002 instead of 1
    }

}

