using UnityEngine;
using System.Collections;

public class restart : MonoBehaviour {

	public GameObject kula; 

	// Update is called once per frame
	void FixedUpdate () {
		bool key = Input.GetKey(KeyCode.R);
		if (key) {
			kula.rigidbody.MovePosition(new Vector3(3.413901f, 7.100071f, -7.553854f));
			kula.rigidbody.velocity.Set(0.0f, 0.0f, 0.0f);
			kula.rigidbody.angularVelocity.Set(0.0f, 0.0f, 0.0f);
		}
	}
}
