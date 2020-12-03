using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

[System.Serializable]
[XmlRoot("LowBackPain")]
public class XML
{
    [XmlAnyElement("Configuration")]
    public XElement configuration = new XElement("Configuration",
        new XElement("Map", new XAttribute("Path", ""), new XAttribute("SizeX", ""), new XAttribute("SizeY", ""),new XAttribute("SizeZ", ""), new XAttribute("PosX", ""), new XAttribute("PosY", ""), new XAttribute("PosZ", ""),
            new XAttribute("RotX", ""), new XAttribute("RotY", ""), new XAttribute("RotZ", "")));

}
