using UnityEngine;
using System.Collections;


enum Type { PAWN, KNIGHT, ROOK, BISHOP, QUEEN, KING }
enum Color { BLACK, WHITE }
enum PlayerType {  HUMAN, COMPUTER }

public class ChessPiece
{
    private Type type;
    private bool isAlive;
    private Color team;

    public ChessPiece()
    {

    }

    //movement system
    void move()
    {
        switch (type)
        {
            case Type.PAWN:
                //stuff
                break;

            case Type.ROOK:
                //stuff
                break;

            case Type.BISHOP:
                //stuff
                break;

            case Type.KNIGHT:
                //stuff
                break;

            case Type.QUEEN:
                //stuff
                break;

            case Type.KING:
                //stuff
                break;
        }
    }
}


public class Player
{
    ChessPiece[] pieces;//array of pieces
    PlayerType pType;//type of player


    //players turn
    public void turn()
    {

    }

}

public class Main : MonoBehaviour
{
    private bool gameOver;
    private Player player1;
    private Player player2;

    // Use this for initialization
	void Start ()
    {
	    //initialize both players, pieces and call the function to start the game
	}
	

}
