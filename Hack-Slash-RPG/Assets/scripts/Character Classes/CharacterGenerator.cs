using UnityEngine;
using System.Collections;
using System; // used for Enum Class

public class CharacterGenerator : MonoBehaviour
{
    private PlayerCharacter _toon;


    // Use this for initialization
    void Start()
    {
        _toon = new PlayerCharacter();
        _toon.Awake();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        DisplayName();
        DisplayAttributes();
        DisplayVitals();
        DisplaySkills();
    }

    private void DisplayName()
    {
        GUI.Label(new Rect(10, 10, 50, 25), "Name");
        _toon.Name = GUI.TextArea(new Rect(65, 10, 100, 25), _toon.Name);
    }

    private void DisplayAttributes()
    {
        for (int i = 0; i < Enum.GetValues(typeof(AttributeName)).Length; i++)
        {
            GUI.Label(new Rect(10, 40 + (i * 25), 100, 25), ((AttributeName)i).ToString());
            GUI.Label(new Rect(115, 40 + (i * 25), 30, 25), _toon.GetPrimaryAttribute(i).AdjustedBaseValue.ToString());
        }
    }

    private void DisplayVitals()
    {
        for (int i = 0; i < Enum.GetValues(typeof(VitalName)).Length; i++)
        {
            GUI.Label(new Rect(10, 40 + ((i + 7) * 25), 100, 25), ((VitalName)i).ToString());
            GUI.Label(new Rect(115, 40 + ((i + 7) * 25), 30, 25), _toon.GetVital(i).AdjustedBaseValue.ToString());
        }
    }

    private void DisplaySkills()
    {
        // ÄÎ ÒÓÊ ÑÚÌ ÑÒÈÃÍÀË 23 åïèçîä BurgZerger!
    }
}
