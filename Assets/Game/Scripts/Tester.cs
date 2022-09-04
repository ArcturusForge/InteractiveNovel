using Arcturus.InteractiveNovel;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
    public string playwrightPath;
    public RectTransform chaParent;

    public Image speakerGraphic;
    public TMP_Text nameDisplay;
    public TMP_Text contentDisplay;

    private bool hasChoice;
    private List<KeyValuePair<string, int>> choices;

    private void Awake()
    {
        SequenceManager.OnDisplayDialogue += PrintDialogue;
        SequenceManager.OnDisplayChoices += PrintChoices;
    }

    private void Start()
    {
        SequenceParser.ParseFile(playwrightPath);
    }

    private void Update()
    {
        // TODO: Implement player choices.
        if (Input.GetKeyDown(KeyCode.Space) && !hasChoice)
            SequenceManager.PlayNextLine();

        if (hasChoice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
                ConfirmChoice(0);

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                ConfirmChoice(1);

            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                ConfirmChoice(2);

            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                ConfirmChoice(3);

            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                ConfirmChoice(4);
        }
    }

    private void PrintDialogue(string name, string content, Sprite pose, string position)
    {
        nameDisplay.text = name;
        contentDisplay.text = content;

        if (name.ToLower() != "<playername>")
            speakerGraphic.overrideSprite = pose;
    }

    private void PrintChoices(List<KeyValuePair<string, int>> choices)
    {
        this.choices = choices;
        Debug.Log("Select Option:");
        for (int i = 0; i < choices.Count; i++)
            Debug.Log($"Press [{i}] || {choices[i].Key}");
        hasChoice = true;
    }

    private void ConfirmChoice(int index)
    {
        if (choices.Count <= index)
            return;

        SequenceManager.PlaySequence(choices[index].Value);
        choices.Clear();
        hasChoice = false;
    }
}
