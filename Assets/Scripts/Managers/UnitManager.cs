using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public float MoveDistance;

    public static UnitManager Instance;

    private List<ScriptablePiece> _pieces;

    public BaseWhitePiece SelectedPiece;

    public static int numOfEnemies;

    private void Awake(){
        if(Instance != null){
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }

        _pieces = Resources.LoadAll<ScriptablePiece>("Pieces").ToList();
    }

    public void SpawnWhitePiece(){
        int pieceCount = GameManager.Instance.Wave/2 + 1;

        for(int i = 0; i < pieceCount; i++){
            var randomPiece = GetRandomPiece<Piece>(PieceColor.White);
            var spawnedPiece = Instantiate(randomPiece);
            var randomSpawnTile = GridManager.Instance.GetWhiteSpawnTile();

            randomSpawnTile.SetPiece(spawnedPiece);
        }
        GameManager.Instance.ChangeState(GameState.GenerateBlackPiece);
    }

    public void SpawnBlackPiece(){
        int pieceCount = GameManager.Instance.Wave + 1;

        for(int i = 0; i < pieceCount; i++){
            var randomPiece = GetRandomPiece<Piece>(PieceColor.Black);
            var spawnedPiece = Instantiate(randomPiece);
            var randomSpawnTile = GridManager.Instance.GetBlackSpawnTile();

            randomSpawnTile.SetPiece(spawnedPiece);
        }
        GameManager.Instance.ChangeState(GameState.WhitesTurn);
    }

    private T GetRandomPiece<T>(PieceColor pieceColor) where T : Piece{
        return (T)_pieces.Where(u => u.PieceColor == pieceColor).OrderBy(o=>Random.value).First().PiecePrefab;
    }

    public void SetSelectedPiece(BaseWhitePiece pieceToSet){
        if(pieceToSet == null){
             GridManager.Instance.RemoveHighlight();
             GameManager.Instance.ChangeState(GameState.BlacksTurn);
        }
        SelectedPiece = pieceToSet;
    }
    public void DestroyPiece(Piece pieceToDestroy){
        Destroy(pieceToDestroy.gameObject);
        numOfEnemies = FindObjectsOfType<BaseBlackPiece>().Length - 1;
        if(numOfEnemies == 0){
            GameManager.Instance.Restart();
        }
        else{
            GameManager.Instance.ChangeState(GameState.BlacksTurn);
        }
    }
}
