using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Arcturus.InteractiveNovel
{
    public class SequenceManager : MonoBehaviour
    {
        private static SequenceManager I;

        private int sequenceIndex = 0;
        private int lineIndex = 0;
        private DialogueSequence currentSequence;
        private List<SequenceCommandBase> sequenceCommands;

        public static Action OnPlaywrightEnded;
        public static Action<string, string, Sprite, string> OnDisplayDialogue;
        public static Action<List<KeyValuePair<string, int>>> OnDisplayChoices;

        private void Awake()
        {
            #region Static Referencing
            if (I == null) I = this;
            else if (I != this) Destroy(gameObject);
            #endregion

            SequenceParser.OnParsingCompleted += GetNextSequence;

            // Dynamically load all sequence commands.
            sequenceCommands = new List<SequenceCommandBase>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var typeList = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(SequenceCommandBase))).ToList();

                // Interate through the list of Types.
                foreach (var type in typeList)
                {
                    // Create a temporary instance of these PresetHeader scripts.
                    var instance = (SequenceCommandBase)Activator.CreateInstance(type);

                    // Store temp instance in list.
                    sequenceCommands.Add(instance);
                }
            }
        }

        private void GetNextSequence()
        {
            currentSequence = SequenceParser.GetSequence(sequenceIndex);
            ProceedLine();
        }

        private void EndSequence()
        {
            sequenceIndex++;
            lineIndex = 0;

            foreach (var action in currentSequence.SequenceEndActions)
            {
                var actionData = action.Split(':');
                foreach (var seqComm in sequenceCommands)
                {
                    if (seqComm.MatchCommand(actionData[0]))
                    {
                        seqComm.HandleCommand(ref sequenceIndex, ref lineIndex, actionData[1]);
                        break;
                    }
                }
            }

            if (SequenceParser.HasSequence(sequenceIndex))
                GetNextSequence();
            else
            {
                Debug.LogWarning($"Playwright has ended!");
                OnPlaywrightEnded?.Invoke();
            }
        }

        private void ProceedLine()
        {
            if (currentSequence.HasEnded(lineIndex))
            {
                EndSequence();
                return;
            }

            var lineData = currentSequence.DialogueLines[lineIndex];
            if (lineData.Name.ToLower() == "choice")
            {
                // Choice
                var choices = new List<KeyValuePair<string, int>>();
                foreach (var choice in lineData.Choices)
                    choices.Add(new KeyValuePair<string, int>(choice.Content, choice.Index));
                OnDisplayChoices?.Invoke(choices);
            }
            else if (lineData.Name.ToLower() == "<playername>")
                OnDisplayDialogue?.Invoke(lineData.Name, lineData.Content, null, "");
            else // Dialogue
                OnDisplayDialogue?.Invoke(lineData.Name, lineData.Content, CharacterManager.GetPose(lineData.Name, lineData.Pose), lineData.Position);
        }

        #region System Interfacing
        public static void PlayNextLine()
        {
            I.lineIndex++;
            I.ProceedLine();
        }

        public static void PlaySequence(int sequenceIndex)
        {
            I.lineIndex = 0;
            I.sequenceIndex = sequenceIndex;
            I.GetNextSequence();
        }
        #endregion
    }
}
