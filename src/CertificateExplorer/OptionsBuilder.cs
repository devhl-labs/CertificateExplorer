using System.Security.Cryptography.X509Certificates;

namespace CertificateExplorer
{
    public class OptionsBuilder
    {
        public StoreName StoreName { get; set; }
        public StoreLocation StoreLocation { get; set; }
        public X509FindType X509FindType { get; set; }
        public OpenFlags OpenFlags { get; set; }
        public string FindValue { get; set; }

        public OptionsBuilder(StoreName storeName, StoreLocation storeLocation, X509FindType x509FindType, OpenFlags openFlags, string findValue)
        {
            StoreName = storeName;
            StoreLocation = storeLocation;
            X509FindType = x509FindType;
            OpenFlags = openFlags;
            FindValue = findValue;
        }

        public string[] Build()
        {
            string[] result = new string[10];

            result[0] = $"-{nameof(StoreName)}";
            result[1] = StoreName.ToString();

            result[2] = $"-{nameof(StoreLocation)}";
            result[3] = StoreLocation.ToString();

            result[4] = $"-{nameof(X509FindType)}";
            result[5] = X509FindType.ToString();

            result[6] = $"-{nameof(OpenFlags)}";
            result[7] = OpenFlags.ToString();

            result[8] = $"-{nameof(FindValue)}";
            result[9] = FindValue;

            return result;
        }
    }
}
