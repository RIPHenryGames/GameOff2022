using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



// Main central canvas
public class CanvasClicking : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);    // always set (global)
        //RaycastHit2D[] hits = new RaycastHit2D[10];            // all the hits on the cursor
        //int numHits = Physics2D.Raycast(worldPos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);   // only checks colliders in physics
        if (hit)
        {
            Debug.Log("Hit: ");
            //counter.AddRep();
        }
    }

    //CounterBehavior counter;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start....");

        GameObject c = GameObject.FindWithTag("Counter");

        //if(c != null) {
        //  counter = c.GetComponent<CounterBehavior>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
    }


}
