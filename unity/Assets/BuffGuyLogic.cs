using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffGuyLogic : MonoBehaviour,
                            UnityEngine.EventSystems.IPointerDownHandler,
                            UnityEngine.EventSystems.IPointerUpHandler
{
    public GameObject main_scene;

    public void Start()
    {
        foreach (UnityEngine.Component comp in this.GetComponents<UnityEngine.Component>())
        {
            UnityEngine.Debug.Log("Buff Guy: " + comp.GetType().FullName);
        }
    }

    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData data)
    {
        // don't do anything
    }

    public void OnPointerUp(UnityEngine.EventSystems.PointerEventData data)
    {
        this.main_scene.GetComponent<MainSceneScript>().AddRep();

        UnityEngine.Animator anim = this.GetComponent<Animator>();
        int to_lift = anim.GetInteger("to_lift") + 1;
        anim.SetInteger("to_lift", to_lift);     // increment amount to lift

        anim.speed += UnityEngine.Mathf.Clamp((to_lift - anim.speed) * 0.5f, -99.0f, 0.3f);
                    // clamped acceleration of lifting speed, unclamped deceleration
    }
}
