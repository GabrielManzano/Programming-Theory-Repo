using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackQueen : BaseBlackPiece
{
    public override void TakeTurn(){
        Debug.Log("Black Takes Turn");
        GameManager.Instance.ChangeState(GameState.WhitesTurn);
    }
}
