using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlackPiece : Piece
{
    //Inheritence
    public bool Dead = false;
    public bool TakenTurn = true;
    public virtual void TakeTurn(){
        if(!Dead){
            foreach(var whitePiece in FindObjectsOfType<BaseWhitePiece>()){
            if((whitePiece.transform.position - transform.position).magnitude < UnitManager.Instance.MoveDistance){
                Debug.Log(this);
                if(FindObjectsOfType<BaseWhitePiece>().Length == 1) GameManager.Instance.ChangeState(GameState.Lose);
                Destroy(whitePiece.gameObject);
                return;
            }
        }
        List<Tile> possibleTiles = new List<Tile>();
        foreach(var tile in GridManager.Instance._tiles){
            if((transform.position - tile.Value.transform.position).magnitude < UnitManager.Instance.MoveDistance && tile.Value.Walkable){
                possibleTiles.Add(tile.Value);
            }
        }
        int tileIndex = Random.Range(0,possibleTiles.Count);
        Tile tileMoveTo = possibleTiles[tileIndex];
        tileMoveTo.SetPiece(this);
        }
        GameManager.Instance.ChangeState(GameState.WhitesTurn);
    }
}
