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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetention : MonoBehaviour, IDragHandler, IEndDragHandler
{
	[SerializeField] GameScript game;
	//Creates the return types for the function
	public enum DraggedDir
	{
		Up,
		Down,
		Right,
		Left
	}

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			game.MoveTiles(GetDragDirection(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))));
		}
	}

    //detects the end of a drag and grabs the data associated with it
    public void OnEndDrag(PointerEventData eventData)
	{
		//Calculates the direction of the drag vector 
		Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;

		if (game.canSwipe) game.MoveTiles(GetDragDirection(dragVectorDirection));
	}

	// OnDrag to statisfy calling IDragHandler in class

	public void OnDrag(PointerEventData eventData)
	{

	}

	// Calculates the vectors direction
	private DraggedDir GetDragDirection(Vector3 dragVector)
	{
		//Finds the absolute value of the X and Y vectors
		float positiveX = Mathf.Abs(dragVector.x);
		float positiveY = Mathf.Abs(dragVector.y);
		DraggedDir draggedDir;


		//finds the dragged direction depending on if it was dragged more on the positive x or y
		if (positiveX > positiveY)
		{
			// checks if the x was more or less then 0
			draggedDir = (dragVector.x > 0) ? DraggedDir.Right : DraggedDir.Left;
		}
		else
		{
			// checks if the y was more or less then 0
			draggedDir = (dragVector.y > 0) ? DraggedDir.Up : DraggedDir.Down;
		}
		Debug.Log(draggedDir);

		// returns the calculated direction 
		return draggedDir;
	}
}
