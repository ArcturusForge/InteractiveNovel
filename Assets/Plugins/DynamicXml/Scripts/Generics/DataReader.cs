using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using Arcturus.Xml.Internal;

public class DataReader
{
    public static DataReader _XmlDataInstanceRef;

    XmlData data; // PlayerPrefs specific param.

    public Action<XmlWriter> OnWrite { get; protected set; }
    public Action<XmlReader> OnRead { get; protected set; }

    #region Constructor
    public DataReader(Action<XmlWriter> OnWrite, Action<XmlReader> OnRead)
    {
        _XmlDataInstanceRef = this;
        this.OnWrite = OnWrite;
        this.OnRead = OnRead;

        // Only used for Read/Write to Prefs.
        data = new XmlData();
    }
    #endregion

    #region Shortcut Methods
    /// <summary>
    /// Shortcut method that writes file to player prefs.<br/>
    /// Used mainly for game saving.
    /// </summary>
    /// <param name="dataName"></param>
    public void WritePrefsDatabase(string dataName)
    {
        var serializer = new XmlSerializer(typeof(XmlData));
        TextWriter tw = new StringWriter();
        serializer.Serialize(tw, data);
        tw.Close();

        Debug.Log(tw.ToString());
        PlayerPrefs.SetString(dataName, tw.ToString());
    }

    /// <summary>
    /// Shortcut method that reads from player prefs.<br/>
    /// Used mainly for game saving.
    /// </summary>
    /// <param name="dataName"></param>
    /// <returns></returns>
    public void ReadPrefsDatabase(string dataName)
    {
        var serializer = new XmlSerializer(typeof(XmlData));
        TextReader tw = new StringReader(PlayerPrefs.GetString(dataName));
        data = (XmlData)serializer.Deserialize(tw);
        tw.Close();
    }
    #endregion

    #region Resources Method
    /// <summary>
    /// Handler method for locating and reading from an Xml database within the Resources directory.<br/>
    /// Mainly used for reading game data e.g. Item stats, Quest data, etc.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="OpeningElementName"></param>
    public void ReadResourcesDatabase(string path, string OpeningElementName)
    {
        var xmlText = Resources.Load<TextAsset>(path);
        XmlTextReader reader = new XmlTextReader(new StringReader(xmlText.text));
        reader.ReadToDescendant(OpeningElementName);
        OnRead?.Invoke(reader);
        reader.Close();
    }
    #endregion

    #region StreamingAssets Methods
    /// <summary>
    /// Handler method for creating and writing an xml document to a specified location.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="firstElement"></param>
    public void WriteStreamAssetsDatabase(string fileName, string firstElement)
    {
        var settings = new XmlWriterSettings();
        settings.Indent = true; // Whether to indent elements.
        settings.IndentChars = ("    "); // Element indentation string.
        settings.CloseOutput = true; // Whether to do writer.Close() automatically.
        settings.OmitXmlDeclaration = false; // Whether to write <?xml version="1.0" encoding="utf-8"?> or not.

        using (var writer = XmlWriter.Create(Path.Combine(Application.streamingAssetsPath, fileName), settings))
        {
            writer.WriteStartElement(firstElement);
            OnWrite?.Invoke(writer);
            writer.WriteEndElement();
            writer.Flush();
        }
    }

    /// <summary>
    /// Handler method for locating and reading from an Xml database within the StreamingAssets directory.<br/>
    /// Mainly used for reading game data e.g. Item stats, Quest data, etc.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="OpeningElementName"></param>
    public void ReadStreamAssetsDatabase(string path, string OpeningElementName)
    {
        var filePath = Path.Combine(Application.streamingAssetsPath, path);
        var xmlText = File.ReadAllText(filePath);
        XmlTextReader reader = new XmlTextReader(new StringReader(xmlText));
        reader.ReadToDescendant(OpeningElementName);
        OnRead?.Invoke(reader);
        reader.Close();
    }
    #endregion
}
