﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class Movement : NetworkBehaviour {

	private Camera camera;

	private Component weapons;

	public float maxJumpHeight = 1.5f;

	public float jumpSpeed = 5;

	public float cameraSpeed = 10;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			camera = (Camera) GetComponentInChildren (typeof(Camera));
			camera.gameObject.SetActive (true);
			weapons = transform.Find ("weapons");
		} else {
			GetComponentInChildren (typeof(Camera)).gameObject.SetActive (false);
		}

        GetComponent<Player>().Setup();

	}

	private float speed = 10;

	private bool jumping = false;

	private bool falling = false;

	private float currentJumpHeight = 0;

	private float horizontal;

	private float vertical;



	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}

		//jump
		if(Input.GetKey(KeyCode.Space) && !jumping && !falling){
			jumping = true;
			Jump();
		}

		if (jumping) {
			Jump ();
		}

		if (falling || !(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.1f)) && !jumping) {
			Fall ();
		}
			
		if (!jumping && !falling) {

			//sprint
			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				speed = 15;
			} 
			if (Input.GetKeyUp (KeyCode.LeftShift)) {
				speed = 10;
			} 

			//movement
			horizontal = Input.GetAxis ("Horizontal") * Time.deltaTime * speed;
			vertical = Input.GetAxis ("Vertical") * Time.deltaTime * speed;

			transform.Translate (horizontal, 0, vertical);
		}
		//camera movement
		float cameraHorizontal = Input.GetAxis("Mouse X") * cameraSpeed;
		float cameraVertical = Input.GetAxis("Mouse Y") * cameraSpeed;
		Vector3 cameraRotation = camera.transform.eulerAngles;
		Vector3 playerRotation = transform.eulerAngles;
		transform.eulerAngles = new Vector3 (0, playerRotation.y + cameraHorizontal, 0);
		camera.transform.eulerAngles = new Vector3(  cameraRotation.x - cameraVertical, playerRotation.y + cameraHorizontal, 0);
		weapons.transform.eulerAngles = new Vector3(  cameraRotation.x - cameraVertical, playerRotation.y + cameraHorizontal, 0);


	}

	private void Jump(){
		if (currentJumpHeight < maxJumpHeight) {
			float temp = 0;
			temp = Time.deltaTime * jumpSpeed;
			currentJumpHeight += temp;
			transform.Translate (horizontal, temp, vertical);
		} else {
			jumping = false;
			falling = true;
		}
	}

	private void Fall(){
		if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down),1)) {
			float temp = 0;
			temp = Time.deltaTime * jumpSpeed;
			transform.Translate (horizontal, -temp, vertical);
		} else {
			jumping = false;
			falling = false;
			currentJumpHeight = 0;
		}
	}

    public override void OnStartClient() {
        base.OnStartClient();

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(netId, player);
    }

    void OnDisable() {
        GameManager.UnRegisterPlayer(transform.name);
    }

}
