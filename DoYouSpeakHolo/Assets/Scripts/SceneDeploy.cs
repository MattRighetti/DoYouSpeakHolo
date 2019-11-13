using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDeploy : MonoBehaviour
{
	private Vector3 leftHousePosition;
	private Vector3 middleHousePosition;
	private Vector3 rightHousePosition;
	private Vector3 leftKeyPosition;
	private Vector3 middleKeyPosition;
	private Vector3 rightKeyPosition;

	public GameObject smallKey;
	public GameObject mediumKey;
	public GameObject bigKey;
	public GameObject smallHouse;
	public GameObject mediumHouse;
	public GameObject bigHouse;

	// Start is called before the first frame update
	void Start() {

		leftHousePosition = new Vector3(-1, 0, 2);
		middleHousePosition = new Vector3(0, 0, 2);
		rightHousePosition = new Vector3(1, 0, 2);
		leftKeyPosition = new Vector3(-1, 0.2f, 1.3f);
		middleKeyPosition = new Vector3(0, 0.2f, 1.3f);
		rightKeyPosition = new Vector3(1, 0.2f, 1.3f);
        Debug.Log("Created Vectors");
        Debug.Log("Instantiating GameObjects");
        initiateScene();

	}

    private void initiateScene() {

		GameObject smallKey = Instantiate(this.smallKey) as GameObject;
		GameObject mediumKey = Instantiate(this.mediumKey) as GameObject;
		GameObject bigKey = Instantiate(this.bigKey) as GameObject;
		GameObject smallHouse = Instantiate(this.smallHouse) as GameObject;
		GameObject mediumHouse = Instantiate(this.mediumHouse) as GameObject;
		GameObject bigHouse = Instantiate(this.bigHouse) as GameObject;

        Debug.Log("Instanted GameObjects");

        smallKey.transform.position = leftKeyPosition;
        mediumKey.transform.position = middleKeyPosition;
        bigKey.transform.position = rightKeyPosition;
        bigHouse.transform.position = leftHousePosition;
        mediumHouse.transform.position = middleHousePosition;
        smallHouse.transform.position = rightHousePosition;
	}

}
