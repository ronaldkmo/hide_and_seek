using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerUI;

    float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += 1 * Time.deltaTime;
        timerUI.text = SecondsToTimeString(time);
    }

    string SecondsToTimeString(float secondsOnly)
	{
        int minutes = (int) secondsOnly / 60;
        int seconds = (int) (secondsOnly % 60);

        return minutes + ":" + seconds.ToString("00");
    }
}
