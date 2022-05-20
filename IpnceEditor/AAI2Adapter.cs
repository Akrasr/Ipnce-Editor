using System.IO;

namespace IpnceEditor
{
    class AAI2Adapter : IpnceAdapter
    {
        public Ipnce Load(BinaryReader br)
        {
            Ipnce ip = new Ipnce();
            ip.Load(br);
            return ip;
        }
        public void Save(Ipnce ip, BinaryWriter bw)
        {
            ip.Save(bw);
        }

        public Ipnce ToIpnce(object ob)
        {
            return (Ipnce)ob;
        }

        public object FromIpnce(Ipnce ip)
        {
            return ip;
        }
    }
}
