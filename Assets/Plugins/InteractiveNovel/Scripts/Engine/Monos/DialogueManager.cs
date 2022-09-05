using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arcturus.InteractiveNovel
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager I;

        public KeyCode interactionKey = KeyCode.Space;
        public RectTransform characterParent;
        public RectTransform choiceParent;
        public TMP_Text nameDisplay;
        public TMP_Text contentDisplay;

        private bool hasChoice;

        // Temp
        public Image speakerGraphic;

        private void Awake()
        {
            #region Static Referencing
            if (I == null) I = this;
            else if (I != this) Destroy(gameObject);
            #endregion

            SequenceManager.OnDisplayDialogue += PrintDialogue;
            SequenceManager.OnDisplayChoices += EnableChoices;
        }

        private void Update()
        {
            if (Input.GetKeyDown(interactionKey) && !hasChoice)
                SequenceManager.PlayNextLine();
        }

        private void PrintDialogue(string name, string content, Sprite pose, string position)
        {
            nameDisplay.text = name;
            contentDisplay.text = content;

            if (name.ToLower() != "<playername>")
                speakerGraphic.overrideSprite = pose;
        }

        private void EnableChoices(List<KeyValuePair<string, int>> choices)
        {
            foreach (var choice in choices)
            {
                var opt = Instantiate(Resources.Load<GameObject>("Prefabs/OptionButton"), choiceParent).GetComponent<OptionButtonController>();
                opt.SetOption(choice.Key, choice.Value, ChoiceSelected);
            }
            hasChoice = true;
        }

        private void ChoiceSelected(int sequenceIndex)
        {
            foreach (Transform option in choiceParent)
                Destroy(option.gameObject);
            SequenceManager.PlaySequence(sequenceIndex);
            hasChoice = false;
        }
    }
}
