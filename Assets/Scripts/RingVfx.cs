using UnityEngine;

public class RingVfx : MonoBehaviour
{
    /// <summary>
    /// Called in Anim
    /// </summary>
    public void Destroy() {
        Destroy(gameObject);
    }
}
