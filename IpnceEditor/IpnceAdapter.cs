using System.IO;

namespace IpnceEditor
{
    interface IpnceAdapter //Used for loading all kinds of Ipnce
    {
        Ipnce Load(BinaryReader br);

        void Save(Ipnce ip, BinaryWriter bw);

        Ipnce ToIpnce(object ob); //The program just transforms any ipnce to AAI2 ipnce

        object FromIpnce(Ipnce ip);
    }
}
