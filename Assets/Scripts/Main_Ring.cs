using System.Collections;
using UnityEngine;



public class Main_Ring : MonoBehaviour {
	public int curent_step ;
	public int curent_ring ;

	public bool scaling = false;
	public GameObject prefab_rings;
	public GameObject pathmaneger;
	BallPathManeger pathManeger_script;
	public GameObject ball_Creater;
	BallCreter ball_creater_script;
	MainRing_Controler mainRing_Controler_script;
	public float radius;
	public Gm_Maneger gm_Maneger;
	public Bg_Controler bg_Controler_script;
	public Color[] curent_color;
	public Color[] color_seted;  // Colors of Balls
	public float main_ring_first_scale_x; // for the create other Object : Ball Path , Ball , ...
	int Childs_number;
	ArrayList number_for_Create_random_color = new ArrayList(); // array for No repetition Random Number
	public float angel;
	public float spd_for_rotate;

	public bool can_Rotate , can_req_to_rotate; 


	void Start () {
		pathManeger_script = pathmaneger.GetComponent<BallPathManeger>();
		main_ring_first_scale_x = transform.localScale.x;
		ball_creater_script = ball_Creater.GetComponent<BallCreter>();
		mainRing_Controler_script = transform.parent.GetComponent<MainRing_Controler>();
		can_Rotate = false;
		can_req_to_rotate = true;
		
}


	
	/// <summary>
	/// function for create the Rings by ring int
	/// </summary>
	/// <param name="curent_ring"> Number for Create Rings </param>
	/// <param name="step"> for rotate MainRing in Specefide Angel </param>
	public void Create_Ring(){
		
		if(curent_step == 1 || curent_step == 2 || curent_step == 3)
		{
			curent_ring = 3;
		}else if (curent_step == 4 || curent_step == 5 || curent_step == 6)
		{
			curent_ring = 4;
		}
		
		transform.rotation = Quaternion.Euler(0, 0, 0);
		float alpha = 360 / curent_ring;
		float main_ring_Distanse_from_center = transform.position.y;
		Childs_number = transform.childCount;

		// When First  Requested 
		if ( Childs_number == 0){ 
			for (int i = 0; i < curent_ring; i++)
			{
				Instantiate(prefab_rings, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), transform);
			}
		}

		// for Create the numer 4 Ring
		GameObject ring4 = null;
		if (Childs_number <= 3 && curent_step >= 4){
			ring4 = Instantiate(prefab_rings , new Vector3(0,0,0) , Quaternion.Euler(0,0,0) , transform );
			if(ring4.GetComponent<SpriteRenderer>().color.a<0.1f)
				ring4.GetComponent<Animator>().SetTrigger("view");
		}

		// for destroy number 4 Ring
		if (Childs_number >= 4)
		{
			if(curent_step == 1 || curent_step == 2 || curent_step == 3)
			{
				for(int i = 3 ; i < Childs_number ; i++)
				{
					Destroy(transform.GetChild(i).gameObject);
				}
			}
		}

		// Set The Rings in distans & angel specefied with MainRing 
		float angel;
		for (int d = 0; d < curent_ring ; d++ ){
			angel = alpha * d;
			transform.GetChild(d).transform.position = new Vector3
				(
				transform.localScale.x * radius * (Mathf.Cos(angel * Mathf.Deg2Rad)),
				transform.localScale.x * radius * (Mathf.Sin(angel * Mathf.Deg2Rad)) + main_ring_Distanse_from_center ,
				0
				);
		}

		
		number_for_Create_random_color.Clear();
		
		// Rotate MainRing to specified angel
		switch (curent_step)
				{
					case 1:
						transform.rotation = Quaternion.Euler(0, 0, 90.0f);
						//print(55555);
						break;
					case 2:
						transform.rotation = Quaternion.Euler(0, 0, 30.0f);
						break;
					case 3:
						transform.rotation = Quaternion.Euler(0, 0, +90.0f);
						break;
					case 4:
						transform.rotation = Quaternion.Euler(0, 0, 0.0f);
						break;
					case 5:
						transform.rotation = Quaternion.Euler(0, 0, 45.0f);
						break;
					case 6:
						transform.rotation = Quaternion.Euler(0, 0, 0.0f);
						break;
				}


		for (int i = 0; i < curent_color.Length; i++)
		{
			number_for_Create_random_color.Add(i);
		}

		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetComponent<SpriteRenderer>().color = curent_color[create_random_number_for_color()];
		}

		for (int i=0; i<transform.childCount; i++)
		{
			color_seted[i] = transform.GetChild(i).GetComponent<SpriteRenderer>().color;
		}

	}


	/// <summary>
	/// create random number for select Color
	/// </summary>
	/// <returns></returns>
	public int create_random_number_for_color()
	{
		int a = UnityEngine.Random.Range(0 , number_for_Create_random_color.Count );
		int selected = (int) number_for_Create_random_color[a];
		number_for_Create_random_color.Remove(selected);
		return selected;
	}


	/// <summary>
	/// Called in Anim for Chenge Path's
	/// </summary>
	void start_path_create()
	{
		pathManeger_script.pathControler(curent_step);
	}


	/// <summary>
	/// Called in Anim For Chenge  Place's of Ball Create
	/// </summary>
	void chenge_stete_can_ball_Create()
	{
		ball_creater_script.change_step();
	}
	

	public void set_scaling_bool(int scaling)
	{
		if(scaling == 1)
		{
			if (transform.childCount != 0) {
				if (transform.GetChild(0).GetComponent<SpriteRenderer>().color.a >= 0.9f) {
				AudioManeger.instance.play("scaling");
				}
			}
			this.scaling = true;
		}
		else if(scaling == 0)
		{
			this.scaling = false;
		}

		send_scaling_bool_to_other_scripts();
	}

	void send_scaling_bool_to_other_scripts()
	{
		ball_creater_script.scaling = scaling;
		ball_creater_script.remove_Childs(true);
		mainRing_Controler_script.can_tuch_to_rotate = !scaling;
	}


	/// <summary>
	/// Called Anim for chengh backGround & chenge Rings and Balls Colors
	/// </summary>
	public void set_bg_img_and_Colors_ring()
	{
		bg_Controler_script.setBg();
		curent_color = bg_Controler_script.curent_ring_colors;
	}
	


}
