using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Arcturus.VisualNovel
{
    public class CharacterManager : MonoBehaviour
    {
        private Dictionary<string, CharacterGen> characters;

        private void Awake()
        {
            DataReader reader = new DataReader(null, ParseDatabase);
            reader.ReadResourcesDatabase("Databases/Characters", "Characters");
        }

        #region Database Reading
        private void ParseDatabase(XmlReader reader)
        {
            characters = new Dictionary<string, CharacterGen>();

            // Read here
        }
        #endregion

        #region System Interfacing

        #endregion
    }
}
