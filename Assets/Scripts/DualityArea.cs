using UnityEngine;

public class DualityArea : MonoBehaviour
{
    [SerializeField] private DualityState inState;
    [SerializeField] private DualityState outState;
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        IDuality duality = other.GetComponentInParent<IDuality>();
        
        if (duality != null)
        {
            duality.CurrentDualityState = inState;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        IDuality duality = other.GetComponentInParent<IDuality>();
        
        if (duality != null)
        {
            duality.CurrentDualityState = outState;
        }
    }
}
