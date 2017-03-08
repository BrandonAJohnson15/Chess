using UnityEngine;
using System.Collections;



//enum for types of pieces
public enum Type { PAWN, KNIGHT, ROOK, BISHOP, QUEEN, KING }
//enum for teams
public enum Color { BLACK, WHITE }
//enum for type of player
public enum PlayerType {  HUMAN, COMPUTER }

//class for each piece
public class ChessPiece
{
    private Type type;
    private bool isAlive;
    private Color team;
    private int x, y;
    public GameObject prefab;

    public ChessPiece(Type t, Color c, int x, int y)
    {
        type = t;
        team = c;
        this.x = x;
        this.y = y;
    }

    //getter and setter for X 
    public int X
    {
        get
        {
            return this.x;
        }
        set
        {
            this.x = value;
        }
    }
    //getter and setter for Y
    public int Y
    {
        get
        {
            return this.y;
        }
        set
        {
            this.y = value;
        }
    }

    //returns the type of player
    public Type getPType()
    {
        return type;
    }

    //returns the color
    public Color getColor()
    {
        return team;
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
    public ChessPiece[] pieces;//array of pieces
    public PlayerType pType;//type of player
    private bool firstTouched = false;
    //correct offset on board for x and y positions
    //uses the index to arrange pieces on board
    public float[] xPos = { -3.3f, -2.35f, -1.4f, -0.45f, 0.45f, 1.4f, 2.35f, 3.3f };
    public float[] yPos = { 3.25f, 2.30f, 1.35f, 0.45f, -0.45f, -1.35f, -2.30f, -3.25f };

    public Player(ChessPiece[] p, PlayerType pT)
    {
        pieces = p;
        pType = pT;
    }

    //players turn
    public void checkInput()
    {
        if (!firstTouched)
        {
            firstTouched = true;
            //gets the first touch position and converts to Vector
            Vector3 tPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0));
            //Debug.Log("x: " + tPos.x + " y: " + tPos.y);

            foreach (ChessPiece p in pieces)
                if (xPos[p.X] - .2f <= tPos.x && tPos.x <= xPos[p.X] + .2f
                        && yPos[p.Y] - .1f <= tPos.y && tPos.y <= yPos[p.Y] + .1f)
                    Debug.Log(p.X + " " + p.Y + " " + p.getPType());
            firstTouched = false;
        }
    }



    public void initializeBoard()
    {
        //loops through each piece to set up on board
        for (int i = 0; i < pieces.Length; i++)
        {
            string color = (pieces[i].getColor() == Color.WHITE) ? "White" : "Black";
            string t = "";
            switch(pieces[i].getPType())
            {
                case Type.PAWN:
                    t = "Pawn";
                    break;
                case Type.BISHOP:
                    t = "Bishop";
                    break;
                case Type.KING:
                    t = "King";
                    break;
                case Type.KNIGHT:
                    t = "Knight";
                    break;
                case Type.QUEEN:
                    t = "Queen";
                    break;
                case Type.ROOK:
                    t = "Rook";
                    break;
            }
            Debug.Log(color + t);

            //sets the prefab to the piece within the resources folder
            pieces[i].prefab = Object.Instantiate(Resources.Load("ChessPieces/Piece", typeof(GameObject)), 
                new Vector3(xPos[pieces[i].X], yPos[pieces[i].Y], -1), Quaternion.identity) as GameObject;
            //gets the correct sprite from the resources folder
            pieces[i].prefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ChessPieces/" + color + t);
        }
    }

}


//main class initializes the game
public class Main : MonoBehaviour
{
    private bool gameOver;
    private Player player1;
    private Player player2;
    private bool turn = true;//true for p1, false for p2

    // Use this for initialization
	void Start ()
    {
        //creates array of pieces for player 1
        ChessPiece[] p1Pieces = { new ChessPiece(Type.PAWN, Color.WHITE, 0,6), new ChessPiece(Type.PAWN, Color.WHITE,1,6),
            new ChessPiece(Type.PAWN, Color.WHITE,2,6), new ChessPiece(Type.PAWN, Color.WHITE,3,6), new ChessPiece(Type.PAWN, Color.WHITE,4,6),
            new ChessPiece(Type.PAWN, Color.WHITE,5,6), new ChessPiece(Type.PAWN, Color.WHITE, 6,6), new ChessPiece(Type.PAWN, Color.WHITE, 7,6),
            new ChessPiece(Type.ROOK, Color.WHITE, 0,7), new ChessPiece(Type.KNIGHT, Color.WHITE,1,7), new ChessPiece(Type.BISHOP, Color.WHITE,2,7),
            new ChessPiece(Type.KING, Color.WHITE,3,7), new ChessPiece(Type.QUEEN, Color.WHITE,4,7), new ChessPiece(Type.BISHOP, Color.WHITE,5,7),
            new ChessPiece(Type.KNIGHT, Color.WHITE, 6,7), new ChessPiece(Type.ROOK, Color.WHITE, 7,7)
        };

        //creates array of pieces for player 2
        ChessPiece[] p2Pieces = { new ChessPiece(Type.PAWN, Color.BLACK, 0,1), new ChessPiece(Type.PAWN, Color.BLACK,1,1),
            new ChessPiece(Type.PAWN, Color.BLACK,2,1), new ChessPiece(Type.PAWN, Color.BLACK,3,1), new ChessPiece(Type.PAWN, Color.BLACK,4,1),
            new ChessPiece(Type.PAWN, Color.BLACK,5,1), new ChessPiece(Type.PAWN, Color.BLACK, 6,1), new ChessPiece(Type.PAWN, Color.BLACK, 7,1),
            new ChessPiece(Type.ROOK, Color.BLACK, 0,0), new ChessPiece(Type.KNIGHT, Color.BLACK,1,0), new ChessPiece(Type.BISHOP, Color.BLACK,2,0),
            new ChessPiece(Type.KING, Color.BLACK,3,0), new ChessPiece(Type.QUEEN, Color.BLACK,4,0), new ChessPiece(Type.BISHOP, Color.BLACK,5,0),
            new ChessPiece(Type.KNIGHT, Color.BLACK, 6,0), new ChessPiece(Type.ROOK, Color.BLACK, 7,0)
        };
        

        //initializes players
        player1 = new Player(p1Pieces, PlayerType.HUMAN);
        player2 = new Player(p2Pieces, PlayerType.HUMAN);
        //places piece on the board
        player1.initializeBoard();
        player2.initializeBoard();

    }

    void Update()
    {
        if (turn)
        {
            if(player1.pType == PlayerType.HUMAN && Input.touchCount > 0 )
                player1.checkInput();
        }
        else
            if (player2.pType == PlayerType.HUMAN && Input.touchCount > 0)
                player2.checkInput();
    }
	

}
