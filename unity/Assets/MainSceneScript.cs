using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;

public class MainSceneScript : MonoBehaviour
{
    public GameObject reps_counter;
    public GameObject countdown_text;
    public GameObject ui_canvas;
    public GameObject buff_guy;
    

    private int score;
    private const string message = "Reps: ";

    private GameObject canvas;

    public GameObject calendar_prefab;
    private struct CalendarDay {
        public int day_of_year;
        public GameObject game_obj;
        public void destroy_game_obj() {
            UnityEngine.Object.Destroy(this.game_obj);
        }
    };
    private List<CalendarDay> calendar_days = new List<CalendarDay>();

    private int last_lift_day = 365;

    private float scroll_start_time;
    private float countdown_duration = 3.0F;        // 3 seconds for countdown
    private float lifting_duration = 60.0F;      // 60 seconds to get SWOLE
    private float lifting_end;

    public void Awake()
    {
        UnityEngine.Debug.Log("AWAKE");
    }

    public void Start()
    {
        UnityEngine.Debug.Log("STARTED");

        this.canvas = UnityEngine.GameObject.Find("Canvas");

        /*GameObject premade = UnityEngine.GameObject.Find("CalendarDay (1)");
        foreach(UnityEngine.Component comp in premade.GetComponents<UnityEngine.Component>()) {
            UnityEngine.Debug.Log("Calendar: " + comp.GetType().FullName);
        }

        GameObject cntr = UnityEngine.GameObject.Find("RepCounter");
        foreach (UnityEngine.Component comp in cntr.GetComponents<UnityEngine.Component>()) {
            UnityEngine.Debug.Log("RepCounter: " +comp.GetType().FullName);
        }*/

        this.score = 0;
        this.reps_counter.GetComponent<TextMeshProUGUI>().text = message;

        this.StartCoroutine(this.countdown_to_lift());
    }

    private IEnumerator countdown_to_lift()
    {
        yield return new UnityEngine.WaitForSecondsRealtime(1.0f);      // wait for Unity to load

        this.scroll_start_time = UnityEngine.Time.unscaledTime;      // will actually start in 3 seconds
        this.StartCoroutine(this.scroll_calendar());

        this.countdown_text.SetActive(true);
        this.countdown_text.GetComponent<TextMeshProUGUI>().text = "3";
        this.countdown_text.GetComponent<Animator>().Play("CountdownShrinking");
        yield return new UnityEngine.WaitForSecondsRealtime(1.0f);
        this.countdown_text.GetComponent<TextMeshProUGUI>().text = "2";
        this.countdown_text.GetComponent<Animator>().Play("CountdownShrinking");
        yield return new UnityEngine.WaitForSecondsRealtime(1.0f);
        this.countdown_text.GetComponent<TextMeshProUGUI>().text = "1";
        this.countdown_text.GetComponent<Animator>().Play("CountdownShrinking");
        yield return new UnityEngine.WaitForSecondsRealtime(1.0f);
        this.countdown_text.GetComponent<TextMeshProUGUI>().text = "Lift!";
        this.countdown_text.GetComponent<Animator>().Play("CountdownShrinking");
        yield return new UnityEngine.WaitForSecondsRealtime(2.0f);     // wait few more sec for text to fade
        this.countdown_text.SetActive(false);
        yield break;
    }

    private IEnumerator scroll_calendar()
    {
        while (true)                            // scroll calendar forever!
        {
            //Time.unscaledTime - this.lifting_start_time

            float scroll_time = (UnityEngine.Time.unscaledTime - this.scroll_start_time) / (this.lifting_duration + this.countdown_duration);

            float scale_in_calendar = this.calc_calendar_pos_given_time(scroll_time);
                            // this *WILL* be before day 0 when we start, so scale will be negative

            int start_day = (int)System.Math.Floor(scale_in_calendar) - 4;
            int end_day = (int)System.Math.Floor(scale_in_calendar) + 4;

            while (this.calendar_days.Count > 0 && this.calendar_days[0].day_of_year < start_day)
            {
                this.calendar_days[0].destroy_game_obj();                   // drop days we passed
                this.calendar_days.RemoveAt(0);
            }

            while (this.calendar_days.Count == 0 || this.calendar_days.Last().day_of_year < end_day)
            {
                CalendarDay calendar_day = new CalendarDay();                           // build upcoming days
                if (this.calendar_days.Count == 0) {
                    calendar_day.day_of_year = start_day;
                }
                else {
                    calendar_day.day_of_year = this.calendar_days.Last().day_of_year + 1;
                }
                if (0 <= calendar_day.day_of_year && calendar_day.day_of_year <= this.last_lift_day) {            // only make day 0+ visible
                    calendar_day.game_obj = UnityEngine.Object.Instantiate(this.calendar_prefab, this.canvas.transform);
                    Transform date = calendar_day.game_obj.transform.GetChild(1);
                    Assert.IsTrue(date.gameObject.name == "Date");
                    date.gameObject.GetComponent<TextMeshProUGUI>().text = (calendar_day.day_of_year + 1).ToString();
                                                                // +1 since days don't start at 0 like indices
                }
                this.calendar_days.Add(calendar_day);
            }

            for (int cur_day = start_day; cur_day <= end_day; ++cur_day)
            {                                                               // place days as they fly past
                Assert.AreEqual(cur_day, this.calendar_days[cur_day - start_day].day_of_year);
                if (this.calendar_days[cur_day - start_day].game_obj == null) {
                    continue;                           // skip drawing days before 0
                }
                RectTransform trans = this.calendar_days[cur_day - start_day].game_obj.GetComponent<RectTransform>();
                trans.anchoredPosition = new Vector2((cur_day - scale_in_calendar) * 120, trans.anchoredPosition.y);
            }

            //foreach (CalendarDay day in this.calendar_days) {
            //    RectTransform trans = day.game_obj.GetComponent<RectTransform>();
            //    trans.anchoredPosition = new Vector2(trans.anchoredPosition.x - 0.005f, trans.anchoredPosition.y);
            //}

            yield return null;
        }
    }

    private float calc_calendar_pos_given_time(float scale)          // [0,1] for gamplay duration
    {
        // want it slower at start, faster at end
        // quadratic graph (which gives constant acceleration), but pick some better
        // points to make it smoother
        //                                     ^           o
        // start x = 0, y = 0                  |        _./
        // mid   x = 0.5, y = s                |   __o''
        // end   y = 1, y = 1                  o--'--------->
        //
        //     y = ax^2 + bx + c
        //
        //  0 = a0^2 + b0 + c
        //  0 = c
        //
        //  1 = a1^2 + b1 + 0
        //  1 = a + b
        //  b = 1 - a
        //
        //  s = a0.5^2 + b0.5 + 0
        //  s = 0.25a + 0.5b
        //  s = 0.25a + 0.5(1 - a)
        //  s = 0.25a + 0.50 - 0.50a
        //  0.25a = 0.50 - s
        //  a = (0.5 - s) / 0.25

        const float s = 0.285f;
        const float a = (0.5f - s) / 0.25f;
        const float b = 1f - a;
        const float c = 0.0f;

        float day0_time_scale = this.countdown_duration / (this.countdown_duration + this.lifting_duration);
        float day0_new_scale = a * day0_time_scale * day0_time_scale + b * day0_time_scale + c;

        float new_scale = a * scale * scale + b * scale + c;

        return (new_scale - day0_new_scale) / (1.0f - day0_new_scale) * last_lift_day;     // scale to hit day 0 precisely
    }

    public void AddRep()
    {
        this.score++;
        this.reps_counter.GetComponent<TextMeshProUGUI>().text = message + this.score;
    }

    public void Update()
    {
        
    }
}
