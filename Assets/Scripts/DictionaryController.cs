using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DictionaryController
{
    private string xmlFileName; // Имя файла без расширения
    private Dictionary<string, string> nodeDictionary;

    //Парсим XML в словарь
    private void LoadDictionary()
    {
        nodeDictionary = new Dictionary<string, string>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Resources.Load<TextAsset>(xmlFileName).text);

        foreach (XmlNode node in xmlDoc.SelectNodes("/dictionary/Node"))
        {
            string id = node.Attributes["id"].Value;
            string text = node.Attributes["Text"].Value;
            nodeDictionary[id] = text;
        }
    }

    public string GetTextById(string id)
    {
        return nodeDictionary.TryGetValue(id, out string text) ? text : null;
    }

    public Dictionary<string, string> GetDictionary(string XmlFileName)
    {
        xmlFileName = XmlFileName;

        LoadDictionary();

        return nodeDictionary;
    }
}
