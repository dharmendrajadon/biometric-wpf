using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;
using System.Xml.Serialization;

namespace IriTech_Installer.models
{

    [XmlRoot(ElementName = "PidData")]
    public class PidData
    {
        [XmlElement(ElementName = "Data")]
        public String Data { get; set; }
        [XmlElement(ElementName = "Hmac")]
        public string Hmac { get; set; }
        [XmlElement(ElementName = "Resp")]
        public Resp Resp { get; set; }
    }

    [XmlRoot(ElementName = "Resp")]
    public class Resp
    {
        [XmlAttribute(AttributeName = "errCode")]
        public string ErrCode { get; set; }
        [XmlAttribute(AttributeName = "errInfo")]
        public string ErrInfo { get; set; }
        [XmlAttribute(AttributeName = "fCount")]
        public string FCount { get; set; }
        [XmlAttribute(AttributeName = "fType")]
        public string FType { get; set; }
        [XmlAttribute(AttributeName = "iCount")]
        public string ICount { get; set; }
        [XmlAttribute(AttributeName = "iType")]
        public string IType { get; set; }
        [XmlAttribute(AttributeName = "nmPoints")]
        public string NmPoints { get; set; }
        [XmlAttribute(AttributeName = "pCount")]
        public string PCount { get; set; }
        [XmlAttribute(AttributeName = "pType")]
        public string PType { get; set; }
        [XmlAttribute(AttributeName = "qScore")]
        public string QScore { get; set; }
    }
}
