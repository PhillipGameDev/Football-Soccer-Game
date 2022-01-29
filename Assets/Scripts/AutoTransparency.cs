using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTransparency : MonoBehaviour
{
    [Range(0,1)]
    public float transparency;
    public Renderer targetRenderer;
    public float fadeDuration;
   private void OnTriggerEnter2D(Collider2D other) {
       if(other.CompareTag("Player")){
           SetTransparency(transparency);
       }
   }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
           SetTransparency(1.0f);
       }
    }
   private void SetTransparency(float alpha){
       StopCoroutine("FadeCoroutine");
       StartCoroutine("FadeCoroutine",alpha);
   }

   private IEnumerator FadeCoroutine(float fadeTo){
       float timer = 0;
       Color current_color = targetRenderer.material.color;
       float startAlpha =targetRenderer.material.color.a;
       while(timer < 1){
           yield return new WaitForEndOfFrame();
           timer += Time.deltaTime/fadeDuration;
           current_color.a = Mathf.Lerp(startAlpha,fadeTo,timer);
           targetRenderer.material.color = current_color;

       }
   }
}
