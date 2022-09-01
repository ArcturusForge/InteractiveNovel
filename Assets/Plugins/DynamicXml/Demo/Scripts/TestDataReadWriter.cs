using UnityEngine.SceneManagement;
using UnityEngine;
using System.Xml;

public class TestDataReadWriter : MonoBehaviour
{
    public enum LoadTest { PlayerPrefs, Resources, StreamingAssets }

    public LoadTest loadTest;

    DataReader dataReader;
    static bool load;

    private void Start()
    {
        dataReader = new DataReader(Write, Read);

        if (load)
        {
            switch (loadTest)
            {
                case LoadTest.PlayerPrefs:
                    dataReader.ReadPrefsDatabase("TestSave"); // The name of the saved prefs param.
                    break;
                case LoadTest.Resources:
                    dataReader.ReadResourcesDatabase("ResourceData", "Database"); // Do not include file type extension.
                    break;
                case LoadTest.StreamingAssets:
                    dataReader.ReadStreamAssetsDatabase("StreamedData.xml", "TestData"); // Must include file type extension.
                    break;
            }
            load = false;
        }
    }

    private void Update()
    {
        // Saves to the player prefs an example database.
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Saving file!");
            switch (loadTest)
            {
                case LoadTest.PlayerPrefs:
                    dataReader.WritePrefsDatabase("TestSave");
                    break;

                case LoadTest.StreamingAssets:
                    dataReader.WriteStreamAssetsDatabase("StreamedData.xml", "TestData");
                    break;
            }
        }

        // Reloads the scene and tells the script to run the loading functions.
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reloading world!");
            load = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// This method is a callback that reads from an example database for testing.<br/>
    /// This is where you would re-apply the data to your scripts.
    /// </summary>
    /// <param name="reader"></param>
    void Read(XmlReader reader)
    {
        var val = reader.GetAttribute("Greeting");
        if (!string.IsNullOrEmpty(val))
        {
            Debug.Log($"The current element: {reader.LocalName}");
            Debug.Log($"The current attribute: \"Greeting\" , The value: {val}");
        }
        else
        {
            Debug.LogWarning($"It is highly likely that when calling the load function that you passed through the wrong OpeningElementName");
            // This is not a major problem, Just pass through the first element in your xml document's name.
            /* Your doc looks something like this example:
             * 
             * <?xml version="1.0" encoding="utf-8"?>
             * <TestElement height="10" width="11">
             *     <NestedElement attribute="foo" />
             * </TestElement>
             * 
             * You need to pass through TestElement as the OpeningElementName in the ReadStreamAssetsDatabase() function.
             * The same goes for the ReadResourcesDatabase() variant.
             * That is because it is the first element in the document.
             */
        }
    }

    /// <summary>
    /// This method is a callback that writes an example database for testing.<br/>
    /// This is where you would get your managers to write all the data they want to store.
    /// </summary>
    /// <param name="writer"></param>
    void Write(XmlWriter writer)
    {
        writer.WriteAttributeString("Greeting", "Hello World!");
    }
}
