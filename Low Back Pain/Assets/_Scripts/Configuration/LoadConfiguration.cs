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


public class LoadConfiguration : MonoBehaviour
{
    public GameObject playButton;
    public GameObject succesInfo;
    public GameObject errorInfo;

    string path;

    ExtensionFilter[] extensions;
    char[] separator;

    XML fileConfiguration;

    // Start is called before the first frame update
    void Start()
    {
        extensions = new[] { new ExtensionFilter("Files", "xml") };

    }

    //Open file to load an configuration of the scene
    public void OpenFile()
    {
        var url = StandaloneFileBrowser.OpenFilePanel("File", "", extensions, false);
        if (url.Length > 0 && !string.IsNullOrEmpty(url[0]))
        {
            path = url[0];

            LoadFile();
        }
    }

    public void LoadFile()
    {
        string url = Uri.UnescapeDataString(path);

        if (url != "")
        {
            try
            {
                //open new xml file
                XmlSerializer serializer = new XmlSerializer(typeof(XML));
                FileStream stream = new FileStream(url, FileMode.Open);
                fileConfiguration = serializer.Deserialize(stream) as XML;
                stream.Flush();
                stream.Close();

                CreateConfiguration();

                succesInfo.SetActive(true);
                errorInfo.SetActive(false);
                playButton.SetActive(true);
                /* LoadClientConfiguration();
                 LoadMapping();

                 load.interactable = true;
                 save.interactable = true;

                 FileBrowser.instance.isImportProfil = true;*/
            }
            catch (Exception e)
            {
                Debug.LogError(e);

                errorInfo.SetActive(true);
                succesInfo.SetActive(false);
                playButton.SetActive(false);
                /*FileBrowser.instance.error.gameObject.SetActive(true);
                load.interactable = false;
                save.interactable = false;*/
            }
        }
    }

    public void CreateConfiguration()
    {
        int nbBlocks = 0;

        try
        {
            nbBlocks = int.Parse(fileConfiguration.configuration.Element("Blocks").Attribute("nb").Value);
        }
        catch
        {
            nbBlocks = 0;
        }

        for (int i = 0; i < nbBlocks; i++)
        {
            IEnumerable<XElement> nbScalingfactor = fileConfiguration.configuration.Element("Blocks").Element("Block" + i).Descendants();

            int nb = 0;
            foreach (var item in nbScalingfactor)
            {
               
                nb++;
            }

            
            //List<Settings.RangeOfMotion> tempListRangeOfMotion = new List<Settings.RangeOfMotion>();
            
            for (int j = 0; j < nb; j++)
            {

                XAttribute attributesScalingFactor = fileConfiguration.configuration.Element("Blocks").Element("Block" + i).Element("ScalingFactor" + j).Attribute("Scaling");
                XAttribute attributesTrialFactor = fileConfiguration.configuration.Element("Blocks").Element("Block" + i).Element("ScalingFactor" + j).Attribute("NbTrials");

                //Settings.RangeOfMotion tempRangeOfMotion = new Settings.RangeOfMotion();

               // tempRangeOfMotion.factor = (float.Parse(attributesScalingFactor.Value));
               // tempRangeOfMotion.nbtrial = (int.Parse(attributesTrialFactor.Value));

                //tempListRangeOfMotion.Add(tempRangeOfMotion);

            }


            //Settings.Block b = new Settings.Block
            {
                //rangeOfMotion = new List<Settings.RangeOfMotion>()
            };
           // b.rangeOfMotion = tempListRangeOfMotion;

           // Settings.instance.myBlocks.Add(b);

        }
    }
  
}
