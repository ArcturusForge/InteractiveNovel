using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace Arcturus.InteractiveNovel
{
    public class BackgroundManager : MonoBehaviour
    {
        private static BackgroundManager I;

        [SerializeField] private RectTransform bgParent;

        private Dictionary<string, Sprite> backgrounds;
        private Image bgBox;

        private void Awake()
        {
            #region Static Referencing
            if (I == null) I = this;
            else if (I != this) Destroy(gameObject);
            #endregion

            DataReader reader = new DataReader(null, ParseDatabase);
            reader.ReadResourcesDatabase("Databases/Backgrounds", "Backgrounds");

            if (bgParent != null)
                bgBox = Instantiate(Resources.Load<GameObject>("Prefabs/BackgroundBox"), bgParent).GetComponent<Image>();
        }

        #region Database Reading
        private void ParseDatabase(XmlReader reader)
        {
            backgrounds = new Dictionary<string, Sprite>();

            // Read here
            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "Background":

                        var bgName = reader.GetAttribute("Name");
                        var imagePath = reader.GetAttribute("Path");

                        backgrounds.Add(bgName, Resources.Load<Sprite>(imagePath));
                        break;
                }
            }
        }
        #endregion

        #region System Interfacing
        public static void SetBackground(string bgName)
        {
            if (!I.backgrounds.ContainsKey(bgName))
                return;

            I.bgBox.overrideSprite = I.backgrounds[bgName];
        }
        #endregion
    }
}
