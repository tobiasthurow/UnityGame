using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Canvas mainMenu;
	public Canvas gameUI;
	public NetworkManagerHUD hud;

	// Use this for initialization
	void Start () {
		mainMenu = GameObject.Find ("Menu").GetComponent<Canvas>();
		gameUI = GameObject.Find ("GameUI").GetComponent<Canvas>();
		GameObject thePlayer = GameObject.Find("NetworkController");
		hud = thePlayer.GetComponent<NetworkManagerHUD>();
		showGameUI ();
	}

	void OnDestroy(){
		showMainMenu ();
	}

	void showMainMenu(){
		mainMenu.enabled = true;
		gameUI.enabled = false;

		hud.offsetX = 550;
		hud.offsetY = 250;
	}

	void showGameUI(){
		mainMenu.enabled = false;
		gameUI.enabled = true;

		hud.offsetX = 0;
		hud.offsetY = 510;
	}

	// Update is called once per frame
	void Update () {

	}
		
}
