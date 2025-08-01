using UnityEngine;

public class VfxManeger : MonoBehaviour
{
    
    public GameObject win_vfx_R, win_vfx_L;
    ParticleSystem win_vfx_R_ps, win_vfx_L_ps;
    public Gm_Maneger gm_Maneger_script;
    long[] vfxPaternVibrate = { 50,50,50 };

    private void Awake() {
        win_vfx_L_ps = win_vfx_L.GetComponent<ParticleSystem>();
        win_vfx_R_ps = win_vfx_R.GetComponent<ParticleSystem>();
        win_vfx_L_ps.Stop();
        win_vfx_R_ps.Stop();
    }

    public void plyWinVfx()
    {
        
        win_vfx_L_ps.Play();
        win_vfx_R_ps.Play();
        AudioManeger.instance.play("win vfx");
        Invoke("PlayVibrate", 0.2f);
    }

   public void PlayVibrate() {

        if (gm_Maneger_script.canVibrate) {
            try {
                Vibration.Vibrate(vfxPaternVibrate, -1);
            }
               catch {
            }
        }
   }

}
