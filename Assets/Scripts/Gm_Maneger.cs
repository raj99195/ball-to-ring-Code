using UnityEngine;
using UnityEngine.UI;


public class Gm_Maneger : MonoBehaviour {
	public bool isDialogViewed;
	public bool gm_runing = false;
	public bool ispaused = false;
	public bool IsFirstLogin;
	public bool help_opened = false;
	public bool canVibrate;

	public int curent_step ;
	public int Curent_ring = 3;

	public int num_for_step_2, num_for_step_3, num_for_step_4, num_for_step_5, num_for_step_6 ;
	public int delta_number_for_random;
	int random_num_for_step_2, random_num_for_step_3, random_num_for_step_4, random_num_for_step_5, random_num_for_step_6;

	int score , bestScore ;

	const string SOUND_MUTE = "soundMute" ; 
	const string MUSIC_MUTE = "musicMute" ;
	const string CAN_VIBRATE = "canVibrator" ;

	long[] trueVibratePatern = { 30, 30};
	long[] falseVibratePatern = { 100, 200, 300};

	public bool can_to_play;
	public bool can_create_ball;

	public Color[] colorsOfMenuObjs;
	public GameObject[] uiGmObjsOfChengeColor;

 	public GameObject mainRing;
	public GameObject ball_creater;
	public MainRing_Controler mainRing_Controler_script;
	public GameObject score_txt , bestScoreText , lastBestScoreText;
	public GameObject WA;
	public Button claimbtn;
	Animator DialogAnimator;

	// meno elements
	public GameObject dialog_blur , btn_start , btn_restart , music_btn , sound_btn , vibrator_btn;
	public GameObject btn_R, btn_L;
	public Bg_Controler bg_Controler_script;
	Main_Ring main_Ring_script;
	
	BallCreter ball_Creater_Script;

	
	void Start() {

		// Comment for Android output
		// for Html5 outPut
		//if (Screen.currentResolution.height > Screen.currentResolution.width) {
		//	Screen.autorotateToPortrait = false;
		//	Screen.autorotateToPortraitUpsideDown = false;
		//	Screen.orientation = ScreenOrientation.Portrait;
		//	Screen.fullScreen = true;
		//	Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
		//}


		if (PlayerPrefs.HasKey("bestScore")) {
		
		bestScore = PlayerPrefs.GetInt("bestScore", -1);
	}
	else {
			bestScore = 0;
	}

        #region setingPrefs
        Image img1 = music_btn.GetComponent<UnityEngine.UI.Image>();
		if (PlayerPrefs.HasKey(MUSIC_MUTE)) {
			if (PlayerPrefs.GetInt(MUSIC_MUTE) == 1) {
				AudioManeger.instance.musicMute = true;
				img1.color = new Color(img1.color.r, img1.color.g, img1.color.b, 0.5f);
			}
			else if(PlayerPrefs.GetInt(MUSIC_MUTE) == 0) {
				AudioManeger.instance.musicMute = false;
				img1.color = new Color(img1.color.r, img1.color.g, img1.color.b, 1f);
			}
		}
		else {
			PlayerPrefs.SetInt(MUSIC_MUTE, 0);
			img1.color = new Color(img1.color.r, img1.color.g, img1.color.b, 1f);
		}

		Image img2 = sound_btn.GetComponent<UnityEngine.UI.Image>();
		if (PlayerPrefs.HasKey(SOUND_MUTE)) {
			if (PlayerPrefs.GetInt(SOUND_MUTE) == 1) {
				AudioManeger.instance.soundMute = true;
				img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 0.5f);
			}
			else if (PlayerPrefs.GetInt(SOUND_MUTE) == 0) {
				AudioManeger.instance.soundMute = false;
				img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 1f);
			}
		}
		else {
			PlayerPrefs.SetInt(SOUND_MUTE, 0);
			img2.color = new Color(img2.color.r, img2.color.g, img2.color.b, 1f);
		}

		Image img3 = vibrator_btn.GetComponent<UnityEngine.UI.Image>();
		if (PlayerPrefs.HasKey(CAN_VIBRATE)) {
			if(PlayerPrefs.GetInt(CAN_VIBRATE) == 1) {
				canVibrate = true;
				img3.color = new Color(img3.color.r, img3.color.g, img3.color.b, 1f);
			}
			else if(PlayerPrefs.GetInt(CAN_VIBRATE) == 0){
				canVibrate = false;
				img3.color = new Color(img3.color.r, img3.color.g, img3.color.b, 0.5f);
			}
		}
		else {
			PlayerPrefs.SetInt(CAN_VIBRATE , 1);
			canVibrate = true;
			img3.color = new Color(img3.color.r, img3.color.g, img3.color.b, 1f);
		}
        #endregion

        main_Ring_script = mainRing.GetComponent<Main_Ring>();
		
		ball_Creater_Script = ball_creater.GetComponent<BallCreter>();
		DialogAnimator = dialog_blur.GetComponent<Animator>();
		bg_Controler_script.set();
		//setDialogColor();
		view_start_dialog(true);
		createRandomNumberByDeltaForStep();
		
		//lastBestScore = 0;
		setBestScoreText(bestScore);
		setLastBestScoreText(bestScore);

		WA.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (mainRing_Controler_script.can_tuch_to_rotate)
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				mainRing_Controler_script.rotate(1);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				mainRing_Controler_script.rotate(-1);
			}
		}

	}

	
	/// <summary>
/// Create The stepLevel
/// </summary>
/// <param name="step"> number level :) </param>
	public void start_step(int step)
	{

		//Debug.Log("start_step1 " + step);
		switch (step)
		{
			case 1:
				setChenges(1);
				mainRing.GetComponent<Animator>().SetTrigger("mainRingTriggerAnim");
				break;
			case 2:
				setChenges(2);
				mainRing.GetComponent<Animator>().SetTrigger("mainRingTriggerAnim");
				break;
			case 3:
				setChenges(3);
				mainRing.GetComponent<Animator>().SetTrigger("mainRingTriggerAnim");
				break;	
			case 4:
				setChenges(4);
				mainRing.GetComponent<Animator>().SetTrigger("mainRingTriggerAnim");
				break;	
			case 5:
				setChenges(5);
				mainRing.GetComponent<Animator>().SetTrigger("mainRingTriggerAnim");
				break;	
			case 6:
				setChenges(6);
				mainRing.GetComponent<Animator>().SetTrigger("mainRingTriggerAnim");
				break;
		}
	}
	
	/// <summary>
	/// set the number step to other script's
	/// </summary>
	/// <param name="curentStep"></param>
	/// <param name="Rings"></param>
	void setChenges(int step)
	{
		curent_step = step;
		main_Ring_script.curent_step = curent_step;
		ball_Creater_Script.curent_step = curent_step;
		mainRing_Controler_script.curent_step = curent_step;
	}

	/// <summary>
	/// set the gm_runing to other script's
	/// </summary>
	void set_gm_Running(bool gm_runing)
	{
		this.gm_runing = gm_runing;
		ball_Creater_Script.setGmRunning(gm_runing);
	}

	void setIsPaused(bool isPaused)
	{
		this.ispaused = isPaused;
		ball_Creater_Script.setPaused(isPaused);
	}

	public void add_score(int num_add_score)
	{
		Invoke("vibrateTrue", 0.05f);
		score += num_add_score;
		AudioManeger.instance.play("ball to ring true");
		setScoreText(score);
		DialogAnimator.SetTrigger("add score");
		
		if(score == random_num_for_step_2)
		{
			curent_step = 2;
			bg_Controler_script.set();
			start_step(curent_step);
		}
		else if(score == random_num_for_step_3)
		{
			curent_step = 3;
			bg_Controler_script.set();
			start_step(curent_step);
		}
		else if (score == random_num_for_step_4)
		{
			curent_step = 4;
			bg_Controler_script.set();
			start_step(curent_step);
		}
		else if (score == random_num_for_step_5)
		{
			curent_step = 5;
			bg_Controler_script.set();
			start_step(curent_step);
		}
		else if (score == random_num_for_step_6)
		{
			curent_step = 6;
			bg_Controler_script.set();
			start_step(curent_step);
			claimbtn.gameObject.SetActive(true);
		}
		//print(score);
	}

	void createRandomNumberByDeltaForStep()
	{
		if(
			delta_number_for_random >= num_for_step_2 
			|| num_for_step_2 >= num_for_step_3 
			|| num_for_step_3 >= num_for_step_4 
			|| num_for_step_4 >= num_for_step_5 
			|| num_for_step_5 >= num_for_step_6
			)
		{
			Debug.LogError("Number For Steps Or delta_number_fo_step Is Invalid!!!");
			Debug.Break();
		}
		random_num_for_step_2 = Random.Range(num_for_step_2 - delta_number_for_random, num_for_step_2 + delta_number_for_random);
		random_num_for_step_3 = Random.Range(num_for_step_3 - delta_number_for_random, num_for_step_3 + delta_number_for_random);
		random_num_for_step_4 = Random.Range(num_for_step_4 - delta_number_for_random, num_for_step_4 + delta_number_for_random);
		random_num_for_step_5 = Random.Range(num_for_step_5 - delta_number_for_random, num_for_step_5 + delta_number_for_random);
		random_num_for_step_6 = Random.Range(num_for_step_6 - delta_number_for_random, num_for_step_6 + delta_number_for_random);
	}
	void setScoreText(int score)
	{
		score_txt.GetComponent<UnityEngine.UI.Text>().text = score.ToString();
	}

	void setBestScoreText(int bestScore)
	{
		bestScoreText.GetComponent<UnityEngine.UI.Text>().text = bestScore.ToString();
	}

	void setLastBestScoreText(int LastbestScore)
	{
		lastBestScoreText.GetComponent<UnityEngine.UI.Text>().text = LastbestScore.ToString();
	}

	public void lose()
	{
		
		//Debug.Log("loser!!!!!!!!!!!!!!!!!");
		set_gm_Running(false);
		mainRing_Controler_script.can_tuch_to_rotate = false;
		vibrateFalse();
		ball_Creater_Script.remove_Childs(true);
		ball_Creater_Script.canCreate = false;
		view_lose_dialog(true);

		claimbtn.gameObject.SetActive(true);
	}

	/// <summary>
	/// called in meno Dialog anim
	/// </summary>
	public void reset_after_lose()
	{
		bg_Controler_script.set_random_array();
		bg_Controler_script.set();
		start_step(1);
		score = 0;
		
	}

	public void reset_After_Pause()
	{
		bg_Controler_script.set_random_array();
		bg_Controler_script.set();
		start_step(1);
		score = 0;
	}

	/// <summary>
	/// Continues the Game After play hiden Pause Anim
	/// </summary>
	public void resumeGame()
	{
		Time.timeScale = 1;
		if (!gm_runing)
		{
			
			set_gm_Running(true);
		}
		mainRing_Controler_script.can_tuch_to_rotate = true;
		
		setRLbtnsActive(true);
	}

	void setMenoObjectsColor()
	{
		Color randomColor = colorsOfMenuObjs[Random.Range(0, (colorsOfMenuObjs.Length))];
		//print("setMenoObjectsColor()");
		foreach(GameObject obj in uiGmObjsOfChengeColor)
		{
			if(obj != null)
			{
				if(obj.GetComponent<UnityEngine.UI.Image>() != null)
				{
					Color c1 = new Color(
						randomColor.r,
						randomColor.g,
						randomColor.b,
						obj.GetComponent<UnityEngine.UI.Image>().color.a
						);
					obj.GetComponent<UnityEngine.UI.Image>().color = c1;
					//Debug.Log("obg is btn or img and set the color" + obj.GetComponent<UnityEngine.UI.Image>());
				}
				else if (obj.GetComponent<UnityEngine.UI.Text>() != null)
				{
					obj.GetComponent<UnityEngine.UI.Text>().color = randomColor;
					//Debug.Log("obg is text and set the color" + obj.GetComponent<UnityEngine.UI.Text>());
				}
			}
		}
	}
	void setRLbtnsActive(bool isActive)
	{
		btn_R.SetActive(isActive);
		btn_L.SetActive(isActive);
		
	}
 
	/////////// meno btns Click
	#region btnsClick

	public void dialogOnClick()
	{
		AudioManeger.instance.play("btns sound");


		if (help_opened)
		{
			helpBtnClick();
		}
		if (setting_opened)
		{
			settingBtnClick();
		}
	}

	public void start_Btn_Clicked()
	{
		AudioManeger.instance.play("btns sound");
		view_start_dialog(false);
		set_gm_Running(true);
		if (!PlayerPrefs.HasKey("isFirstLogin"))
		{
			IsFirstLogin = true;
			PlayerPrefs.SetInt("isFirstLogin", 0);
			Invoke("helpBtnClick", 1f);
			setRLbtnsActive(false);	
		}
		else
		{
			IsFirstLogin = false;
			setRLbtnsActive(true);
			ball_Creater_Script.canCreate = true;
		}
	
	}

	public void restart_Btn_Clicked()
	{
		if (help_opened) {
			helpBtnClick();
		}
		if (setting_opened) {
			settingBtnClick();
		}
		AudioManeger.instance.play("btns sound");
		if (ispaused)
		{
			Time.timeScale = 1;
			DialogAnimator.SetTrigger("hiden pause and restart");
			setRLbtnsActive(true);
			ball_Creater_Script.remove_Childs(false);
			setIsPaused(false);
			setScoreText(0);
			
		}
		else
		{
			view_lose_dialog(false);
		}

		
		set_gm_Running(true);
		mainRing_Controler_script.can_tuch_to_rotate = true;
		ball_Creater_Script.canCreate = true;

	}

	public void resume_Btn_Clicked()
	{
		if (help_opened) {
			helpBtnClick();
		}
		if (setting_opened) {
			settingBtnClick();
		}
		AudioManeger.instance.play("btns sound");
		view_pause_dialog(false);
		if (ispaused)
		{
			setIsPaused(false);
		}
	}

	bool setting_opened = false;
	public void settingBtnClick()
	{
		AudioManeger.instance.play("btns sound");
		Debug.Log("settingBtnClick");
		if (setting_opened)
		{
			DialogAnimator.SetTrigger("back click setting btn");
			setting_opened = false;
		}
		else
		{
			DialogAnimator.SetTrigger("click setting btn");
			setting_opened = true;
		}
	}

	
	public void helpBtnClick()
	{
		AudioManeger.instance.play("btns sound");
		Debug.Log("helpBtnClick");
		if (help_opened)
		{
			if (IsFirstLogin) {
				IsFirstLogin = false;
				start_Btn_Clicked();
			}

			DialogAnimator.SetTrigger("hiden guide");
			help_opened = false;
		}
		else
		{
			DialogAnimator.SetTrigger("view guide");
			help_opened = true;
		}
	}


	public void closeHelpBtnClick()
	{
		if (help_opened)
		{
			AudioManeger.instance.play("btns sound");
			helpBtnClick();
		}
	}

	/// <summary>
	/// pause button clicked
	/// </summary>
	public void pause_button()
	{
		if (help_opened) {
			return;
		}
		Time.timeScale = 0;
		AudioManeger.instance.play("btns sound");
		if (gm_runing)
		{
			set_gm_Running(false);
			setIsPaused(true);
			Debug.Log("dialog_pause_viwed = " + ispaused);
			view_pause_dialog(true);
			mainRing_Controler_script.can_tuch_to_rotate = false;
		}
	}


	public void soundMuteBtnClick() {
		Image img = sound_btn.GetComponent<UnityEngine.UI.Image>();
		if (AudioManeger.instance.soundMute) {
			AudioManeger.instance.soundMute = false;
			PlayerPrefs.SetInt(SOUND_MUTE, 0);
			img.color = new Color(img.color.r,img.color.g,img.color.b,1f);
		}
		else {
			AudioManeger.instance.soundMute = true;
			PlayerPrefs.SetInt(SOUND_MUTE, 1);
			img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
		}
	}

	public void musicMuteBtnClick() {
		Image img = music_btn.GetComponent<UnityEngine.UI.Image>();
		if (AudioManeger.instance.musicMute) {
			PlayerPrefs.SetInt(MUSIC_MUTE, 0);
			AudioManeger.instance.musicMute = false;
			AudioManeger.instance.play("meno background");
			img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
		}
		else {
			PlayerPrefs.SetInt(MUSIC_MUTE, 1);
			AudioManeger.instance.musicMute = true;
			AudioManeger.instance.stop("meno background");
			img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
		}
	}

	public void vibratorBtnClick() {
		Image img = vibrator_btn.GetComponent<UnityEngine.UI.Image>();
		if (PlayerPrefs.GetInt(CAN_VIBRATE)==1) {
			img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
			PlayerPrefs.SetInt(CAN_VIBRATE, 0);
			canVibrate = false;
		}
		else if (PlayerPrefs.GetInt(CAN_VIBRATE) == 0) {
			img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
			PlayerPrefs.SetInt(CAN_VIBRATE, 1);
			canVibrate = true;
		}
	}
	#endregion

	/////////// dialog view
	#region meno dilog view
	/// <summary>
	/// Viewed start meno dialog
	/// </summary>
	/// <param name="view"></param>
	void view_start_dialog(bool view)
	{
		if (view)
		{
			isDialogViewed = true;
			AudioManeger.instance.play("meno background");
			AudioManeger.instance.stop("gm background");
			
			setMenoObjectsColor();
			//setDialogColor();
			DialogAnimator.SetTrigger("view_start");
			setRLbtnsActive(false);
		}
		else
		{
			isDialogViewed = false;
			AudioManeger.instance.play("gm background");
			AudioManeger.instance.stop("meno background");
			
			DialogAnimator.SetTrigger("hiden_start");
			
		}
	}

	/// <summary>
	/// Viewed the lose Dialog Meno
	/// </summary>
	/// <param name="view"></param>
	string viewName, hidenName;
	void view_lose_dialog(bool view)
	{
		
		if (view)
		{
			isDialogViewed = true;
			AudioManeger.instance.play("meno background");
			AudioManeger.instance.stop("gm background");

			setMenoObjectsColor();
			if (bestScore < score && score > 5)
			{
				PlayerPrefs.SetInt("bestScore", score);
				PlayerPrefs.Save();
				//print("............." + PlayerPrefs.GetInt("bestScore", -1));
				bestScore = score;
				setBestScoreText(bestScore);
				viewName = "view lose best record";
				hidenName = "hiden lose best record";
			}
			else if(bestScore < score && score <= 5) {
				PlayerPrefs.SetInt("bestScore", score);
				PlayerPrefs.Save();
				bestScore = score;
				setBestScoreText(bestScore);
				viewName = "view_lose";
				hidenName = "hiden_lose";
				
			}
			else
			{
				
				viewName = "view_lose";
				hidenName = "hiden_lose";
			}
			//setDialogColor();
			DialogAnimator.SetTrigger(viewName);
			setRLbtnsActive(false);
			
		}
		else
		{
			if (Time.timeScale == 0) {
				Time.timeScale = 1;
			}
			isDialogViewed = false;
			AudioManeger.instance.play("gm background");
			AudioManeger.instance.stop("meno background");

			DialogAnimator.SetTrigger(hidenName);
			setRLbtnsActive(true);

			setScoreText(0);
			setLastBestScoreText(bestScore);
			if (setting_opened)
			{
				settingBtnClick();
			}
		}
	}

	void view_pause_dialog(bool view)
	{
		if (view)
		{
			isDialogViewed = true;
			AudioManeger.instance.play("meno background");
			AudioManeger.instance.stop("gm background");

			//setDialogColor();
			DialogAnimator.SetTrigger("view pause");
			setRLbtnsActive(false);
		}
		else
		{
			isDialogViewed = false;
			AudioManeger.instance.play("gm background");
			AudioManeger.instance.stop("meno background");

			DialogAnimator.SetTrigger("hiden pause");
		}
	}

	#endregion

	void vibrateFalse() {
		if (canVibrate) {
			try {
				Vibration.Vibrate(falseVibratePatern, -1);
			}
			catch {

			}
		}
	}
	void vibrateTrue() {

		if (canVibrate) {
			try { 
				Vibration.Vibrate(trueVibratePatern, -1);
			}
			catch {
			}
		}
	}
}
