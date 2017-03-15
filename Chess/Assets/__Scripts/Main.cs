using UnityEngine;
using System.Collections;
using System.Collections.Generic;



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
    public bool isAlive = true;
    private Color team;
    private int x, y;
    public GameObject prefab;

    public ChessPiece()
    {

    }

    public ChessPiece(int x, int y, GameObject go)
    {
        this.x = x;
        this.y = y;
        prefab = go;
    }

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

}


public class Player
{
    public ChessPiece[] pieces;//array of pieces
    public PlayerType pType;//type of player
    public Vector3 moveTo = Vector3.zero;
    public ChessPiece sPiece;
    //correct offset on board for x and y positions
    //uses the index to arrange pieces on board
    public float[] xPos = { -3.3f, -2.35f, -1.4f, -0.45f, 0.45f, 1.4f, 2.35f, 3.3f };
    public float[] yPos = { 3.25f, 2.30f, 1.35f, 0.45f, -0.45f, -1.35f, -2.30f, -3.25f };

    public Player(ChessPiece[] p, PlayerType pT)
    {
        pieces = p;
        pType = pT;
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
    private bool firstTouched = false;
    private bool turn = true;//true for p1, false for p2
    private List<ChessPiece> potentialMoves;
    private ChessPiece selectedPiece = new ChessPiece();
    private float offBlackX = -3.3f, offBlackY = 6f, offWhiteX = -3.3f, offWhiteY = -6f;
    private float offset = .75f;
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
            if (player1.pType == PlayerType.HUMAN && Input.touchCount > 0)
            {
                checkInput(player1);
            }
        }
        else
        {
            if (player2.pType == PlayerType.HUMAN && Input.touchCount > 0)
            {
                checkInput(player2);
            }
        }
    }

    //players turn
    public void checkInput(Player player)
    {
        bool validPiece = false;
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!firstTouched)
            {
                firstTouched = true;
                //gets the first touch position and converts to Vector
                Vector3 tPos = Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0));

                foreach (ChessPiece p in player.pieces)
                {
                    if (player.xPos[p.X] - .45f <= tPos.x && tPos.x <= player.xPos[p.X] + .45f
                        && player.yPos[p.Y] - .45f <= tPos.y && tPos.y <= player.yPos[p.Y] + .45f && p.isAlive)
                    {
                        validPiece = true;
                        player.sPiece = p;
                    }
                }
                if (validPiece)
                {
                    selectedPiece.prefab = Object.Instantiate(Resources.Load("ChessPieces/Selected", typeof(GameObject)),
                        new Vector3(player.xPos[player.sPiece.X], player.yPos[player.sPiece.Y], -.5f), Quaternion.identity) as GameObject;
                    showMoves(player);
                }
                else
                    firstTouched = false;

            }
            else//second touch
            {
                bool deselect = false;
                Vector3 tPos = Camera.main.ScreenToWorldPoint(
                   new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0));

                foreach (ChessPiece p in player.pieces)
                {
                    if (player.xPos[p.X] - .35f <= tPos.x && tPos.x <= player.xPos[p.X] + .35f
                        && player.yPos[p.Y] - .35f <= tPos.y && tPos.y <= player.yPos[p.Y] + .35f)
                    {
                        if (p.X == player.sPiece.X && p.Y == player.sPiece.Y)//deselect piece
                        {
                            deselect = true;
                            firstTouched = false;
                            //deletes selected piece
                            Destroy(selectedPiece.prefab);
                            //deletes all potential moves
                            foreach(ChessPiece go in potentialMoves)
                                Destroy(go.prefab);
                        }
                    }
                }

                if (!deselect)
                {
                    bool vMove = false;
                    int xP = -1;
                    int yP = -1;
                    foreach(ChessPiece go in potentialMoves)
                    {
                        if(player.xPos[go.X] - .35f <= tPos.x && tPos.x <= player.xPos[go.X] + .35f
                            && player.yPos[go.Y] - .35f <= tPos.y && tPos.y <= player.yPos[go.Y] + .35f)
                        {
                            vMove = true;
                            xP = go.X;
                            yP = go.Y;
                            break;
                        }
                    }

                    if(vMove)
                    {
                        player.sPiece.X = xP;
                        player.sPiece.Y = yP;
                        player.sPiece.prefab.transform.position = new Vector3(player.xPos[player.sPiece.X], player.yPos[player.sPiece.Y], -1);
                        checkCollisions(player);
                        Destroy(selectedPiece.prefab);
                        firstTouched = false;
                        selectedPiece = new ChessPiece();
                        foreach (ChessPiece g in potentialMoves)
                            Destroy(g.prefab);
                        turn = !turn;
                    }
                }

            }

        }
    }

    //movement system
    public void showMoves(Player player)
    {
        potentialMoves = new List<ChessPiece>();
        switch (player.sPiece.getPType())
        {

            case Type.PAWN:
                getPMoves(player, Type.PAWN);
                break;

            case Type.ROOK:
                getPMoves(player, Type.ROOK);
                break;

            case Type.BISHOP:
                getPMoves(player, Type.BISHOP);
                break;

            case Type.KNIGHT:
                getPMoves(player, Type.KNIGHT);
                break;

            case Type.QUEEN:
                getPMoves(player, Type.ROOK);
                getPMoves(player, Type.BISHOP);
                break;

            case Type.KING:
                getPMoves(player, Type.KING);
                break;
        }
    }


    public void getPMoves(Player player, Type t)
    {
        int depth = -1;
        int j;
        bool invalidMove = false;
        switch (t)
        {

            case Type.PAWN:

                if (turn)//player1
                {
                    //checks in front of it
                    foreach (ChessPiece p in player2.pieces)
                    {
                        if (p.isAlive && p.Y == player.sPiece.Y - 1 && p.X == player.sPiece.X)
                            invalidMove = true;
                    }
                    //checks in front of it
                    foreach (ChessPiece p in player1.pieces)
                    {
                        if (p.isAlive && p.Y == player.sPiece.Y - 1 && p.X == player.sPiece.X)
                            invalidMove = true;
                    }
                    if (!invalidMove && player.sPiece.Y - 1 >= 0)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X, player.sPiece.Y - 1, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                            new Vector3(player1.xPos[player.sPiece.X], player1.yPos[(player.sPiece.Y - 1)], -.5f), Quaternion.identity) as GameObject));
                    }
                    //checks 2 spaces up if on initial position
                    if (player.sPiece.Y == 6)
                    {
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.Y == player.sPiece.Y - 2 && p.X == player.sPiece.X)
                                invalidMove = true;
                        }
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.Y == player.sPiece.Y - 2 && p.X == player.sPiece.X)
                                invalidMove = true;
                        }
                        if (!invalidMove)
                        {
                            potentialMoves.Add(new ChessPiece(player.sPiece.X, player.sPiece.Y - 2, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player1.xPos[player.sPiece.X], player1.yPos[(player.sPiece.Y - 2)], -.5f), Quaternion.identity) as GameObject));
                        }
                    }
                    invalidMove = false;
                    //checks diagonal
                    foreach (ChessPiece p in player2.pieces)
                    {
                        if (p.isAlive)
                        {
                            if (p.Y == player.sPiece.Y - 1 && p.X == player.sPiece.X - 1 || p.Y == player.sPiece.Y - 1 && p.X == player.sPiece.X + 1)
                            {
                                potentialMoves.Add(new ChessPiece(p.X, p.Y, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                    new Vector3(player1.xPos[p.X], player1.yPos[p.Y], -.5f), Quaternion.identity) as GameObject));
                            }
                        }
                    }
                }
                else//player 2
                {
                    //checks in front of it
                    foreach (ChessPiece p in player1.pieces)
                    {
                        if (p.isAlive && p.Y == player.sPiece.Y + 1 && p.X == player.sPiece.X)
                            invalidMove = true;
                    }
                    foreach (ChessPiece p in player2.pieces)
                    {
                        if (p.isAlive && p.Y == player.sPiece.Y + 1 && p.X == player.sPiece.X)
                            invalidMove = true;
                    }
                    if (!invalidMove && player.sPiece.Y + 1 <= 7)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X, player.sPiece.Y + 1, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                            new Vector3(player2.xPos[player.sPiece.X], player2.yPos[(player.sPiece.Y + 1)], -.5f), Quaternion.identity) as GameObject));
                    }
                    //checks 2 spaces up if on initial position
                    if (player.sPiece.Y == 1)
                    {
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.Y == player.sPiece.Y + 2 && p.X == player.sPiece.X)
                                invalidMove = true;
                        }
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.Y == player.sPiece.Y + 2 && p.X == player.sPiece.X)
                                invalidMove = true;
                        }
                        if (!invalidMove)
                        {
                            potentialMoves.Add(new ChessPiece(player.sPiece.X, player.sPiece.Y + 2, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player2.xPos[player.sPiece.X], player2.yPos[(player.sPiece.Y + 2)], -.5f), Quaternion.identity) as GameObject));
                        }
                    }
                    invalidMove = false;
                    //checks diagonal
                    foreach (ChessPiece p in player1.pieces)
                    {
                        if (p.isAlive)
                        {
                            if (p.Y == player.sPiece.Y + 1 && p.X == player.sPiece.X + 1 || p.Y == player.sPiece.Y + 1 && p.X == player.sPiece.X - 1)
                            {
                                potentialMoves.Add(new ChessPiece(p.X, p.Y, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                    new Vector3(player1.xPos[p.X], player1.yPos[p.Y], -.5f), Quaternion.identity) as GameObject));
                            }
                        }
                    }
                }

                break;

            case Type.ROOK:
                //move right
                for (int i = player.sPiece.X + 1; i <= 7; i++)
                {
                    foreach (ChessPiece cp in player1.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.X && player.sPiece.Y == cp.Y)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i - 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    foreach (ChessPiece cp in player2.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.X && player.sPiece.Y == cp.Y)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i - 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                }
                //adds potential moves 
                if (depth == -1)
                    depth = 7;
                for (int i = player.sPiece.X + 1; i <= depth; i++)
                    potentialMoves.Add(new ChessPiece(i, player.sPiece.Y, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                        new Vector3(player1.xPos[i], player1.yPos[player.sPiece.Y], -.5f), Quaternion.identity) as GameObject));
                depth = -1;//resets depth

                //move left
                for (int i = player.sPiece.X - 1; i >= 0; i--)
                {
                    foreach (ChessPiece cp in player1.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.X && player.sPiece.Y == cp.Y)
                        {
                            if (depth == -1 || i > depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i + 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    foreach (ChessPiece cp in player2.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.X && player.sPiece.Y == cp.Y)
                        {
                            if (depth == -1 || i > depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i + 1;
                                else
                                    depth = i;
                            }
                        }
                    }

                }
                //adds potential moves 
                if (depth == -1)
                    depth = 0;
                for (int i = player.sPiece.X - 1; i >= depth; i--)
                    potentialMoves.Add(new ChessPiece(i, player.sPiece.Y, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                        new Vector3(player1.xPos[i], player1.yPos[player.sPiece.Y], -.5f), Quaternion.identity) as GameObject));
                depth = -1;//resets depth

                //move down
                for (int i = player.sPiece.Y + 1; i <= 7; i++)
                {
                    foreach (ChessPiece cp in player1.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.Y && player.sPiece.X == cp.X)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i - 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    foreach (ChessPiece cp in player2.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.Y && player.sPiece.X == cp.X)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i - 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                }
                //adds potential moves 
                if (depth == -1)
                    depth = 7;
                for (int i = player.sPiece.Y + 1; i <= depth; i++)
                    potentialMoves.Add(new ChessPiece(player.sPiece.X, i, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                        new Vector3(player1.xPos[player.sPiece.X], player1.yPos[i], -.5f), Quaternion.identity) as GameObject));
                depth = -1;//resets depth


                //move up
                for (int i = player.sPiece.Y - 1; i >= 0; i--)
                {
                    foreach (ChessPiece cp in player1.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.Y && player.sPiece.X == cp.X)
                        {
                            if (depth == -1 || i > depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i + 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    foreach (ChessPiece cp in player2.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.Y && player.sPiece.X == cp.X)
                        {
                            if (depth == -1 || i > depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i + 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                }
                //adds potential moves 
                if (depth == -1)
                    depth = 0;
                for (int i = player.sPiece.Y - 1; i >= depth; i--)
                    potentialMoves.Add(new ChessPiece(player.sPiece.X, i, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                        new Vector3(player1.xPos[player.sPiece.X], player1.yPos[i], -.5f), Quaternion.identity) as GameObject));

             break;

            case Type.BISHOP:
                j = player.sPiece.Y - 1;
                depth = -1;
                //move up/right
                for (int i = player.sPiece.X + 1; i <= 7 && j >= 0; i++)
                {

                    foreach (ChessPiece cp in player1.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.X && j == cp.Y)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i - 1;
                                else
                                    depth = i;
                            }
                        }
                    }

                    foreach (ChessPiece cp in player2.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.X && j == cp.Y)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i - 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    j--;
                }
                //adds potential moves 
                if (depth == -1 && player.sPiece.Y != 0)
                    depth = 7;
                j = player.sPiece.Y - 1;
                for (int i = player.sPiece.X + 1; i <= depth && j >= 0; i++)
                    potentialMoves.Add(new ChessPiece(i, j, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                        new Vector3(player1.xPos[i], player1.yPos[j--], -.5f), Quaternion.identity) as GameObject));
                depth = -1;//resets depth
                j = player.sPiece.Y - 1;

                //move left/up
                for (int i = player.sPiece.X - 1; i >= 0 && j >= 0; i--)
                {
                    foreach (ChessPiece cp in player1.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.X && j == cp.Y)
                        {
                            if (depth == -1 || i > depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i + 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    foreach (ChessPiece cp in player2.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.X && j == cp.Y)
                        {
                            if (depth == -1 || i > depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i + 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    j--;
                }
                //adds potential moves 
                if (depth == -1 && player.sPiece.Y != 0)
                    depth = 0;
                j = player.sPiece.Y - 1;
                for (int i = player.sPiece.X - 1; i >= depth && j >= 0; i--)
                    potentialMoves.Add(new ChessPiece(i, j, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                        new Vector3(player1.xPos[i], player1.yPos[j--], -.5f), Quaternion.identity) as GameObject));
                depth = -1;//resets depth
                j = player.sPiece.X + 1;

                //move down/right
                for (int i = player.sPiece.Y + 1; i <= 7 && j <= 7; i++)
                {
                    foreach (ChessPiece cp in player1.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.Y && j == cp.X)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i - 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    foreach (ChessPiece cp in player2.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.Y && j == cp.X)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i - 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    j++;
                }
                //adds potential moves 
                if (depth == -1 && player.sPiece.X != 7)
                    depth = 7;
                j = player.sPiece.X + 1;
                for (int i = player.sPiece.Y + 1; i <= depth && j <= 7; i++)
                    potentialMoves.Add(new ChessPiece(j, i, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                        new Vector3(player1.xPos[j++], player1.yPos[i], -.5f), Quaternion.identity) as GameObject));
                depth = -1;//resets depth
                j = player.sPiece.X - 1;

                //move down/left
                for (int i = player.sPiece.Y + 1; i <= 7 && j >= 0; i--)
                {
                    foreach (ChessPiece cp in player1.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.Y && j == cp.X)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i + 1;
                                else
                                    depth = i;
                            }
                        }

                    }
                    foreach (ChessPiece cp in player2.pieces)
                    {
                        //checks if a piece is blocking path
                        if (cp.isAlive && i == cp.Y && j == cp.X)
                        {
                            if (depth == -1 || i < depth)
                            {
                                //if pieces are the same color
                                if (player.sPiece.getColor() == cp.getColor())
                                    depth = i + 1;
                                else
                                    depth = i;
                            }
                        }
                    }
                    j--;
                }
                //adds potential moves 
                if (depth == -1 && player.sPiece.X != 0)
                    depth = 7;
                j = player.sPiece.X - 1;
                Debug.Log(depth);
                for (int i = player.sPiece.Y + 1; i <= depth && j >= 0; i++)
                    potentialMoves.Add(new ChessPiece(j, i, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                        new Vector3(player1.xPos[j--], player1.yPos[i], -.5f), Quaternion.identity) as GameObject));
                break;

            case Type.KNIGHT:
                if (player.sPiece.X - 2 >= 0 && player.sPiece.Y + 1 <= 7)
                {
                    if (turn)
                    {
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X - 2 && p.Y == player.sPiece.Y + 1)
                                invalidMove = true;
                        }
                    }
                    else
                    {
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X - 2 && p.Y == player.sPiece.Y + 1)
                                invalidMove = true;
                        }
                    }

                    if (!invalidMove)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X - 2, player.sPiece.Y + 1, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player1.xPos[player.sPiece.X - 2], player1.yPos[player.sPiece.Y + 1], -.5f), Quaternion.identity) as GameObject));
                    }
                }
                invalidMove = false;


                if (player.sPiece.X - 2 >= 0 && player.sPiece.Y - 1 >= 0)
                {
                    if (turn)
                    {
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X - 2 && p.Y == player.sPiece.Y - 1)
                                invalidMove = true;
                        }
                    }
                    else
                    {
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X - 2 && p.Y == player.sPiece.Y - 1)
                                invalidMove = true;
                        }
                    }

                    if (!invalidMove)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X - 2, player.sPiece.Y - 1, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player1.xPos[player.sPiece.X - 2], player1.yPos[player.sPiece.Y - 1], -.5f), Quaternion.identity) as GameObject));
                    }
                }
                invalidMove = false;

                if (player.sPiece.X + 2 <= 7 && player.sPiece.Y - 1 >= 0)
                {
                    if (turn)
                    {
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X + 2 && p.Y == player.sPiece.Y - 1)
                                invalidMove = true;
                        }
                    }
                    else
                    {
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X + 2 && p.Y == player.sPiece.Y - 1)
                                invalidMove = true;
                        }
                    }

                    if (!invalidMove)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X + 2, player.sPiece.Y - 1, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player1.xPos[player.sPiece.X + 2], player1.yPos[player.sPiece.Y - 1], -.5f), Quaternion.identity) as GameObject));
                    }
                }
                invalidMove = false;

                if (player.sPiece.X + 2 <= 7 && player.sPiece.Y + 1 <= 7)
                {
                    if (turn)
                    {
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X + 2 && p.Y == player.sPiece.Y + 1)
                                invalidMove = true;
                        }
                    }
                    else
                    {
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X + 2 && p.Y == player.sPiece.Y + 1)
                                invalidMove = true;
                        }
                    }

                    if (!invalidMove)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X + 2, player.sPiece.Y + 1, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player1.xPos[player.sPiece.X + 2], player1.yPos[player.sPiece.Y + 1], -.5f), Quaternion.identity) as GameObject));
                    }
                }
                invalidMove = false;

                if (player.sPiece.X + 1 <= 7 && player.sPiece.Y + 2 <= 7)
                {
                    if (turn)
                    {
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X + 1 && p.Y == player.sPiece.Y + 2)
                                invalidMove = true;
                        }
                    }
                    else
                    {
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X + 1 && p.Y == player.sPiece.Y + 2)
                                invalidMove = true;
                        }
                    }

                    if (!invalidMove)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X + 1, player.sPiece.Y + 2, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player1.xPos[player.sPiece.X + 1], player1.yPos[player.sPiece.Y + 2], -.5f), Quaternion.identity) as GameObject));
                    }
                }
                invalidMove = false;

                if (player.sPiece.X - 1 >= 0 && player.sPiece.Y + 2 <= 7)
                {
                    if (turn)
                    {
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X - 1 && p.Y == player.sPiece.Y + 2)
                                invalidMove = true;
                        }
                    }
                    else
                    {
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X - 1 && p.Y == player.sPiece.Y + 2)
                                invalidMove = true;
                        }
                    }

                    if (!invalidMove)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X - 1, player.sPiece.Y + 2, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player1.xPos[player.sPiece.X - 1], player1.yPos[player.sPiece.Y + 2], -.5f), Quaternion.identity) as GameObject));
                    }
                }
                invalidMove = false;

                if (player.sPiece.X + 1 <= 7 && player.sPiece.Y - 2 >= 0)
                {
                    if (turn)
                    {
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X + 1 && p.Y == player.sPiece.Y - 2)
                                invalidMove = true;
                        }
                    }
                    else
                    {
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X + 1 && p.Y == player.sPiece.Y - 2)
                                invalidMove = true;
                        }
                    }

                    if (!invalidMove)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X + 1, player.sPiece.Y - 2, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player1.xPos[player.sPiece.X + 1], player1.yPos[player.sPiece.Y - 2], -.5f), Quaternion.identity) as GameObject));
                    }
                }
                invalidMove = false;

                if (player.sPiece.X - 1 >= 0 && player.sPiece.Y - 2 >= 0)
                {
                    if (turn)
                    {
                        foreach (ChessPiece p in player1.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X - 1 && p.Y == player.sPiece.Y - 2)
                                invalidMove = true;
                        }
                    }
                    else
                    {
                        foreach (ChessPiece p in player2.pieces)
                        {
                            if (p.isAlive && p.X == player.sPiece.X - 1 && p.Y == player.sPiece.Y - 2)
                                invalidMove = true;
                        }
                    }

                    if (!invalidMove)
                    {
                        potentialMoves.Add(new ChessPiece(player.sPiece.X - 1, player.sPiece.Y - 2, Object.Instantiate(Resources.Load("ChessPieces/PotentialMoves", typeof(GameObject)),
                                new Vector3(player1.xPos[player.sPiece.X - 1], player1.yPos[player.sPiece.Y - 2], -.5f), Quaternion.identity) as GameObject));
                    }
                }
                invalidMove = false;
                break;


            case Type.KING:

                break;

        }
    }

    public void checkCollisions(Player player)
    {
        if(turn)
        {
            foreach(ChessPiece cp in player2.pieces)
            {
                if(player.sPiece.X == cp.X && player.sPiece.Y == cp.Y)
                {
                    cp.isAlive = false;
                    moveOffBoard(cp);
                }
            }
        }
        else
        {
            foreach (ChessPiece cp in player1.pieces)
            {
                if (cp.isAlive && player.sPiece.X == cp.X && player.sPiece.Y == cp.Y)
                {
                    cp.isAlive = false;
                    moveOffBoard(cp);
                }
            }
        }
    }

    public void moveOffBoard(ChessPiece cp)
    {
        if(cp.getColor() == Color.BLACK)
        {
            cp.prefab.transform.position = new Vector3(offBlackX, offBlackY);
            if(offBlackX < 3.3f)
                offBlackX += offset;
            else
            {
                offBlackX = -3.3f;
                offBlackY -= offset;
            }
        }
        else
        {
            cp.prefab.transform.position = new Vector3(offWhiteX, offWhiteY);
            if (offWhiteX < 3.3f)
                offWhiteX += offset;
            else
            {
                offWhiteX = -3.3f;
                offWhiteY += offset;
            }
        }
    }


}
