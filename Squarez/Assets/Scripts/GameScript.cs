/*

                                                        o                       
                                                        oOo                     
                                               o         oOOo                   
                                             ooo         oO@Oo                  
                                    oo     oO@@@Oo      oO@@@Oo                 
                          ooooOOOOOOo     oO@@@@@@OOooOO@@@@@@Oo                
                     ooOOOOOOOOOOOOo     oO@@@@@@@@@@@@@@@@@@@Oo                
                 ooOO@@@@@@@@@@@@@Oo     oO@@@@@@@@@@@@@@@@@@@Oo                
              oOO@@@@@@@@@@@@@@@@@Oo     oO@@@@@@@@@@@@@@@@@@@Oo                
           oOO@@@@@@@@@@@@@@@@@@@@@Oo     oOOO@@@@@@@@@@@@@OOOo                 
         oO@@@@@@@@@@@@@@@@@@@@@@@@@Oo       oOOOOOOOOOOOOOo                   
       oO@@@@@@@@@@@@@@@@@@@Oo                                                  
      O@@@@@@@@@@@@@@@@Oo                                                       
    oO@@@@@@@@@@@@@@Oo                                                          
   oO@@@@@@@@@@@@@Oo                  oOOOOOOo                                  
  oO@@@@@@@@@@@@@Oo              oOOOO@@@@@@@@OOOo                              
  oO@@@@@@@@@@@Oo              oO@@@@@@@@@@@@@@@@@Oo                            
 oO@@@@@@@@@@@Oo              oO@@@@@@@@@@@@@@@@@@@Oo                           
 oO@@@@@@@@@@Oo               oO@@@@@@@@@@@@@@@@@@@Oo                           
 oO@@@@@@@@@@Oo               oO@@@@@@@@@@@@@@@@@@@Oo      Oo                   
oO@@@@@@@@@@@Oo               oO@@@@@@@@@@@@@@@@@@@Oo     oOOOOOOOo            
 oO@@@@@@@@@@Oo                 oO@@@@@@@@@@@@@@@@Oo     oO@@@@@@@OOOOOOOo      
 oO@@@@@@@@@@Oo                   oOOOO@@@@@@OOOo       oO@@@@@@@@@@@@@@@OOOOOOo
 oO@@@@@@@@@@@Oo                       oOOOOo            oO@@@@@@@@OOOOOOo      
  oO@@@@@@@@@@@Oo                                         oOOOOOOOOo            
  oO@@@@@@@@@@@@Oo                                         Oo                   
   oO@@@@@@@@@@@@@Oo                                 oo                         
    oO@@@@@@@@@@@@@@Oo                                oOOo                      
      oO@@@@@@@@@@@@@@@Oo                             oOOOo                     
       oO@@@@@@@@@@@@@@@@@Oo                         oO@@@@Oo                   
         oO@@@@@@@@@@@@@@@@@@@@@@@Oo               oO@@@@@@@Oo                  
           oO@@@@@@@@@@@@@@@@@@@@@@@@@@@@@Oo     oO@@@@@@@@@@Oo                 
              oO@@@@@@@@@@@@@@@@@@@@@@@@@Oo    oO@@@@@@@@@@@@Oo                 
                 ooOO@@@@@@@@@@@@@@@@@@@Oo     oO@@@@@@@@@@@@@Oo                
                     ooOO@@@@@@@@@@@@@@@@Oo    oO@@@@@@@@@@@@@Oo                
                          ooooOOOOO@@@@@@Oo     oO@@@@@@@@@@@Oo                 
                                   oooooOOOoo      oOOOOOOoO 
                                    
Copyright 2020, Joshua Wereszczuk, All rights reserved.

This file was writen by Joshua Wereszczuk and the following restrictions are in place. You may not use the Code 
for any commercial purpose except as expressly provided for herein. You may not sell, sub-license, rent, lease, 
lend, assign or otherwise transfer, duplicate or otherwise reproduce, directly or indirectly, the Code in whole 
or in part. Merge OEM may terminate this Agreement immediately upon notice to Customer.

*/

// Setting Namespaces
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
	// Serializing Variables for quick editing in editor
	[SerializeField] GameObject backBoard;
	[SerializeField] GameObject tileSpace;
	[SerializeField] GameObject[] tile;
	[SerializeField] Color wallColour;
	[SerializeField] Text scoreText;
	[SerializeField] Text gameOverText;

	// Creating Global Variables for functions
	GameObject[] tileSpaces;
	BoardSpace[] boardSpaces;

	List<GameObject> objects;
	List<List<GameObject>> totalObjects;

	int score;
	int boardSize;
	public bool canSwipe = true;

	int threesCount = 0;

	// Creating Board Class to keep things organised 
	public class BoardSpace
	{
		// Constructor that takes no arguments:
		public BoardSpace()
		{
			spaceOccupied = false;
			isWallTile = false;
		}

		// Constructor that takes one argument:
		public BoardSpace(bool value, bool value2)
		{
			spaceOccupied = value;
			isWallTile = value2;
		}

		// Auto-implemented readonly property:
		public bool spaceOccupied { get; set; }
		public bool isWallTile { get; set; }
	}

	private void Start()
	{
		// Creating new lists at the start
		objects = new List<GameObject>();
		totalObjects = new List<List<GameObject>>();

		// Sets GameOver text to off

		gameOverText.enabled = false;

		// Set up Board

		TesterBoard();

	}

	// Spawning the dynamic board

	public void SpawnBoard(int size, int[] wallTiles)
	{
		// Setting up variables 

		boardSize = size;

		GameObject backBoardObj = Instantiate(backBoard, this.transform);

		tileSpaces = new GameObject[boardSize * boardSize];
		boardSpaces = new BoardSpace[boardSize * boardSize];

		float startPosX = -0.5f;
		float startPosY = 0.5f;

		// Looping through board positions and creating/assigning them to variables

		for (int i = 0; i < boardSize * boardSize; i++)
		{ 
			// Creates and Scales tile
			tileSpaces[i] = Instantiate(tileSpace, backBoardObj.transform);

			tileSpaces[i].transform.localScale = Vector2.one / boardSize / 1.25f;

			// Finds out if tile should be wrapped around to the other side

			if (startPosX > 0.5f - 1f / boardSize * 0.25f * 2) { startPosX -= 1; startPosY -= 1f / boardSize; }

			// Positions tile in correct spot on grid

			tileSpaces[i].transform.position = new Vector2(startPosX + (1f / boardSize * 0.25f * 2), startPosY - (1f / boardSize * 0.25f * 2));

			// Adds a tile width to the start of next tile

			startPosX += 1f / boardSize;

			// Adds boardspace to list

			boardSpaces[i] = new BoardSpace(false, false);
		}

		// Finds out if there should be a wall

		if (wallTiles.Length != 0)
		{

			// Loops through walls

			foreach (int i in wallTiles)
			{
				//Changes Variables to make boardspace into a wall

				boardSpaces[i].isWallTile = true;
				tileSpaces[i].GetComponent<SpriteRenderer>().color = wallColour;
			}
		}

		// Scales Backboard
		backBoardObj.transform.localScale = Vector2.one * 4;
	}

	// Start of the movement script

	public void MoveTiles(SwipeDetention.DraggedDir draggedDir)
	{
		// Finds out which direction was inputed

		if (!canSwipe) return;

		switch (draggedDir) 
		{
			case SwipeDetention.DraggedDir.Down:

				// loops through all board positions in the game from left to right, bottom to top

				for (int i = boardSize * (boardSize - 1); i >= 0; i--)
				{

					// Calculates movement for tile position

					CalculateMovement(i, draggedDir);
				}

				break;
			case SwipeDetention.DraggedDir.Left:

				// loops through all board positions in the game from top to bottom, left to right

				for (int i = 1; i <= boardSize; i++)
				{
					for (int e = 0; e < boardSize; e++)
					{

						// Calculates movement for tile position

						CalculateMovement(i + (boardSize * e) - 1, draggedDir);
					}
				}

				break;
			case SwipeDetention.DraggedDir.Right:

				// loops through all board positions in the game from bottom to top, right to left

				for (int i = boardSize - 1; i > 0; i--)
				{
					for (int e = 0; e < boardSize; e++)
					{

						// Calculates movement for tile position

						CalculateMovement(i + (boardSize * e) - 1, draggedDir);
					}
				}

				break;
			case SwipeDetention.DraggedDir.Up:

				// loops through all board positions in the game from top to bottom

				for (int i = boardSize; i <= boardSize * boardSize - 1; i++)
				{

					// Calculates movement for tile position

					CalculateMovement(i, draggedDir);
				}

				break;
		}
	}

	private void CalculateMovement(int tile, SwipeDetention.DraggedDir draggedDir)
	{
		// Setting Needed Varibales

		float moved = 0f;
		int validspot = tile;

		// Seeing if there is a tile in the boardspace if not skipping everything

		if (tileSpaces[tile].transform.childCount == 0) return;

		// Finding direction again

		switch (draggedDir)
		{
			case SwipeDetention.DraggedDir.Down:

				// Looping through possible positions

				for (int i = 1; i < boardSize; i++)
				{
					// Adding boardsize to check tile under the current tile

					int checktile = tile + boardSize * i;

					// seeing if the checked tile is on the board

					if (checktile >= boardSize * boardSize) break;

					// checking if the space being check is occupied or is a wall

					if (boardSpaces[checktile].spaceOccupied || boardSpaces[checktile].isWallTile)
					{
						break;
					}

					// making the checked space valid for movement

					moved++;
					validspot = checktile;
				}

				break;
			case SwipeDetention.DraggedDir.Left:

				// Looping through possible positions

				for (int i = 1; i < boardSize; i++)
				{
					// Taking away 1 to check the tile to the left of the current space

					int checktile = tile - i;

					// checking if the checked space wraps around the board

					if ((checktile + 1) % boardSize == 0) break;

					// checking if the space being check is occupied or is a wall

					if (boardSpaces[checktile].spaceOccupied || boardSpaces[checktile].isWallTile)
					{
						break;
					}

					// making the checked space valid for movement

					moved++;
					validspot = checktile;
				}

				break;
			case SwipeDetention.DraggedDir.Right:

				// Looping through possible positions

				for (int i = 1; i < boardSize; i++)
				{

					// Adding 1 to check the tile to the left of the current space

					int checktile = tile + i;

					// checking if the space being check is occupied or is a wall

					if (boardSpaces[checktile].spaceOccupied || boardSpaces[checktile].isWallTile)
					{
						break;
					}

					// making the checked space valid for movement

					moved++;
					validspot = checktile;

					// checking if the checked space wraps around the board

					if ((checktile + 1) % boardSize == 0)
					{
						break;
					}
				}

				break;
			case SwipeDetention.DraggedDir.Up:

				// Looping through possible positions

				for (int i = 1; i < boardSize; i++)
				{

					// Minusing the board size to check the tile above the current space

					int checktile = tile - boardSize * i;

					// Checking if the checked space is on the board

					if (checktile < 0) break;

					// checking if the space being check is occupied or is a wall

					if (boardSpaces[checktile].spaceOccupied || boardSpaces[checktile].isWallTile)
					{
						break;
					}

					// making the checked space valid for movement

					moved++;
					validspot = checktile;
				}
				break;
		}

		// Seeing if the valid spot in the original spot

		if (validspot != tile)
		{

			// Setting varibales to account for the movement of the tiles

			canSwipe = false;
			boardSpaces[tile].spaceOccupied = false;
			boardSpaces[validspot].spaceOccupied = true;

			// switching the parent of the tile

			GameObject movingTile = tileSpaces[tile].transform.GetChild(0).gameObject;
			movingTile.transform.SetParent(tileSpaces[validspot].transform);

			// Move the tile with a free tweening libary to animate it to its parents position

			LeanTween.moveLocal(movingTile, Vector3.zero, moved / (2.5f * boardSize)).setEaseOutQuad().setOnComplete(TileFinish);
		}
	}

	// Called to spawn a tile

	public void SpawnTile(bool isCheck)
	{

		// Setting required varbiables 

		int randint = Random.Range(0, boardSize * boardSize);
		int original = randint;

		// Loop to check if something equal to or below random spot isnt occupied
		
		for (int i = randint; i >= 0; i--)
		{
			if (!boardSpaces[i].spaceOccupied && !boardSpaces[i].isWallTile)
			{

				// Sets randint to i

				randint = i;
				break;
			}
		}

		// Checks if there are free spaces above the random space 

		if (boardSpaces[randint].spaceOccupied == true)
		{
			for (int i = boardSize * boardSize - 1; i > original; i--)
			{
				if (!boardSpaces[i].spaceOccupied && !boardSpaces[i].isWallTile)
				{

					// sets randint to the free spot

					randint = i;
					break;
				}
			}
		}

		// finds out if there were any free spaces

		if (!boardSpaces[randint].spaceOccupied && !boardSpaces[randint].isWallTile)
		{
			// Instaniates tile on the free spot whilst picking a random color

			boardSpaces[randint].spaceOccupied = true;
			//Instantiate(tile[Random.Range(0,3)], tileSpaces[randint].transform);
			if (isCheck) LeanTween.scale(Instantiate(tile[Random.Range(0, 3)], tileSpaces[randint].transform), Vector3.one, 0.25f).setEaseOutQuad().setOnComplete(Checks);
			else LeanTween.scale(Instantiate(tile[Random.Range(0, 3)], tileSpaces[randint].transform), Vector3.one, 0.25f).setEaseOutQuad();
		}
	}

	// Called by the tween after it is finished

	private void TileFinish()
	{

		// When there is no more moving tiles

		if (LeanTween.tweensRunning == 0)
		{
			// Starts check for combinations

			Checks();

			// Spawns a new tile

			SpawnTile(true);

		}
	}

 	private void Checks()
    {
		// Starts check for combinations

		CheckTiles();

		// Checks if there are no tiles on the board

		CheckForZero();

		// Checks if game is over

		CheckGameOver();

		// allows the player to swipe

		canSwipe = true;
	}

	private void CheckTiles()
	{

		// clear lists

		threesCount = 0;
		totalObjects.Clear();

		// Loops through all board spaces horizontally

		for (int i = 0; i < boardSize; i++)
		{
			// Clear list

			objects.Clear();

			for (int i2 = 0; i2 < boardSize; i2++)
			{
				// Check for Occupied slot or wall tile

				if(!boardSpaces[i * boardSize + i2].spaceOccupied || boardSpaces[i * boardSize + i2].isWallTile)
				{
					// Check if there was a three alread found

					if (objects.Count < 3)
					{

						// Clear list

						objects.Clear();
					}
					else
					{

						// Add the tiles into a list to count later and clear the old list

						totalObjects.Add(new List<GameObject>(objects));
						threesCount++;
						objects.Clear();
					}
				} 
				else
				{

					// make a variable equal to the tile found

					GameObject t1 = tileSpaces[i * boardSize + i2].transform.GetChild(0).gameObject;

					// check if its the first tile in the collection

					if (objects.Count != 0)
					{

						// Compare tiles tag to the first in the list

						if (t1.CompareTag(objects[0].tag))
						{

							// adds tile to list

							objects.Add(t1);
						}
						else
						{

							// deletes list

							if (objects.Count >= 3)
							{
								// adds collection to list if over 3

								totalObjects.Add(new List<GameObject>(objects));
								threesCount++;
							}
							objects.Clear();
							objects.Add(t1);
						}
					}
					else
					{

						// adds tile to list
					
						objects.Add(t1);
					}
				}

			}

			// Checks if tiles collection is more then 3

			if (objects.Count < 3)
			{

				// removes list

				objects.Clear();
			}
			else
			{
				// Add the tiles into a list to count later and clear the old list
		
				totalObjects.Add(new List<GameObject>(objects));
				threesCount++;
				objects.Clear();
			}
		}

		// Checks all board spaces vertically

		for (int i = 0; i < boardSize; i++)
		{
			// Clears the list

			objects.Clear();

			for (int i2 = 0; i2 < boardSize; i2++)
			{

				// Checks if the board space is not occupied or is a wall

				if (!boardSpaces[i + (boardSize * i2)].spaceOccupied || boardSpaces[i + (boardSize * i2)].isWallTile)
				{

					// Checks if the collection is less then 3

					if (objects.Count < 3)
					{
						// Clears list

						objects.Clear();
					}
					else
					{
						// Add the tiles into a list to count later and clear the old list

						totalObjects.Add(new List<GameObject>(objects));
						threesCount++;
						objects.Clear();
					}
				}
				else
				{
					// Makes a variable equal to the found tile

					GameObject t1 = tileSpaces[i + (boardSize * i2)].transform.GetChild(0).gameObject;

					// Checks whether its the first of the collection

					if (objects.Count != 0)
					{

						// Checks if the tile tags matchup

						if (t1.CompareTag(objects[0].tag))
						{
							// adds tile to list
						
							objects.Add(t1);
						}
						else
						{
							// clears list

							if (objects.Count >= 3)
							{
								// adds collection to list if over 3
								totalObjects.Add(new List<GameObject>(objects));
								threesCount++;
							}
							objects.Clear();
							objects.Add(t1);
						}
					}
					else
					{
						// adds tile to the list

						objects.Add(t1);
					}
				}

			}

			// checks if the collection has at least 3 in it

			if (objects.Count < 3)
			{
				// Clears the list

				objects.Clear();
			}
			else
			{
				// Add the tiles into a list to count later and clear the old list

				totalObjects.Add(new List<GameObject>(objects));
				threesCount++;
				objects.Clear();
			}
		}

		// Finds the connected collections 

		findConnected();

		// loops through all objects in the list

		foreach (List<GameObject> i in totalObjects)
		{
			// Adds the relevent score to the score board

			score += 3 + ((i.Count - 3) * 2);
			scoreText.text = $"Score: {score}";

			// loops through all the tiles in collections and deletes them

			foreach (GameObject i2 in i)
			{
				for (int i3 = 0; i3 < tileSpaces.Length; i3++)
				{

					if (tileSpaces[i3].transform.childCount != 0)
					{
						if (tileSpaces[i3].transform.GetChild(0).gameObject.Equals(i2))
						{
							boardSpaces[i3].spaceOccupied = false;
							Destroy(i2);
						}
					}
					
				}
			}
		}
	}

	private void findConnected()
	{
		// Creation of relevent lists

		List<List<GameObject>> copyList = totalObjects;
		List<GameObject> removed = new List<GameObject>();
		
		// loops through all collections in list

		for (int i = 0; i < copyList.Count; i++)
		{
			// loops through all tiles in collections

			for (int i2 = 0; i2 < copyList[i].Count; i2++)
			{
				// loops through all collections

				for (int i3 = 0; i3 < copyList.Count; i3++)
				{
					// checks if its looking at the same collection that its already in

					if (!copyList[i3].Equals(copyList[i]))
					{
						// checks to see if the collection contains tiles from another collection and ahsnt already been removed

						if (copyList[i3].Contains(copyList[i][i2]) && !removed.Contains(copyList[i][i2]))
						{
							// removes the tiles from one of the collections and merges the rest into one collection

							removed.Add(copyList[i][i2]);
							totalObjects[i3].Remove(copyList[i][i2]);
							foreach (GameObject i4 in totalObjects[i3])
							{
								totalObjects[i].Add(i4);
							}
							totalObjects.Remove(totalObjects[i3]);
						}
					}
				}
			}
		}
	}

	private void CheckForZero()
	{
		// Set up variables

		bool aSquare = false;

		// Loops through all board spaces checking for a space occupied

		for (int i = 0; i < boardSpaces.Length; i++) if (boardSpaces[i].spaceOccupied) { aSquare = true; break; }

		// Checks if any tiles were found if not spawns a tile

		if (!aSquare) SpawnTile(false);
	}

	// A test board for the demo

	public void TesterBoard()
	{
		// Makes a array of numbers for wall locations

		int[] s = new int[] { 5, 11 };

		// Spawns the board

		SpawnBoard(4, s);

		// Spawns 2 tiles

		SpawnTile(false);
		SpawnTile(false);
	}

	// Game over Funtion

	private void CheckGameOver()
	{

		foreach (BoardSpace space in boardSpaces)
		{
			if (!space.spaceOccupied && !space.isWallTile)
			{
				return;
			}
		}

		gameOverText.enabled = true;
		Debug.Log("you lost");
	}

	// Gets called by button to restart board

	public void ReloadBoard()
	{
		// Sets score to 0

		score = 0;
		scoreText.text = $"Score: {score}";

		// Sets gameover text to off

		gameOverText.enabled = false;

		// Makes all squares un occupied
		
		foreach (BoardSpace space in boardSpaces)
		{
			space.spaceOccupied = false;
		}

		// Destroys all children of occupied squares

		foreach (GameObject spot in tileSpaces)
		{
			if (spot.transform.childCount != 0) Destroy(spot.transform.GetChild(0).gameObject);
		}

		// Spawns 2 new tiles

		SpawnTile(false);
		SpawnTile(false);
	}

	// Gets called by button to quit game

	public void ExitGame()
    {
		//Closes Application on call
		Application.Quit();
    }
}
