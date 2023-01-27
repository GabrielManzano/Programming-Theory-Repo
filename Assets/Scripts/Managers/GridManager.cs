using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private int _width, _height; 

    [SerializeField] private Tile _boardTilePrefab, _obstacleTilePrefab;

    [SerializeField] private Transform _cam;

    [SerializeField] public Dictionary<Vector2, Tile> _tiles;

    [SerializeField] private Transform _boardParent;

    private void Awake(){
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
    }


    public void GenerateGrid()
    {
        _cam = GameObject.Find("Main Camera").transform;
        _boardParent = GameObject.Find("Board").transform;
        _tiles = new Dictionary<Vector2, Tile>();
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var randomTile = Random.Range(0,6) == 3 ? _obstacleTilePrefab : _boardTilePrefab;
                var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity, _boardParent);
                spawnedTile.name = $"Tile {x} {y}";

                spawnedTile.Init(x,y);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width/2 - 0.5f, (float)_height/2 - 0.5f, -10);

        GameManager.Instance.ChangeState(GameState.GenerateWhitePiece);
    }

    public Tile GetWhiteSpawnTile(){
        return _tiles.Where(t=>t.Key.x < _width/2 && t.Value.Walkable).OrderBy(t=>Random.value).First().Value;
    }

    public Tile GetBlackSpawnTile(){
        return _tiles.Where(t=>t.Key.x > _width/2 && t.Value.Walkable).OrderBy(t=>Random.value).First().Value;
    }


    public Tile GetTileAtPosition(Vector2 pos)
    {
        if(_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }
    public void SetHighlight(){
        foreach(var tile in _tiles){
            if(UnitManager.Instance.SelectedPiece != null){
                if(tile.Value.FindDistanceFromPiece() < UnitManager.Instance.MoveDistance && tile.Value.Walkable) tile.Value._moveHighlight.SetActive(true);
            }
            if(tile.Value.OccupiedPiece != null){
                if(tile.Value.OccupiedPiece.PieceColor == PieceColor.Black && tile.Value.FindDistanceFromPiece() < UnitManager.Instance.MoveDistance){
                    tile.Value._attackHighlight.SetActive(true);
                } 
            }
            
        }
    }
    public void RemoveHighlight(){
        foreach(var tile in _tiles){
            tile.Value._moveHighlight.SetActive(false);
            tile.Value._attackHighlight.SetActive(false);
        }
    }
}
