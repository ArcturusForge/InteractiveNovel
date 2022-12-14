using System.Collections.Generic;
using UnityEngine;

namespace Arcturus.InteractiveNovel
{
    /// <summary>
    /// Holds the data of a single character generated by the CharacterManager script while parsing the Character xml file.
    /// </summary>
    [System.Serializable]
    public class CharacterGen
    {
        public string Name { get { return name; } }
        private readonly string name;
        public Dictionary<string, string> Poses { get { return poses; } }
        private readonly Dictionary<string, string> poses;
        public AudioClip Voice { get { return voice; } }
        private readonly AudioClip voice;


        public CharacterGen(string name, List<KeyValuePair<string, string>> poses, AudioClip voice)
        {
            this.name = name;
            this.voice = voice;

            this.poses = new Dictionary<string, string>();
            foreach (var pose in poses)
            {
                if (!this.poses.ContainsKey(pose.Key))
                    this.poses.Add(pose.Key, pose.Value);
                else
                    Debug.LogWarning($"{name} has already got a pose named {pose.Key}. One is being ignored!");
            }
        }
    }
}
