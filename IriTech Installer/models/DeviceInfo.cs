using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;
using System.Xml.Serialization;

namespace IriTech_Installer.models
{
    [XmlRoot(ElementName = "additional_info")]
    public class Additional_info
    {
        [XmlElement(ElementName = "Param")]
        public Param Param { get; set; }
    }

    [XmlRoot(ElementName = "DeviceInfo")]
    public class DeviceInfo
    {
        [XmlElement(ElementName = "additional_info")]
        public Additional_info Additional_info { get; set; }
        [DeserializeAs(Name = "dc")]
        public string Dc { get; set; }
        [DeserializeAs(Name = "dpId")]
        public string DpId { get; set; }
        [DeserializeAs(Name = "mc")]
        public string Mc { get; set; }
        [DeserializeAs(Name = "mi")]
        public string Mi { get; set; }
        [DeserializeAs(Name = "rdsId")]
        public string RdsId { get; set; }
        [DeserializeAs(Name = "rdsVer")]
        public string RdsVer { get; set; }
    }

    [XmlRoot(ElementName = "Param")]
    public class Param
    {
        [DeserializeAs(Name = "name")]
        public string Name { get; set; }
        [DeserializeAs(Name = "value")]
        public string Value { get; set; }
    }
}
