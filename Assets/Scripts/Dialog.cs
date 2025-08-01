using UnityEngine;

public class Dialog : MonoBehaviour
{
	public admobLauncher admobLuncher;
	public BallCreter ballCreteScript;
	public VfxManeger vfxManeger_script;
	public Gm_Maneger gm_Maneger_script;
	public GameObject score_text;
	public GameObject countDownLable;
	public GameObject main_ring  ;
	

	public BallPathManeger BallPathManeger_script;
	public void reset_after_lose()
	{
		
		gm_Maneger_script.reset_after_lose();
	}

	public void reset_After_Pause()
	{
		gm_Maneger_script.reset_After_Pause();
	}

	
	public void viewd_Rings_and_Paths(int Viewer)
	{
		bool view = false; 
		if(Viewer == 1)
		{
			view = true;
		}


		string action = "";
		if (view)
		{
			action = "view";
		}
		else
		{
			action = "hiden";
		}
		
		
		Animator[] ring_s_Animator = main_ring.GetComponentsInChildren<Animator>();
		for(int i = ring_s_Animator.Length-1; i>0; i--)
		{
			ring_s_Animator[i].SetTrigger(action);
		}
		

		BallPathManeger_script.canViwed = view;
		BallPathManeger_script.path_viwer(view , false);
		

	}
	public void runWinEfect()
	{
		vfxManeger_script.plyWinVfx();   
	} 

	public void startStep(int step)
	{
		
		gm_Maneger_script.start_step(1);
	}

	public void setCountDownLableNumber(int number)
	{
		countDownLable.GetComponent<UnityEngine.UI.Text>().text = number.ToString();
	}

	public void resumeGame()
	{
		gm_Maneger_script.resumeGame();
	}

	public void showAds() {
		// Comment for html output
		admobLuncher.showads();
	}

	public void resetAds() {
		// Comment for html output
		admobLuncher.sendRequest();
	}


	#region play Sounds

	public void playAnimDot_sound() {
		AudioManeger.instance.play("ball to ring true");
	}
	public void playCrownCollision_sound() {
		AudioManeger.instance.play("crown down");
	}
	public void playCrownRotate_sound() {
		AudioManeger.instance.play("crown rotate");
	}
	public void newRecord_Sound() {
		AudioManeger.instance.play("new record");
	}


    #endregion
}
