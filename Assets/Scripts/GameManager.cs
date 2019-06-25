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

    }

    IEnumerator Tick()
    {
        //Debug.Log("Tick");

        // update piece (move down if possible)
        if(activePiece)

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
        int rotateAmount = Random.Range(0, 3);
        for(int i = 0; i < rotateAmount; i++)
            activePiece.GetComponent<Piece>().RotateRight();

        // find bottom most tile
        Transform lowestTile = activePiece.GetComponent<Piece>().GetLowestTile();
        //Debug.Log(lowestTile.position);

        // move piece so that bottom most tile is just above playgroundHeight
        float offset = playfieldHeight - lowestTile.position.y;
        activePiece.transform.position = new Vector3(activePiece.transform.position.x, activePiece.transform.position.y + offset, activePiece.transform.position.z);
        
        // snap to grid fn
        activePiece.GetComponent<Piece>().SnapToGrid();
    }

    

}

