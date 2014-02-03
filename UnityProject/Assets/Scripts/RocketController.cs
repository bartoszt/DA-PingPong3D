using UnityEngine;
using System.Collections;

public class RocketController : MonoBehaviour {
	
	private float moveX;
	private float moveY;
	private float moveZ;
	private float rotationX;
	private float rotationY;
	private float rotationZ; 

	public int movementSpeed;

	public int Player;
	
	// Update is called once per frame
	void FixedUpdate () {	
		//Debug.Log (pxsLeapInput.GetHandAxis ("Depth"));
		moveX = pxsLeapInput.GetHandAxis("Depth")[Player];// * 3; 
		moveY = pxsLeapInput.GetHandAxis("Vertical")[Player]; 
		moveZ = pxsLeapInput.GetHandAxis("Horizontal")[Player];// * 3; 
		rotationX = pxsLeapInput.GetHandAxis("RotationX")[Player];// * 45;
		rotationY = pxsLeapInput.GetHandAxis("RotationY")[Player];// * 45;
		rotationZ = pxsLeapInput.GetHandAxis("RotationZ")[Player];// * 45;

		//Debug.Log (rotationX + " | " + rotationY + " | " + rotationZ);
		Vector3 angle = new Vector3 (rotationY * 20, rotationX * 30, 90.0f);

		//if (Player == 0 && moveX * movementSpeed > 0.0f || Player == 1 && moveX * movementSpeed < 0.0f) {
					if (moveZ != 0.0f || moveY != 0.0f || moveZ != 0.0f) {
							float mY = moveY*movementSpeed/2-3;
							if(mY < 1) {
								mY = 1;
							}
							rigidbody.MovePosition (new Vector3 (moveX * movementSpeed, moveY*movementSpeed/2-3, moveZ * movementSpeed));
					}
			//}
		Quaternion deltaRotation = Quaternion.Euler(angle);
		rigidbody.MoveRotation(deltaRotation);

	}
}
