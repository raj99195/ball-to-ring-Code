using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool gm_running = true;
    public bool isPaused = false;
    public GameObject ring_vfx;
    BallCreter ballCreter_script;
    
    int curent_step;
    float spd;
    Color gm_color;
    Vector2 targetPoint;
    Gm_Maneger Gm_Maneger_script;
    GameObject ring; // ring of move ball to it.
    bool go_to_ring  = false; 
    void Start()
    {
        
        targetPoint = new Vector2(transform.position.x, -6f);

        Gm_Maneger_script = GameObject.FindGameObjectWithTag("gm_maneger").GetComponent<Gm_Maneger>();

        ballCreter_script = gameObject.GetComponentInParent<BallCreter>();
        


        gm_color = GetComponent<SpriteRenderer>().color;
        GetComponent<TrailRenderer>().colorGradient = createGradiantColorforTrail(gm_color);

        curent_step = ballCreter_script.curent_step;
        switch (curent_step) {
            case 1: {
                    spd = UnityEngine.Random.Range(ballCreter_script.min_spd_1, ballCreter_script.max_spd_1);
            }
                break; 
            case 2: {
                    spd = UnityEngine.Random.Range(ballCreter_script.min_spd_2, ballCreter_script.max_spd_2);
                }
                break;
            case 3: {
                    spd = UnityEngine.Random.Range(ballCreter_script.min_spd_3, ballCreter_script.max_spd_3);
                }
                break;
            case 4: {
                    spd = UnityEngine.Random.Range(ballCreter_script.min_spd_4, ballCreter_script.max_spd_4);
                }
                break;
            case 5: {
                    spd = UnityEngine.Random.Range(ballCreter_script.min_spd_5, ballCreter_script.max_spd_5);
                }
                break;
            case 6: {
                    spd = UnityEngine.Random.Range(ballCreter_script.min_spd_6, ballCreter_script.max_spd_6);
                }
                break;
        }

        
    }




    Gradient createGradiantColorforTrail(Color ball_color)
    {
        Gradient gradient = new Gradient();

        GradientAlphaKey[] alphaKey = new GradientAlphaKey[3];
       
        alphaKey[0].time = 0.0f;
        alphaKey[1].time = 0.3f;
        alphaKey[2].time = 1.0f;

        alphaKey[0].alpha = 0.8f;
        alphaKey[1].alpha = 0.7f;
        alphaKey[2].alpha = 0.0f;
       
        GradientColorKey[] colorKey = new GradientColorKey[2];
        colorKey[0].color = ball_color;
        colorKey[1].color = Color.black;
        colorKey[0].time = 0.0f;
        colorKey[1].time = 1.0f;

        
        gradient.SetKeys(colorKey , alphaKey);
        return gradient;
    }

    void Update()
    {

       if (gm_running)
       {
           if (go_to_ring)
           {
               transform.position = ring.transform.position;
           }
           else
           {

                transform.position -= new Vector3(0, spd * Time.deltaTime,0);
               //transform.position = new Vector2(transform.position.x, transform.position.y - spd *Time.deltaTime);
           }
       }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "ball")
        {
            if (collision.GetComponent<SpriteRenderer>().color == gm_color)
            {
                go_to_ring = true;
                ring = collision.gameObject;
                GameObject ring_vfx_instance = Instantiate(ring_vfx);
                ring_vfx_instance.transform.position = ring.transform.position;
                ring_vfx_instance.GetComponent<SpriteRenderer>().color = ring.GetComponent<SpriteRenderer>().color;
                GetComponent<Animator>().SetTrigger("start_anim");
                gameObject.tag = "ball_to_ring";
                //Destroy(collision.gameObject);
                Gm_Maneger_script.add_score(1);
            }
            else if (collision.GetComponent<SpriteRenderer>().color != gm_color)
            {
                //Time.timeScale = 0;
                AudioManeger.instance.play("ball to ring false");
                ring = collision.gameObject;
                GameObject ring_vfx_instance = Instantiate(ring_vfx);
                ring_vfx_instance.transform.position = ring.transform.position;
                ring_vfx_instance.GetComponent<SpriteRenderer>().color = ring.GetComponent<SpriteRenderer>().color;
                gameObject.tag = "ball_to_ring_false";
                GetComponent<Animator>().SetTrigger("ball_to_false_ring");
                
                Gm_Maneger_script.lose();
            }
        }
    GetComponent<TrailRenderer>().time = 0;
    }
    /// <summary>
    /// in destroy anim
    /// </summary>
    public void setSpd_0()
    {
        spd = 0;
    }

    /// <summary>
    /// called in end of anim
    /// </summary>
    public void desttroy()
    {
        //Debug.Log("destroyyyyyyyyyyyyy");
        Destroy(gameObject);
    }
}
