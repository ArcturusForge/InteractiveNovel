using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assembly Detection:
/// https://forum.unity.com/threads/how-to-get-all-system-relection-assembly-for-all-module-in-unity.806052/#:~:text=You%20can%20use%20%27System.,loaded%20assemblies%20in%20the%20project.
/// </summary>

namespace Arcturus.InteractiveNovel
{
    public class SequenceParser : MonoBehaviour
    {
        private static SequenceParser I;

        // Stores the current scene's entire dialogue sequence.
        private Dictionary<int, DialogueSequence> dialogueSequences;

        // Vars used only during file parsing.
        public static Action OnParsingCompleted;
        private DialogueSequence currentSequence;
        private DialogueLine previousLine;

        #region Helper Funcs
        private string[] SplitBySemi(string line)
        {
            return line.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion

        private void Awake()
        {
            #region Static Referencing
            if (I == null) I = this;
            else if (I != this) Destroy(gameObject);
            #endregion

            dialogueSequences = new Dictionary<int, DialogueSequence>();
        }

        private void BeginParsing(string playwrightPath)
        {
            var tAsset = Resources.Load<TextAsset>(playwrightPath).ToString();
            var cleanedAsset = tAsset.Replace("; ", ";").Replace(": ", ":");
            var playwright = cleanedAsset.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            if (playwright.Length == 0)
                Debug.LogWarning("Playwright has no data within");

            foreach (var line in playwright)
            {
                switch (line[0])
                {
                    case '/': // Skips comments in text file.
                        continue;

                    case '[':
                        ParseIndexOrCommand(line);
                        continue;

                    case '^':
                        ParseChoiceLine(line);
                        continue;

                    default:
                        ParseAlternativeLine(line);
                        continue;
                }
            }

            currentSequence = null;
            previousLine = null;
            OnParsingCompleted?.Invoke();
        }

        #region File Parsing
        private void ParseIndexOrCommand(string line)
        {
            if (int.TryParse(line.Replace("[", "").Replace("]", ""), out var index))
            {
                currentSequence = new DialogueSequence();
                dialogueSequences.Add(index, currentSequence);
                return;
            }
            else
            {
                if (!line.Contains(";"))
                    currentSequence.SequenceEndActions.Add(line.Replace("[", "").Replace("]", ""));
                else
                {
                    foreach (var splitAction in SplitBySemi(line))
                        currentSequence.SequenceEndActions.Add(splitAction.Replace("[", "").Replace("]", ""));
                }
                return;
            }
        }

        private void ParseChoiceLine(string line)
        {
            var lineData = SplitBySemi(line);
            if (lineData.Length < 2 && lineData[1].Contains(":"))
                return;

            var choiceData = lineData[1].Split(':');
            var index = int.Parse(choiceData[1].Replace("[", "").Replace("]", ""));
            previousLine.Choices.Add(new DialogueChoice(choiceData[0], index));
        }

        private void ParseAlternativeLine(string line)
        {
            var lineData = SplitBySemi(line);
            switch (lineData.Length)
            {
                case 1: // Player choice preperation.
                    previousLine = new DialogueLine("Choice", "", "", "");
                    currentSequence.DialogueLines.Add(previousLine);
                    break;

                case 2: // Player speaking.
                    var contentPlayer = lineData[1].Contains("*") ? previousLine.Content : lineData[1];
                    previousLine = new DialogueLine("<playername>", contentPlayer, "", "");
                    currentSequence.DialogueLines.Add(previousLine);
                    break;

                case 4: // Character speaking.
                    var name = lineData[0].Contains("*") ? previousLine.Name : lineData[0];
                    var contentCharacter = lineData[1].Contains("*") ? previousLine.Content : lineData[1];
                    var pose = lineData[2].Contains("*") ? previousLine.Pose : lineData[2];
                    var position = lineData[3].Contains("*") ? previousLine.Position : lineData[3];

                    previousLine = new DialogueLine(name, contentCharacter, pose, position);
                    currentSequence.DialogueLines.Add(previousLine);
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region System Interfacing
        public static void ParseFile(string playwrightPath)
        {
            I.BeginParsing(playwrightPath);
        }

        public static bool HasSequence(int sequenceIndex)
        {
            return I.dialogueSequences.ContainsKey(sequenceIndex);
        }

        public static DialogueSequence GetSequence(int sequenceIndex)
        {
            if (!I.dialogueSequences.ContainsKey(sequenceIndex))
            {
                Debug.LogError($"Requesting sequence that does not exist: {sequenceIndex}");
                return null;
            }

            return I.dialogueSequences[sequenceIndex];
        }
        #endregion
    }
}
