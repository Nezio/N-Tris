using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private void Update()
    {
        // rotate piece
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateRight();
        }
        
    }

    public void SnapToGrid()
    { // move the piece so that tiles within it allign to the grid
        Transform tile = gameObject.transform.GetChild(0);
        float offsetX = tile.position.x % 1;
        float offsetY = tile.position.y % 1;
        float newX;
        float newY;

        if (offsetX <= 0.5)
            newX = gameObject.transform.position.x - offsetX;
        else
            newX = gameObject.transform.position.x + 1 - offsetX;

        if (offsetY <= 0.5)
            newY = gameObject.transform.position.y - offsetY;
        else
            newY = gameObject.transform.position.y + 1 - offsetY;

        gameObject.transform.position = new Vector3(newX, newY, gameObject.transform.position.z);
    }

    public void RotateRight()
    {
        gameObject.transform.Rotate(0, 0, -90);
        SnapToGrid(); // not really needed, but its here just in case piece moves to 1.002 instead of 1
    }

    public Transform GetLowestTile()
    {
        Transform lowestTile = gameObject.transform.GetChild(0);
        foreach (Transform tile in gameObject.transform)
        {
            if (tile.position.y < lowestTile.position.y)
                lowestTile = tile;
        }

        return lowestTile;
    }

}
