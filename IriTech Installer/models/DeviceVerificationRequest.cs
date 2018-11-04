using System;

namespace IriTech_Installer.models
{
    public class DeviceVerificationRequest
    {
        public String Token { get; set; }
        public String BrandCode { get; set; }
        public String SerialNumber { get; set; }
    }
}
