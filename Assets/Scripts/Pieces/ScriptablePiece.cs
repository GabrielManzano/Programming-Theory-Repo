using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Piece")]
public class ScriptablePiece : ScriptableObject
{
    public PieceColor PieceColor;
    public Piece PiecePrefab;
}
public enum PieceColor{
    White = 0,
    Black = 1
}
