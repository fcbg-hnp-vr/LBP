using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System;
using UnityEngine.EventSystems;
using SFB;
using TMPro;
using UnityEngine.UI;

public class CreateConfiguration : MonoBehaviour
{
    public GameObject playButton;
    public GameObject errorFileExist;

    XML xmlFile;
    string path;
   
    public void SaveConfiguration()
    {
        TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);
        var date = TimeZoneInfo.ConvertTime(System.DateTime.Now, zone);

        string url = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "xml"); //StandaloneFileBrowser.OpenFolderPanel("LowBackpain Configuration", "", false);

        if (url.Length > 0 && !string.IsNullOrEmpty(url))
        {
            path = @url;

            CreateFile();

            XmlSerializer serializer = new XmlSerializer(typeof(XML));

            FileStream stream;
            stream = File.Create(path);
            serializer.Serialize(stream, xmlFile);

            stream.Flush();
            stream.Close();

            playButton.SetActive(true);

        }
    }

    public void OverwriteFile()
    {
        File.Delete(path);

        CreateFile();

        XmlSerializer serializer = new XmlSerializer(typeof(XML));

        FileStream stream;
        stream = File.Create(path);
        serializer.Serialize(stream, xmlFile);

        stream.Flush();
        stream.Close();

        playButton.SetActive(true);

    }

    public void CreateFile()
    {
        if (ConfigurationManager.instance != null)
        {
            xmlFile = new XML();

            //Add new child element
            XElement blocksElement = new XElement("Blocks");
            xmlFile.configuration.Add(blocksElement);
            xmlFile.configuration.Element("Blocks").SetAttributeValue("nb", ConfigurationManager.instance.nbBlock);

            for (int i = 0; i < ConfigurationManager.instance.nbBlock; i++)
            {
                XElement blockElement = new XElement("Block"+i);
                xmlFile.configuration.Element("Blocks").Add(blockElement);

                //List<Settings.RangeOfMotion> tempListRangeOfMotion = new List<Settings.RangeOfMotion>();

                for (int j = 0; j < ConfigurationManager.instance.allInputFiledRangeOfMotion[i].Count; j++)
                {

                    XElement salingFactorElement = new XElement("ScalingFactor" + j);
                    xmlFile.configuration.Element("Blocks").Element("Block" + i).Add(salingFactorElement);

                    xmlFile.configuration.Element("Blocks").Element("Block" + i).Element("ScalingFactor" + j).SetAttributeValue("Scaling", ConfigurationManager.instance.allInputFiledRangeOfMotion[i][j].GetComponent<TMP_InputField>().text);

                    Dictionary<int, int> tempDictionary = new Dictionary<int, int>();
                    tempDictionary = ConfigurationManager.instance.nbTrialPerBlockPerRangeOfMotion[i];
                    int nbTrial = tempDictionary[j];
                    xmlFile.configuration.Element("Blocks").Element("Block" + i).Element("ScalingFactor" + j).SetAttributeValue("NbTrials", nbTrial.ToString());

                    //Create block settings
                    //Settings.RangeOfMotion tempRangeOfMotion = new Settings.RangeOfMotion();
                   // tempRangeOfMotion.factor = (float.Parse(ConfigurationManager.instance.allInputFiledRangeOfMotion[i][j].GetComponent<TMP_InputField>().text));
                    //tempRangeOfMotion.nbtrial = nbTrial;

                    //tempListRangeOfMotion.Add(tempRangeOfMotion);

                }

                //Settings.Block b = new Settings.Block
                {
                    //rangeOfMotion = new List<Settings.RangeOfMotion>()
                };
                //b.rangeOfMotion = tempListRangeOfMotion;

                //Settings.instance.myBlocks.Add(b);
            }
        }
        else
        {
            Debug.LogError("No instance of ConfigurationManager !");
            return;
        }
       

    }
}
