using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isWalkable;
    public GameObject _moveHighlight;
    public GameObject _attackHighlight;

    public Piece OccupiedPiece;
    public bool Walkable => _isWalkable && OccupiedPiece == null;

    public virtual void Init(int x, int y)
    {
        
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    public void SetPiece(Piece newPiece){
        if(newPiece.OccupiedTile != null) newPiece.OccupiedTile.OccupiedPiece = null;
        newPiece.transform.position = transform.position - newPiece.PeiceOffset;
        OccupiedPiece = newPiece;
        newPiece.OccupiedTile = this;
    }

    private void OnMouseDown(){
        if(GameManager.Instance.GameState != GameState.WhitesTurn) return;

        if(OccupiedPiece != null){
            if(OccupiedPiece.PieceColor == PieceColor.White){ 
            UnitManager.Instance.SetSelectedPiece((BaseWhitePiece)OccupiedPiece);
            GridManager.Instance.SetHighlight();
            }
            else{
                if(UnitManager.Instance.SelectedPiece != null ){
                    var enemy = (BaseBlackPiece) OccupiedPiece;
                    if((enemy.transform.position - UnitManager.Instance.SelectedPiece.transform.position).magnitude < UnitManager.Instance.MoveDistance){
                        UnitManager.Instance.DestroyPiece(enemy);
                        enemy.Dead = true;
                        UnitManager.Instance.SetSelectedPiece(null);
                    }
                }
            }
        }
        else{
            if(UnitManager.Instance.SelectedPiece != null && _isWalkable && FindDistanceFromPiece() < UnitManager.Instance.MoveDistance){
                SetPiece(UnitManager.Instance.SelectedPiece);
                UnitManager.Instance.SetSelectedPiece(null);

            }
        }
    }
    //Abstraction
    public float FindDistanceFromPiece(){
        if(UnitManager.Instance.SelectedPiece != null){
            return (float)(transform.position - UnitManager.Instance.SelectedPiece.transform.position).magnitude;
        }
        return 0f;
    }
}