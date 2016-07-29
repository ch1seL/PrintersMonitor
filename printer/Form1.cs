using SnmpSharpNet;
using System;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace printer
{
    public partial class Form1 : Form
    {
        public static printersDataSet.CurrentErorrsDataTable CEdt = new printersDataSet.CurrentErorrsDataTable();
        public static string[] ErrorMessageText = new string[128];
        private printersDataSetTableAdapters.ErorrsLogTableAdapter Aadapter = new printersDataSetTableAdapters.ErorrsLogTableAdapter();
        private printersDataSet.ErorrsLogDataTable Adt = new printersDataSet.ErorrsLogDataTable();
        private printersDataSetTableAdapters.CurrentErorrsTableAdapter CEadapter = new printersDataSetTableAdapters.CurrentErorrsTableAdapter();
        private IPAddress ip;
        private printersDataSetTableAdapters.PrintersTableAdapter IPadapter = new printersDataSetTableAdapters.PrintersTableAdapter();
        private printersDataSet.PrintersDataTable IPdt = new printersDataSet.PrintersDataTable();
        private int IPid, Aid;
        public Form1()
        {
            InitializeComponent();
            foreach (var item in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (!(item.IsIPv6LinkLocal || item.IsIPv6Multicast || item.IsIPv6SiteLocal || item.IsIPv6Teredo) && item.ToString().Length >= 7)
                {
                    ip = item;
                    break;
                }
            }
            label1.Text = ip.ToString() + "\n";
            label2.Text = "Device SNMP information:\n";
            IPdt = IPadapter.GetData();
            IPid = IPdt.Count;
            Adt = Aadapter.GetData();
            Aid = Adt.Count;
            CEdt = CEadapter.GetData();
            button1_Click(new object(), new EventArgs());
            new Server(1994);
        }

        public enum model
        {
            Kyocera = 0,
            HP = 1,
            Samsung = 2
        }

        private void B3_DoWork(object sender)
        {
            string s = combine((int)sender);
            if (s.Trim() != "")
            {
                Invoke((MethodInvoker)delegate
                {
                    richTextBox1.Text += s;
                });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            Thread Thread = new Thread(new ParameterizedThreadStart(doit));
            Thread.IsBackground = true;
            Thread.Start(Thread);
        }

        private string combine(int id)
        {
            //try
            //{

            var ip = IPdt.FindByid(id).ip.Trim();

            printer p = new printer() { ip = ip, id = id };
            string mod = SNMPget(ip, new string[] { "1.3.6.1.2.1.1.1.0" })[0];
            if (mod.Contains("Samsung"))
                p.Model = model.Samsung;
            else if (mod.Contains("KYOCERA"))
                p.Model = model.Kyocera;
            else if (mod.Contains("HP"))
                p.Model = model.HP;
            else
                return "";
            if (p.Model == model.Kyocera)
            {
                string[] result = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.2.1.3.1", "1.3.6.1.4.1.1347.43.10.1.1.12.1.1", "1.3.6.1.2.1.25.3.5.1.2.1" });
                p.name = "Kyocera " + result[0];
                p.count = result[1];
                p.error = ErrorMessageText[(int)result[2][0]];
            }
            else if (p.Model == model.HP)
            {
                string[] result = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.2.1.3.1",
                                                                 "1.3.6.1.4.1.11.2.3.9.4.2.1.4.1.2.5.0",
                                                                 "1.3.6.1.2.1.25.3.5.1.2.1"});
                p.name = result[0];
                p.count = result[1];
                p.error = ErrorMessageText[Convert.ToInt32(result[2])];
            }
            else if (p.Model == model.Samsung)
            {
                string[] result = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.2.1.3.1",
                                                                 "1.3.6.1.2.1.25.3.5.1.2.1"});
                p.name = result[0];
                p.count = "\"Can't extract the number of sheets\"";
                p.error = ErrorMessageText[Convert.ToInt32(result[1])];
            }
            if (p.name != null && p.count != null && p.name != "" && p.count != "")
            {
                bool good = false;
                foreach (printersDataSet.PrintersRow item in IPdt.Rows)
                {
                    if (item.ip.Trim() == p.ip)
                    {
                        good = true;
                        break;
                    }
                }

                /* УБрали автоматическое добавление принтеров в базу
                 * 
                 * if (!good)
                {
                    IPdt.AddPrintersRow(IPid, p.ip, p.name);
                    IPadapter.Insert(IPid++, p.ip, p.name);
                }*/


                if (p.error != "no error" && p.error!=null)
                {
                    good = true;
                    foreach (printersDataSet.CurrentErorrsRow item in CEdt.Rows)
                    {
                        if (item.printer_id == p.id)
                        {
                            good = false;
                            break;
                        }
                    }
                    if (good)
                    {
                        Aadapter.Insert(p.id, DateTime.Now, Convert.ToInt32(p.count), p.error ?? "");
                        CEadapter.Insert(p.id, p.error ?? "");
                        CEdt.AddCurrentErorrsRow(p.id, p.error ?? "");
                    }
                }
                else
                {
                    good = true;
                    foreach (printersDataSet.CurrentErorrsRow item in CEdt.Rows)
                    {
                        if (item.printer_id == p.id)
                        {
                            good = false;

                            string result = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.5.1.2.1" })[0];

                            if (item.Error == "no tonner" && (p.error = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.5.1.2.1" })[0]) == "0")
                            {
                                Thread.Sleep(10000);
                                if (item.Error == "no tonner" && (p.error = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.5.1.2.1" })[0]) == "0")
                                {
                                    bool i_am_here = true;
                                }
                            }
                            break;
                        }
                    }
                    if (!good)
                    {
                        CEadapter.Delete(p.id);
                        CEdt = CEadapter.GetData();
                        Aadapter.Insert(p.id, DateTime.Now, Convert.ToInt32(p.count), ErrorMessageText[Convert.ToInt32(p.error)]);
                    }
                }
                return p.ip + " " + p.name + " " + p.count + " " + p.error + "\n";
            }
            //}
            //catch (Exception e)
            //{
            //    throw e;

            //}
            return "";
        }

        private void doit(object sender)
        {
            int uptime = 0;
            ErrorMessageText[0] = "no error";
            ErrorMessageText[6] = "paper is jammed in the orinter";
            ErrorMessageText[10] = "крышка открыта";
            ErrorMessageText[12] = "no tonner";///
            ErrorMessageText[18] = "no tonner";
            ErrorMessageText[26] = "no tonner";
            ErrorMessageText[32] = "low tonner";
            ErrorMessageText[50] = "no tonner";
            ErrorMessageText[66] = "нет бумаги";
            //string[][] ipse = GetIP();

            //string ips = ip.ToString().Remove(ip.ToString().IndexOf('.', 3));

            printersDataSet.CurrentErorrsDataTable CEdt1 = CEdt;
            foreach (printersDataSet.CurrentErorrsRow p in CEdt1.Rows)
            {
                combine(p.printer_id);
            }

            printersDataSet.PrintersDataTable IPdt1 = IPdt;
            foreach (printersDataSet.PrintersRow p in IPdt1.Rows)
            {
                Thread Thre = new Thread(new ParameterizedThreadStart(B3_DoWork));
                Thre.IsBackground = true;
                Thre.Start(p.id);
            }
            /*for (int i = Convert.ToByte(ipse[0][2]); i <= Convert.ToByte(ipse[1][2]); i++)
            {
                for (int j = Convert.ToByte(ipse[0][3]); j <= Convert.ToByte(ipse[1][3]); j++)
                {
                    bool good = true;
                    foreach (printersDataSet.PrintersRow p in IPdt.Rows)
                    {
                        if (p.ip.Trim() == ips + "." + i.ToString() + "." + j.ToString())
                        {
                            good = false;
                            break;
                        }
                    }
                    if (!good)
                    {
                        continue;
                    }
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 16);
                    try
                    {
                        s.Connect(ips + "." + i.ToString() + "." + j.ToString(), 191);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    if (s.Connected)
                    {
                        Thread Thre = new Thread(new ParameterizedThreadStart(B3_DoWork));
                        Thre.IsBackground = true;
                        Thre.Priority = ThreadPriority.Lowest;
                        Thre.Start(ips + "." + i.ToString() + "." + j.ToString());
                    }
                }
            }
            ///1.3.6.1.4.1.1347.43.10.1.1.12.1.1
            ///1.3.6.1.4.1.1347.43.5.1.1.1.1
            ///1.3.6.1.2.1.1.1.0
            ///1.3.6.1.2.1.25.3.2.1.3
            ///1.3.6.1.2.1.1.2.0*/
        }

        private string[][] GetIP()
        {
            string entry = Dns.GetHostEntry(ip).HostName;
            string ips = ip.ToString().Remove(ip.ToString().IndexOf('.', 3));

            byte[] mask = null;
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();
            foreach (ManagementObject objMO in objMOC)
            {
                if (!(bool)objMO["ipEnabled"]) continue;
                string[] subnets = (string[])objMO["IPSubnet"];
                mask = Dns.GetHostAddresses(subnets[0])[0].GetAddressBytes();
            }
            byte[] ipBytes = ip.GetAddressBytes();
            byte[] maskBytes = mask.ToArray();
            byte[] startIPBytes = new byte[ipBytes.Length];
            byte[] endIPBytes = new byte[ipBytes.Length];
            for (int i = 0; i < ipBytes.Length; i++)
            {
                startIPBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                endIPBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
            }
            IPAddress startIP = new IPAddress(startIPBytes);
            IPAddress endIP = new IPAddress(endIPBytes);
            string[] bstart = startIP.ToString().Split('.');
            string[] bend = endIP.ToString().Split('.');
            return new string[2][] { bstart, bend };
        }

        private string[] SNMPget(string ip, string[] oid)
        {
            string[] output = new string[oid.Length];
            IpAddress agent = new IpAddress(ip);
            Pdu pdu = new Pdu(PduType.Get);
            foreach (var item in oid)
            {

                pdu.VbList.Add(item);
            }
            UdpTarget target = new UdpTarget((IPAddress)agent, 161, 512, 1);
            try
            {
                SnmpV2Packet result2 = (SnmpV2Packet)target.Request(pdu, new AgentParameters(SnmpVersion.Ver2, new OctetString("public")));
                for (int i = 0; i < result2.Pdu.VbList.Count; i++)
                {
                    output[i] = result2.Pdu.VbList[i].Value.ToString();
                }
            }
            catch (Exception)
            {
                try
                {
                    SnmpV1Packet result1 = (SnmpV1Packet)target.Request(pdu, new AgentParameters(SnmpVersion.Ver1, new OctetString("public")));
                    for (int i = 0; i < result1.Pdu.VbList.Count; i++)
                    {
                        output[i] = result1.Pdu.VbList[i].Value.ToString();
                    }
                }
                catch (Exception)
                {
                }
                
            }
            for (int i = 0; i < oid.Length; i++)
            {
                if (oid[i] == "1.3.6.1.2.1.25.3.5.1.2.1")
                {
                    if (output[i] == "\0" || output[i] == "Null" || output[i] == "" || output[i] == "00 00")
                    {
                        output[i] = "0";
                    }

                }

            }
            return output;
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            printersDataSet.PrintersDataTable IPdt1 = IPdt;
            foreach (printersDataSet.PrintersRow p in IPdt1.Rows)
            {
                Thread Thre = new Thread(new ParameterizedThreadStart(B3_DoWork));
                Thre.IsBackground = true;
                Thre.Priority = ThreadPriority.Lowest;
                Thre.Start(p.id);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            printersDataSet.CurrentErorrsDataTable CEdt1 = CEdt;
            foreach (printersDataSet.CurrentErorrsRow p in CEdt1.Rows)
            {
                combine(p.printer_id);
            }
        }

        public struct printer
        {
            public int id { get; set; }
            public string count { get; set; }
            public string error { get; set; }
            public string ip { get; set; }
            public model Model { get; set; }
            public string name { get; set; }
        }
    }
}