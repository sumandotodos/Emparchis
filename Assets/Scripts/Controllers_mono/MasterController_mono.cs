using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/*
 * 
 * 
 * MasterController
 * 
 *    Game: Camina un Rato con mis Zapatos
 * 
 * 
 * 
 */ 

public class MasterController_mono : Task {

	public GameObject blackScreenOfDeath;
	public Texture[] playersMiniature;
	public RawImage myPlayerMiniature;

	public GameObject upgradeCanvas;
	public UIScaleFader upgradeNoticeScaler;

	const float maxDoubleTapDelay = 0.25f;
	float doubleTapElapsedTime = 0;

	public UIFaderScript gearFader;
	public UIDelayFader gearDelay;

	public const int ForceTest = -1;
	public const int ForceTest3Subtest = 0;
	public int testIncremento = 4;

	public AudioManager aManager;

	public StringBank serverStatusStrings;

	public GameObject logoActivity;
	public GameObject titleActivity;
	public GameObject scanActivity;
	public GameObject mainGameActivity;
	public GameObject newGameActivity;
	public GameObject joinGameActivity;
	public GameObject createNewGameActivity;
	public GameObject familyActivity;
	public GameObject choosePlayerActivity;
	public GameObject continueGameActivity;
	public GameObject rouletteActivity;
	public GameObject notMyRouletteActivity;
	public GameObject turnActivity;
	public GameObject statisticsActivity;
	public GameObject galleryActivity;
	public GameObject serverStatusCanvas;

	public TitleController_mono titleController;
	public GameController_mono gameController;
	public LogoController_mono logoController;
	//public CreateNewGameController_mono createNewGameController;
	//public JoinNewGameController_mono joinNewGameController;
	public FamilyController_mono familyController;
	public GalleryController_mono galleryController;
	public ChoosePlayerController_mono choosePlayerController;
	public RouletteController_mono rouletteController;
	public NotMyRouletteController_mono notMyRouletteController;
	public MainGameController_mono mainGameController;
	public StatisticsController_mono statisticsController;
	public MiniatureController_mono miniatureController;
	public PreFinishController_mono prefinishController;
	public CommonTestController_mono commonTestController;
	public NubarronController_mono nubarronController;
	public VotationController_mono votationController;
	public TicketScreenController_mono ticketScreenController;
	public BingoController_mono bingoController;
	public Type0TestController_mono type0Controller;
	public Type1TestController_mono type1Controller;
	public Type2TestController_mono type2Controller;
	public Type3PrimeTestController_mono type3Controller;
	public Type3PrimeAuxController_mono type3auxController;
	public Type4TestController_mono type4Controller;

	public Text serverStatusText;
	public RawImage serverStatusBatsu;

	public RunMode runMode;

	[HideInInspector]
	public int previousState;
	WWW www;

	[HideInInspector]
	public string startActivity;

	[HideInInspector]
	bool showingService = false;

	public UIScaleFader servicePanel;

	public int state;
	public float timer;

	public void reset() {

		if (runMode != RunMode.Debug) {
			logoActivity.SetActive (false);
			titleActivity.SetActive (false);
			scanActivity.SetActive (false);
			mainGameActivity.SetActive (false);
			newGameActivity.SetActive (false);
			joinGameActivity.SetActive (false);
			familyActivity.SetActive (false);
			choosePlayerActivity.SetActive (false);
			rouletteActivity.SetActive (false);
			notMyRouletteActivity.SetActive (false);
			turnActivity.SetActive (false);
			statisticsActivity.SetActive (false);
			serverStatusCanvas.SetActive (false);
			blackScreenOfDeath.SetActive (false);
		}

		state = 0;
		timer = 0.0f;
		state = 1;

		gameController.loadMoarData (); // load obtained gifts data
	}

	public void playSound(AudioClip clip) {
		aManager.playSound (clip);
	}

	void Start () 
	{	
		
		upgradeCanvas.SetActive (false);
		WWWForm myWWWForm = new WWWForm ();
		myWWWForm.AddField ("app", "EmpLite");



		blackScreenOfDeath.SetActive (false);
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		reset ();

		servicePanel.scaleOutImmediately ();

		Screen.orientation = ScreenOrientation.Portrait;
	}

	public void showGear() {
		gearDelay.resetTimer ();
		gearFader.fadeOut ();
	}
	
	void Update () 
	{
		doubleTapElapsedTime += Time.deltaTime;
		if (Input.GetMouseButtonDown (0)) 
		{
			if (doubleTapElapsedTime < maxDoubleTapDelay) 
			{
				showGear ();
			}
			doubleTapElapsedTime = 0.0f;
		}

		if (state == 666) {
			timer -= Time.deltaTime;
			if (timer < 0.0f) {
				
				timer = 0.25f;
				state = 667;
			}
		}
		if (state == 667) {
			timer -= Time.deltaTime;
			if (timer < 0.0f) {
				SceneManager.LoadScene ("Scenes/Selector");
			}
		}
	
		if (runMode == RunMode.Debug)
			return;

		if (state == 0) { // idle

		}
		if (state == 1) { // launch logo activity
			logoActivity.SetActive (true);
			logoController.startLogoActivity (this);
			previousState = 2;
			state = 2; // 
		}

		if (state == 20) {
			
			state = 21;
		}
		if (state == 21) {
			if (www.isDone) {
				if (www.text.Equals ("")) {
					string status = serverStatusStrings.getString (0);
					serverStatusCanvas.SetActive (true);
					serverStatusText.text = status;
					state = 22;
				} else {
					string[] msg = www.text.Split (':');

					if (msg [0].Equals ("0")) {
						state = previousState; // all A-OK, proceed normally
						serverStatusCanvas.SetActive (false);
					} else if (msg [0].Equals ("1")) { // planned downtime
					
						string status = serverStatusStrings.getString (1);
						status = status.Replace ("<1>", msg [1]);
						status = status.Replace ("<2>", msg [2]);
						status = status.Replace ("<3>", msg [3]);
						status = status.Replace ("<4>", msg [4]);
						serverStatusBatsu.enabled = false;
						serverStatusCanvas.SetActive (true);
						serverStatusText.text = status;
						state = 22;
					} else if (msg [0].Equals ("3")) { // pre-downtime notice
						string status = serverStatusStrings.getString (2);
						status = status.Replace ("<1>", msg [1]);
						status = status.Replace ("<2>", msg [2]);
						serverStatusBatsu.enabled = true;
						serverStatusCanvas.SetActive (true);
						serverStatusText.text = status;

						state = previousState;

					}
					else  { // some other unplanned shit
						string status = serverStatusStrings.getString (0);
						serverStatusBatsu.enabled = false;
						serverStatusCanvas.SetActive (true);
						serverStatusText.text = status;
						state = 22;
					}
				}
			}
		}

		if (state == 2) { // wait for logo to end
			if (!isWaitingForTaskToComplete) {
				logoActivity.SetActive (false);
				galleryActivity.SetActive (false);
				titleActivity.SetActive (true);
				titleController.startTitleActivity (this);
				state = 3;
			}
		}
		if (state == 3) { // wait for title to end
			if (!isWaitingForTaskToComplete) {
				if (startActivity.Equals ("Title")) {
					joinGameActivity.SetActive (false);
					createNewGameActivity.SetActive (false);
					titleActivity.SetActive (true);
					titleController.startTitleActivity (this);

				}
				if (startActivity.Equals ("ContinueGame")) {
					titleActivity.SetActive (false);
					continueGameActivity.SetActive (true);

					//state = 4;
				}
				if (startActivity.Equals ("StartNewGame")) {
					titleActivity.SetActive (false);
					startActivity = "MainGame";
				}/*
				if (startActivity.Equals ("JoinNewGame")) {
					titleActivity.SetActive (false);
					joinGameActivity.SetActive (true);
					joinNewGameController.startJoinNewGameActivity (this);
					//state = 4;
				}*/
				if (startActivity.Equals ("Gallery")) {
					titleActivity.SetActive (false);
					galleryActivity.SetActive (true);
					galleryController.startGalleryActivity (this);
					state = 2; // start title when this task ends...
				}
				if (startActivity.Equals ("Family")) {
//					joinGameActivity.SetActive (false);
//					newGameActivity.SetActive (false);
//					familyActivity.SetActive (true);
//					familyController.startFamilyActivity (this);
					familyActivity.SetActive (false);
					choosePlayerActivity.SetActive (true);
					choosePlayerController.startChoosePlayerActivity (this);
					//state = 4;
				}
				if (startActivity.Equals ("ChoosePlayer")) {
					familyActivity.SetActive (false);
					choosePlayerActivity.SetActive (true);
					choosePlayerController.startChoosePlayerActivity (this);
					//state = 4;
				}
				if (startActivity.Equals ("MainGame")) {
					int myPlayer = gameController.localPlayerN = 0;
					myPlayerMiniature.enabled = true;
					myPlayerMiniature.texture = playersMiniature [myPlayer];
					continueGameActivity.SetActive (false);
					choosePlayerActivity.SetActive (false);
					mainGameController.startMainGameActivity (this);
					//state = 4;
				}
				if (startActivity.Equals ("ResetGame")) {
					//networkAgent.disconnect ();
					gameController.reset ();
					hardReset ();
				}
			}
		}
		if (state == 4) { // waiting for CreateNewGame activity to finish
		}
	}


	public void closeServerStatus() {
		serverStatusCanvas.SetActive (false);
	}

	public void refreshServerStatus() 
	{
		if(state < 20) 
		{	
			previousState = state;
			state = 20; // check server status
		}
	}


	public void stopAllControllers() 
	{

	}

	public void toggleServicePanel() 
	{
		if (showingService) {
			showingService = false;
			servicePanel.scaleOut ();
		} else {
			showingService = true;
			servicePanel.scaleIn ();
		}

		gearDelay.resetTimer ();
		gearDelay.going = !gearDelay.going;
	}

	public void hardReset()
	{			
			
		blackScreenOfDeath.SetActive (true);
		timer = 0.5f;
		state = 666;
	}
}
