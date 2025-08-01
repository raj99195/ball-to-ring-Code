using UnityEngine;

public class BallCreter : MonoBehaviour {
    public bool gm_runing;

    public bool canCreate = false;
    public bool scaling = false;

    public GameObject ball_prefab;
    public GameObject ball_path_maneger;

    public GameObject main_Ring;
    float main_ring_first_scale_x;
    float radius;
    Main_Ring main_Ring_script;

    public float min_spd_1, max_spd_1, min_spd_2, max_spd_2, min_spd_3, max_spd_3, min_spd_4, max_spd_4, min_spd_5, max_spd_5, min_spd_6, max_spd_6;
    public float first_time_1, first_time_2, first_time_3, first_time_4, first_time_5, first_time_6;
    public int curent_step;

    
    public float miner_time_1, miner_time_2, miner_time_3, miner_time_4, miner_time_5, miner_time_6; // They reduce the time it takes to build balls
    float new_miner_time = 0;
    float[] x_posations;
    
    float time;

    public int numDupClrBall;


    void Start() {
        

        miner_time_1 = miner_time_1 * Time.deltaTime;
        miner_time_2 = miner_time_2 * Time.deltaTime;
        miner_time_3 = miner_time_3 * Time.deltaTime;
        miner_time_4 = miner_time_4 * Time.deltaTime;
        miner_time_5 = miner_time_5 * Time.deltaTime;
        miner_time_6 = miner_time_6 * Time.deltaTime;

        first_time_1 = first_time_1 * Time.deltaTime;
        first_time_2 = first_time_2 * Time.deltaTime;
        first_time_3 = first_time_3 * Time.deltaTime;
        first_time_4 = first_time_4 * Time.deltaTime;
        first_time_5 = first_time_5 * Time.deltaTime;
        first_time_6 = first_time_6 * Time.deltaTime;

        main_Ring_script = main_Ring.GetComponent<Main_Ring>();
        main_ring_first_scale_x = main_Ring_script.main_ring_first_scale_x;
        radius = main_Ring_script.radius;
        time = 0;
    }

    public void remove_Childs(bool fade)
    {
        
        int Childes = transform.childCount;
        for (int i = 0; i < Childes; i++)
        {
            if(transform.GetChild(i).tag == "ball") // for ball's not collision to rings
            {
                if (fade) {
                    transform.GetChild(i).GetComponent<Animator>().SetTrigger("remove");
                }
                else {
                    Destroy(transform.GetChild(i).gameObject);
                }
                
            }
            
        }

    }

    GameObject ball;
    int a = 60;
    void Update() {

       if (canCreate  && !scaling && gm_runing)
       {
               
           switch (curent_step) {
       
               case 1: {
                       time--;
                       if(time <= 0) {
                            ball = Instantiate(ball_prefab, new Vector3(x_posations[Random.Range(0, x_posations.Length)], transform.position.y, 0), Quaternion.Euler(0, 0, 0), transform);
                            setBallColor();
                            new_miner_time += miner_time_1;
                            time = first_time_1 - new_miner_time;
                           
                       }
                   }
                   break;
               case 2: {
                       time--;
                       if (time <= 0)
                       {
                            ball = Instantiate(ball_prefab, new Vector3(x_posations[Random.Range(0, x_posations.Length)], transform.position.y, 0), Quaternion.Euler(0, 0, 0), transform);
                            setBallColor();
                            new_miner_time += miner_time_2;
                            time = first_time_2 - new_miner_time;
                           
                       }
       
                   }
                   break;
               case 3: {
                       time--;
                       if (time <= 0)
                       {
                            ball = Instantiate(ball_prefab, new Vector3(x_posations[Random.Range(0, x_posations.Length)], transform.position.y, 0), Quaternion.Euler(0, 0, 0), transform);
                            setBallColor();
                            new_miner_time += miner_time_3;
                            time = first_time_3 - new_miner_time;
                           
                       }
                   }
                   break;
               case 4: {
                       time--;
                       if (time <= 0)
                       {
                            ball = Instantiate(ball_prefab, new Vector3(x_posations[Random.Range(0, x_posations.Length)], transform.position.y, 0), Quaternion.Euler(0, 0, 0), transform);
                            setBallColor();
                            new_miner_time += miner_time_4;
                            time = first_time_4 - new_miner_time;
                           
                       }
                   }
                   break;
               case 5: {
                       time--;
                       if (time <= 0)
                       {
                            ball = Instantiate(ball_prefab, new Vector3(x_posations[Random.Range(0, x_posations.Length)], transform.position.y, 0), Quaternion.Euler(0, 0, 0), transform);
                            setBallColor();
                            new_miner_time += miner_time_5;
                            time = first_time_5 - new_miner_time;
                           
                       }
                   }
                   break;
               case 6: {
                       time--;
                       if (time <= 0)
                       {
                            ball = Instantiate(ball_prefab, new Vector3(x_posations[Random.Range(0, x_posations.Length)], transform.position.y, 0), Quaternion.Euler(0, 0, 0), transform);
                            setBallColor();
                            new_miner_time += miner_time_6;
                            time = first_time_6 - new_miner_time;
                           
                       }
                   }
                   break;
       
       
                  
           }
           
       }
    }

    public void change_step()
    {
        x1 = 0;
        new_miner_time = 0;
        float angel;
        switch (curent_step)
        {
            case 1:
                {
                    angel = 90;
                    x_posations = new float[1];
                    time = first_time_1;
                    x_posations[0] = main_ring_first_scale_x * radius * (Mathf.Cos( angel * Mathf.Deg2Rad));

                }
                break;
            case 2:
                {
                    angel = 30;
                    x_posations = new float[2];
                    time = first_time_2;
                    x_posations[0] = main_ring_first_scale_x * radius * (Mathf.Cos(angel * Mathf.Deg2Rad));
                    x_posations[1] = -1 * main_ring_first_scale_x * radius * (Mathf.Cos(angel * Mathf.Deg2Rad));
                    

                }
                break;
            case 3:
                {
                    angel = 90;
                    x_posations = new float[3];
                    time = first_time_3;
                    x_posations[0] = main_ring_first_scale_x * radius * (Mathf.Cos(angel * Mathf.Deg2Rad));
                    x_posations[1] = main_ring_first_scale_x * radius * (Mathf.Cos((angel -120f) * Mathf.Deg2Rad));
                    x_posations[2] = -1 * main_ring_first_scale_x * radius * (Mathf.Cos((angel -120f) * Mathf.Deg2Rad));

                }
                break;
            case 4:
                {
                    angel = 90;
                    x_posations = new float[1];
                    time = first_time_4;
                    x_posations[0] = -1 * main_ring_first_scale_x * radius * (Mathf.Cos(angel * Mathf.Deg2Rad));
                }
                break;
            case 5:
                {
                    angel = 45;
                    x_posations = new float[2];
                    time = first_time_5;
                    x_posations[0] = -1 * main_ring_first_scale_x * radius * (Mathf.Cos(angel * Mathf.Deg2Rad));
                    x_posations[1] =     main_ring_first_scale_x * radius * (Mathf.Cos(angel * Mathf.Deg2Rad));
                }
                break;
            case 6:
                {
                    angel = 0;
                    x_posations = new float[3];
                    time = first_time_6;
                    x_posations[0] = main_ring_first_scale_x * radius * (Mathf.Cos(angel * Mathf.Deg2Rad));
                    x_posations[1] = -1 * main_ring_first_scale_x * radius * (Mathf.Cos(angel * Mathf.Deg2Rad));
                    x_posations[2] = main_ring_first_scale_x * radius * (Mathf.Cos((angel - 90f) * Mathf.Deg2Rad));
                }
                break;
        }
    }

    int x1 = 0;
    int lastBallColorId = 0 ;
    void setBallColor() {
        int c1 ;
        Color[] colors;
        colors = main_Ring_script.GetComponent<Main_Ring>().color_seted;
        c1 = UnityEngine.Random.Range(0, colors.Length - 1);
        if(c1 == lastBallColorId) {
            if(x1 >= numDupClrBall) {
                while (c1 == lastBallColorId) {
                    c1 = UnityEngine.Random.Range(0, colors.Length - 1);
                }
                x1 = 0;
            }
            else {
                x1++;
            }
        }
        print("x1 is : "+x1);
        ball.GetComponent<SpriteRenderer>().color = colors[c1];
        lastBallColorId = c1;
        
        
    }

    public void setGmRunning(bool gmRunning)
    {
        gm_runing = gmRunning;
        
        foreach(Ball ball_script in GetComponentsInChildren<Ball>())
        {
            
            ball_script.gm_running = gmRunning;
        }
    }

    public void setPaused(bool isPaused)
    {
        foreach (Ball ball_script in GetComponentsInChildren<Ball>())
        {
            ball_script.isPaused = isPaused;
        }

        string action = "";

        if (isPaused)
            action = "pause";
        else
            action = "run";

        foreach (Animator ballAnimator in GetComponentsInChildren<Animator>())
        {
            ballAnimator.SetTrigger(action);
        }
    }
}
