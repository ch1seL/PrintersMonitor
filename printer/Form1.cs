using SnmpSharpNet;
using System;
using System.Linq;
using System.Management;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace printer
{
    public partial class Form1 : Form
    {
        public static printersDataSet.CurrentErorrsDataTable CEdt = new printersDataSet.CurrentErorrsDataTable();
        public static string[] ErrorMessageText = new string[128];
        public static printersDataSetTableAdapters.ErorrsLogTableAdapter ELadapter = new printersDataSetTableAdapters.ErorrsLogTableAdapter();
        public printersDataSet.ErorrsLogDataTable ELdt = new printersDataSet.ErorrsLogDataTable();
        public static printersDataSetTableAdapters.CurrentErorrsTableAdapter CEadapter = new printersDataSetTableAdapters.CurrentErorrsTableAdapter();
        private IPAddress ip;
        private printersDataSetTableAdapters.PrintersTableAdapter Padapter = new printersDataSetTableAdapters.PrintersTableAdapter();
        private printersDataSet.PrintersDataTable Pdt = new printersDataSet.PrintersDataTable();
        private int Pid, ELid;
        private msngcentr msngcentr = new msngcentr();

        public Form1()
        {
            InitializeComponent();
            var builder = new System.Text.StringBuilder();
            foreach (var item in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (!(item.IsIPv6LinkLocal || item.IsIPv6Multicast || item.IsIPv6SiteLocal || item.IsIPv6Teredo) && item.ToString().Length >= 7)
                {
                    if (label1.Text == "")
                        label1.Text = item.ToString();
                    else
                        label1.Text += ", " + item.ToString();
                    ip = item;
                    //break;
                }
            }

            label2.Text = "Device SNMP information:";
            Pdt = Padapter.GetData();
            Pid = Pdt.Count;
            ELdt = ELadapter.GetData();
            ELid = ELdt.Count;
            CEdt = CEadapter.GetData();
            button1_Click(new object(), new EventArgs());
            new Server(1994);
            msngcentr.Parent = this;
            msngcentr.Show();
            msngcentr.BackColor = System.Drawing.Color.Coral;
            msngcentr.Location = new System.Drawing.Point(871, 30);
            msngcentr.Size = new System.Drawing.Size(260, 353);
        }

        public enum model
        {
            Kyocera = 0,
            HP = 1,
            Samsung = 2
        }

        private void B3_DoWork(object sender)
        {
            /*var s = */
            combine((int)sender);
            /*if (s.Trim() != "")
            {
                Invoke((MethodInvoker)delegate
                {
                    //richTextBox1.Text += s;
                });
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //richTextBox1.Text = "";
            var Thread = new Thread(new ParameterizedThreadStart(doit))
            {
                IsBackground = true
            };
            Thread.Start(Thread);
        }

        private string combine(int id)
        {
            try
            {
                var ip = Pdt.FindByid(id).ip.Trim();
                var p = new cprinter { ip = ip, id = id };
                var mod = SNMPget(ip, new string[] { "1.3.6.1.2.1.1.1.0" })[0];
                if (mod == null)
                    return "";
                else if (mod.Contains("Samsung"))
                    p.Model = model.Samsung;
                else if (mod.Contains("KYOCERA"))
                    p.Model = model.Kyocera;
                else if (mod.Contains("HP"))
                    p.Model = model.HP;

                if (p.Model == model.Kyocera)
                {
                    var result = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.2.1.3.1",
                                                            "1.3.6.1.4.1.1347.43.10.1.1.12.1.1",
                                                            "1.3.6.1.2.1.25.3.5.1.2.1" });
                    p.name = "Kyocera " + result[0];
                    p.count = result[1];
                    if ((p.error = ErrorMessageText[Convert.ToInt32(result[2])])==null)
                    {
                        p.error = "uncnown error " + result[2] + " add this error";
                    }
                }
                else if (p.Model == model.HP)
                {
                    var result = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.2.1.3.1",
                                                            "1.3.6.1.4.1.11.2.3.9.4.2.1.4.1.2.5.0",
                                                            "1.3.6.1.2.1.25.3.5.1.2.1"});
                    p.name = result[0];
                    p.count = result[1];
                    if ((p.error = ErrorMessageText[Convert.ToInt32(result[2])]) == null)
                    {
                        p.error = "uncnown error " + result[2] + " add this error";
                    }
                }
                else if (p.Model == model.Samsung)
                {
                    var result = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.2.1.3.1",
                                                            "1.3.6.1.2.1.25.3.5.1.2.1"});
                    p.name = result[0];
                    p.count = "\"Can't extract the number of sheets\"";
                    if ((p.error = ErrorMessageText[Convert.ToInt32(result[1])]) == null)
                    {
                        p.error = "uncnown error " + result[2] + " add this error";
                    }
                }
                if (p.name != null && p.count != null && p.name != "" && p.count != "")
                {
                    var good = false;
                    /*
                    foreach (printersDataSet.PrintersRow item in Pdt.Rows)
                    {
                        if (item.id == p.id)
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

                    if (p.error != "no error")
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
                            ELadapter.Insert(p.id, DateTime.Now, Convert.ToInt32(p.count), p.error, 0);
                            CEadapter.Insert(p.id, p.error);
                            CEdt.AddCurrentErorrsRow(p.id, p.error);
                        }
                        this.currentErorrsTableAdapter.Fill(this.printersDataSet.CurrentErorrs);
                    }
                    else
                    {
                        good = true;
                        foreach (printersDataSet.CurrentErorrsRow item in CEdt.Rows)
                        {
                            if (item.printer_id == p.id)
                            {
                                good = false;

                                var result = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.5.1.2.1" })[0];

                                if (item.Error == "no tonner" && (p.error = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.5.1.2.1" })[0]) == "0")
                                {
                                    Thread.Sleep(10000);
                                    if (item.Error == "no tonner" && (p.error = SNMPget(ip, new string[] { "1.3.6.1.2.1.25.3.5.1.2.1" })[0]) == "0")
                                    {
                                        var i_am_here = true;
                                    }
                                }
                                break;
                            }
                        }
                        if (!good)
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                good = true;
                                foreach (msng item in msngcentr.Controls)
                                {
                                    if (item.print.id == p.id)
                                    {
                                        good = false;
                                        break;
                                    }
                                }
                                if (good)
                                {
                                    msngcentr.Add(p, Pdt.FindByid(p.id).coment, CEdt.FindByprinter_id(p.id).Error);
                                }
                            });
                        }
                    }
                    return p.ip + " " + p.name + " " + p.count + " " + p.error + "\n";
                }
            }
            catch (Exception e)
            {
                //throw;
            }
            return "";
        }

        private void doit(object sender)
        {
            ErrorMessageText[0] = "no error";
            ErrorMessageText[10] = "крышка открыта";

            /*Надо переделать

            ErrorMessageText[0] = "no error";
            ErrorMessageText[6] = "paper is jammed in the orinter";
            ErrorMessageText[10] = "крышка открыта";
            ErrorMessageText[12] = "no tonner";///
            ErrorMessageText[18] = "no tonner";
            ErrorMessageText[26] = "no tonner";
            ErrorMessageText[32] = "low tonner";
            ErrorMessageText[48] = "no error";
            ErrorMessageText[50] = "no tonner";
            ErrorMessageText[66] = "нет бумаги";

            */

            //string[][] ipse = GetIP();

            //string ips = ip.ToString().Remove(ip.ToString().IndexOf('.', 3));

            var CEdt1 = CEdt;
            foreach (printersDataSet.CurrentErorrsRow p in CEdt1.Rows)
                combine(p.printer_id);

            var IPdt1 = Pdt;
            foreach (printersDataSet.PrintersRow p in IPdt1.Rows)
            {
                var Thre = new Thread(new ParameterizedThreadStart(B3_DoWork))
                {
                    IsBackground = true
                };
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
            var entry = Dns.GetHostEntry(ip).HostName;
            var ips = ip.ToString().Remove(ip.ToString().IndexOf('.', 3));

            using (var objMC = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                byte[] mask = null;
                var objMOC = objMC.GetInstances();
                foreach (var objMO in objMOC)
                {
                    if (!(bool)objMO["ipEnabled"]) continue;
                    var subnets = (string[])objMO["IPSubnet"];
                    mask = Dns.GetHostAddresses(subnets[0])[0].GetAddressBytes();
                }
                var ipBytes = ip.GetAddressBytes();
                var maskBytes = mask.ToArray();
                var startIPBytes = new byte[ipBytes.Length];
                var endIPBytes = new byte[ipBytes.Length];
                for (int i = 0; i < ipBytes.Length; i++)
                {
                    startIPBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                    endIPBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
                }
                var startIP = new IPAddress(startIPBytes);
                var endIP = new IPAddress(endIPBytes);
                var bstart = startIP.ToString().Split('.');
                var bend = endIP.ToString().Split('.');
                return new string[2][] { bstart, bend };
            }
        }

        private static string[] SNMPget(string ip, string[] oid)
        {
            var output = new string[oid.Length];
            var agent = new IpAddress(ip);
            var pdu = new Pdu(PduType.Get);
            foreach (var item in oid)
                pdu.VbList.Add(item);
            using (var target = new UdpTarget((IPAddress)agent, 161, 512, 1))
            {
                try
                {
                    var result2 = (SnmpV2Packet)target.Request(pdu, new AgentParameters(SnmpVersion.Ver2, new OctetString("public")));
                    for (int i = 0; i < result2.Pdu.VbList.Count; i++)
                        if (oid[i] == "1.3.6.1.2.1.25.3.5.1.2.1")
                            output[i] = ((OctetString)(result2.Pdu.VbList[i].Value)).ToArray()[0].ToString();
                        else
                            output[i] = result2.Pdu.VbList[i].Value.ToString();
                }
                catch (Exception)
                {
                    var result1 = (SnmpV1Packet)target.Request(pdu, new AgentParameters(SnmpVersion.Ver1, new OctetString("public")));
                    for (int i = 0; i < result1.Pdu.VbList.Count; i++)
                        if (oid[i] == "1.3.6.1.2.1.25.3.5.1.2.1")
                            output[i] = ((OctetString)(result1.Pdu.VbList[i].Value)).ToArray()[0].ToString();
                        else
                            output[i] = result1.Pdu.VbList[i].Value.ToString();
                }
                return output;
            }
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            //richTextBox1.Text = "";
            var IPdt1 = Pdt;
            foreach (printersDataSet.PrintersRow p in IPdt1.Rows)
            {
                var Thre = new Thread(new ParameterizedThreadStart(B3_DoWork))
                {
                    IsBackground = true,
                    Priority = ThreadPriority.Lowest
                };
                Thre.Start(p.id);
            }
            this.currentErorrsTableAdapter.Fill(this.printersDataSet.CurrentErorrs);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            var CEdt1 = CEdt;
            foreach (printersDataSet.CurrentErorrsRow p in CEdt1.Rows)
                combine(p.printer_id);
            this.currentErorrsTableAdapter.Fill(this.printersDataSet.CurrentErorrs);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "printersDataSet.CurrentErorrs". При необходимости она может быть перемещена или удалена.
            this.currentErorrsTableAdapter.Fill(this.printersDataSet.CurrentErorrs);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "printersDataSet.Printers". При необходимости она может быть перемещена или удалена.
            this.printersTableAdapter.Fill(this.printersDataSet.Printers);
        }

        public struct cprinter
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