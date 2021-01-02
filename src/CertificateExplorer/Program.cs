using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CertificateExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            OptionsBuilder? builder = null;

            builder = new(StoreName.My, StoreLocation.CurrentUser, X509FindType.FindBySubjectDistinguishedName, OpenFlags.ReadOnly, "<extended subject name>");
            var a = FindCerts(builder.Build());

            builder = new(StoreName.My, StoreLocation.CurrentUser, X509FindType.FindBySubjectName, OpenFlags.ReadOnly, "<extended subject name>");
            var b = FindCerts(builder.Build());

            builder = new(StoreName.My, StoreLocation.CurrentUser, X509FindType.FindBySubjectDistinguishedName, OpenFlags.ReadOnly, "CN=<subject name>");
            var c = FindCerts(builder.Build());

            builder = new(StoreName.My, StoreLocation.CurrentUser, X509FindType.FindBySubjectName, OpenFlags.ReadOnly, "CN=<subject name>");
            var d = FindCerts(builder.Build());

            builder = new(StoreName.My, StoreLocation.CurrentUser, X509FindType.FindBySubjectDistinguishedName, OpenFlags.ReadOnly, "<subject name>");
            var e = FindCerts(builder.Build());

            builder = new(StoreName.My, StoreLocation.CurrentUser, X509FindType.FindBySubjectName, OpenFlags.ReadOnly, "<subject name>");
            var f = FindCerts(builder.Build());

#else
            

            if (args.Any(a => a.ToLower() == "-help" || a.ToLower() == "help"))
                Help();
            else
                FindCerts(args);


#endif


        }

        static void Help()
        {
            Console.WriteLine(
@"Required parameters: storeName, storeLocation, x509FindType, openFlags
Optional Parameters: findValue, privatekey
Enumerations are from the System.Security.Cryptography.X509Certificates namespace.");
        }

        static X509Certificate2Collection FindCerts(string[] args)
        {
            StoreName storeName = ParseEnumArgument<StoreName>(args, nameof(storeName));
            StoreLocation storeLocation = ParseEnumArgument<StoreLocation>(args, nameof(storeLocation));
            X509FindType x509FindType = ParseEnumArgument<X509FindType>(args, nameof(x509FindType));
            OpenFlags openFlags = ParseEnumArgument<OpenFlags>(args, nameof(openFlags));
            string findValue = ParseStringArgument(args, nameof(findValue)) ?? string.Empty;

            using var store = new X509Store(storeName, storeLocation);

            store.Open(openFlags);

            X509Certificate2Collection collection = store.Certificates.Find(x509FindType, findValue, false);

            foreach (X509Certificate2 cert in collection)
            {
                StringBuilder stringBuilder = new();

                const int PAD = -17;

                stringBuilder.AppendLine($"{"Friendly Name:",PAD}{ cert.FriendlyName }");
                stringBuilder.AppendLine($"{"Has Private Key:",PAD}{ cert.HasPrivateKey}");
                stringBuilder.AppendLine($"{"Issuer:",PAD}{ cert.Issuer }");
                stringBuilder.AppendLine($"{"Issuer Name:",PAD}{ cert.IssuerName.Oid?.FriendlyName }");
                stringBuilder.AppendLine($"{"Not After:",PAD}{ cert.NotAfter }");
                stringBuilder.AppendLine($"{"Not Before:",PAD}{ cert.NotBefore} ");

                if (args.Any(a => a?.ToLower() == "-privatekey"))
                    stringBuilder.AppendLine($"{"Private Key:",PAD}{ cert.PrivateKey }");

                stringBuilder.AppendLine($"{"Public Key:",PAD}{ cert.PublicKey.Oid.Value }");
                stringBuilder.AppendLine($"{"Serial Number:",PAD}{ cert.SerialNumber }");
                stringBuilder.AppendLine($"{"Subject:",PAD}{ cert.Subject }");
                stringBuilder.AppendLine($"{"Subject Name:",PAD}{ cert.SubjectName.Name }");
                stringBuilder.AppendLine($"{"Thumbprint:",PAD}{ cert.Thumbprint }");
                stringBuilder.AppendLine($"{"Version:",PAD}{ cert.Version }");

                Console.WriteLine(stringBuilder);
            }

            Console.WriteLine($"Length:{collection.Count}");

            return collection;
        }

        static TEnum ParseEnumArgument<TEnum>(string[]args, string paramName) where TEnum : struct, Enum
        {         
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i]?.ToLower() == $"-{paramName.ToLower()}")
                {
                    string input = args[i + 1];

                    if (int.TryParse(input, out int storeNameParsed))
                        return (TEnum)Enum.ToObject(typeof(TEnum), storeNameParsed);
                    else
                        return (TEnum)Enum.Parse(typeof(TEnum), input, true);
                }
            }

            throw new ArgumentException($"Parameter must be provided", paramName);
        }

        static string? ParseStringArgument(string[] args, string paramName)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i]?.ToLower() == $"-{paramName.ToLower()}")                
                    return args[i + 1];                
            }

            return null;
        }
    }
}
