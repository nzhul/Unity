using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    Text message;
    Text P1Score;
    Text P2Score;


    // Use this for initialization
    void Start()
    {
        message = transform.Find("CenterMessage").GetComponent<Text>();
        message.enabled = false;
        P1Score = transform.Find("P1Score").GetComponent<Text>();
        P2Score = transform.Find("P2Score").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTempCenterMessage(string text, float seconds)
    {
        message.text = text;
        StartCoroutine(ShowMessage(seconds));
    }

    public void SetPlayerScore(PlayerMeta meta)
    {
        string newText = meta.playerName + ": " + meta.GetScore();
        if (meta.playerID == PlayerID.P1)
            P1Score.text = newText;
        else
            P2Score.text = newText;
    }

    IEnumerator ShowMessage(float seconds)
    {
        message.enabled = true;
        yield return new WaitForSeconds(seconds);
        message.enabled = false;

    }
}
