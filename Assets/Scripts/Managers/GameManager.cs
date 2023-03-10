using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GameObject lost;
    public static GameManager Instance;
    public GameState GameState;
    public int Wave;
    [SerializeField] private TextMeshProUGUI score;
    private void Awake(){
        if(Instance != null){
            Destroy(gameObject);
            return;
        }
        else{
            Instance = this;
            lost = GameObject.Find("CanvasParent");
            lost.SetActive(false);
        }
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.R) && GameState == GameState.Lose || Input.GetKeyDown(KeyCode.R) && GameState == GameState.WhitesTurn){
            RestartAfterLose();
        }
    }
    private void RestartAfterLose(){
        lost.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Wave = 0;
        StartCoroutine(Waiting(.3f,0));
    }
    private void Start(){
        ChangeState(GameState.GenerateBoard);
    }
    public void ChangeState(GameState newState){
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateBoard:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.GenerateWhitePiece:
                UnitManager.Instance.SpawnWhitePiece();
                break;
            case GameState.GenerateBlackPiece:
                UnitManager.Instance.SpawnBlackPiece();
                break;
            case GameState.WhitesTurn:
                break;
            case GameState.BlacksTurn:
                foreach(BaseBlackPiece enemy in FindObjectsOfType<BaseBlackPiece>()){
                    enemy.TakenTurn = false;
                }
                StartCoroutine(Waiting(.1f,1));
                break;
            case GameState.Lose:
                lost.SetActive(true);
                score.text = "Score: " + Wave;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Wave++;
        StartCoroutine(Waiting(.3f,0));
    }
    IEnumerator Waiting(float sec, int mode){
        yield return new WaitForSeconds(sec);
        if(mode == 0) ChangeState(GameState.GenerateBoard);
        if(mode == 1) foreach(BaseBlackPiece enemy in FindObjectsOfType<BaseBlackPiece>())
                    {
                        if(!enemy.TakenTurn){
                            enemy.TakeTurn();
                            enemy.TakenTurn = true;
                        }
                    }
    }
    
}
public enum GameState{
    GenerateBoard = 0,
    GenerateWhitePiece = 1,
    GenerateBlackPiece = 2,
    WhitesTurn = 3,
    BlacksTurn = 4,
    Lose = 5
}