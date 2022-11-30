using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffGuyLogic : MonoBehaviour,
                            UnityEngine.EventSystems.IPointerDownHandler,
                            UnityEngine.EventSystems.IPointerUpHandler
{
    public GameObject main_scene;

    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData data)
    {
        // don't do anything
    }

    public void OnPointerUp(UnityEngine.EventSystems.PointerEventData data)
    {
        this.main_scene.GetComponent<MainSceneScript>().AddRep();
    }
}
