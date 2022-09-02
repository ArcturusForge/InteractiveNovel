using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Arcturus.InteractiveNovel
{
    public class CharacterManager : MonoBehaviour
    {
        private static CharacterManager I;
        private Dictionary<string, CharacterGen> characters;
        [SerializeField] private AudioClip defaultVoice;

        private void Awake()
        {
            #region Static Referencing
            if (I == null) I = this;
            else if (I != this) Destroy(gameObject);
            #endregion

            DataReader reader = new DataReader(null, ParseDatabase);
            reader.ReadResourcesDatabase("Databases/Characters", "Characters");
        }

        #region Database Reading
        private void ParseDatabase(XmlReader reader)
        {
            characters = new Dictionary<string, CharacterGen>();

            // Read here
            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "Character":

                        var charName = reader.GetAttribute("Name");
                        var voicePath = reader.GetAttribute("VoicePath");
                        var poses = new List<KeyValuePair<string, string>>();

                        if (reader.ReadToDescendant("Pose"))
                        {
                            do
                            {
                                var poseName = reader.GetAttribute("Name");
                                var posePath = reader.GetAttribute("Path");
                                poses.Add(new KeyValuePair<string, string>(poseName, posePath));
                            } while (reader.ReadToNextSibling("Pose"));
                        }

                        // TODO: Implement voices
                        characters.Add(charName, new CharacterGen(charName, poses, null));
                        break;
                }
            }
        }
        #endregion

        #region System Interfacing
        /// <summary>
        /// Returns the pose of a character stored in the database.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="pose"></param>
        /// <returns>Null if the character or pose is invalid.</returns>
        public static Sprite GetPose(string character, string pose)
        {
            if (!I.characters.ContainsKey(character) || !I.characters[character].Poses.ContainsKey(pose))
            {
                Debug.LogError($"Missing either Character: {character} OR Pose: {pose}!");
                return null;
            }

            var poseP = I.characters[character].Poses[pose];
            return Resources.Load<Sprite>(poseP);
        }

        /// <summary>
        /// Returns the voice of a character stored in the database.
        /// </summary>
        /// <param name="character"></param>
        /// <returns>Default voice if character has none set.</returns>
        public static AudioClip GetVoice(string character)
        {
            if (!I.characters.ContainsKey(character))
            {
                Debug.LogError($"Missing Character: {character}!");
                return null;
            }

            var voice = I.characters[character].Voice;
            return voice == null ? I.defaultVoice : voice;
        }
        #endregion
    }
}
