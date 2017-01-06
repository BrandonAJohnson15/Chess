using System;

namespace ChessGame
{
	public class ChessPiece
	{
		private Type type;
		private bool isAlive;
		private bool capturedPiece;


		public ChessPiece ()
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

	public class game
	{
		private bool gameOver;
			
	}

	public void Main(String[] args)
	{
		enum{PAWN, KNIGHT, ROOK, BISHOP, QUEEN, KING}






		//endGame -GameOver-
		if(capturedPiece.getType() == Type.KING)
		{
			gameOver = true;
		}
	}


}

