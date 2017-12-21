using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerBattleMovement : BattleMovementState
{

	public override void Execute()
	{
		base.Execute();
		Debug.Log("Player movement state executing!");
	}
	
	protected override void HandleMovement()
	{
		// check player input, move navi accordingly
		bool moveLeft = 				Input.GetKey(controls.moveLeft);
		bool moveRight = 				Input.GetKey(controls.moveRight);
		bool moveUp = 					Input.GetKey(controls.moveUp);
		bool moveDown = 				Input.GetKey(controls.moveDown);

		PanelController currentPanel = 	mover.panelCurrentlyOn;
		PanelController toMoveTo = 		null;
		Vector3 newPos = 				mover.transform.position;

		// basically grabbing adjacent panels from the battlefield manager...
		if (moveLeft)
			toMoveTo = BattlefieldManager.instance.GetPanelRelativeTo(currentPanel, Direction.left);
			
		else if (moveRight)
			toMoveTo = BattlefieldManager.instance.GetPanelRelativeTo(currentPanel, Direction.right);
	
		else if (moveUp)
			toMoveTo = BattlefieldManager.instance.GetPanelRelativeTo(currentPanel, Direction.up);

		else if (moveDown)
			toMoveTo = BattlefieldManager.instance.GetPanelRelativeTo(currentPanel, Direction.down);
		
		bool moved = false;

		// ... and checking if we can move there.
		if (toMoveTo != null && toMoveTo.traversable && toMoveTo.owner == mover.onSideOf)
		{
			// TODO: play an animation during the movement
			newPos.x = toMoveTo.transform.position.x;
			newPos.z = toMoveTo.transform.position.z;
			mover.transform.position = newPos;
			moved = true;
		}	

		if (moved)
			ResetMoveDelay();
	}

	
}
