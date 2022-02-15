using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//Application Used Library
using System.Net;
using System.Net.NetworkInformation;

namespace IPAddress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string hostname = Dns.GetHostName();
            lbDisplay.Items.Add("Hostname: " + hostname);
            System.Net.IPAddress[] addresses = Dns.GetHostAddresses(hostname);
            foreach (System.Net.IPAddress addr in addresses)
            {
                lbDisplay.Items.Add("IP Address: " + addr.ToString() + " " + addr.AddressFamily);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            System.Net.NetworkInformation.NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface nic in nics)
            {
                //basically, there are just a bunch of properties to query
                lbDisplay.Items.Add("ID: " + nic.Id);
                lbDisplay.Items.Add("Name: " + nic.Name);
                lbDisplay.Items.Add("Physical Address: " + nic.GetPhysicalAddress());
                lbDisplay.Items.Add("IP Addresses: ");
                foreach (UnicastIPAddressInformation addr in nic.GetIPProperties().UnicastAddresses)
                {
                    lbDisplay.Items.Add(addr.Address 
                        //+ " " + DateTime.Now 
                        + " (least expires " + new TimeSpan(0, 0, (int)addr.DhcpLeaseLifetime)
                        +")"); //" - {0} (lease expires {1})",
                }
                lbDisplay.Items.Add(" ");
            }
        /*
        // Only proceed if there is a network available.
if (NetworkInterface.GetIsNetworkAvailable())
{
// Get the set of all NetworkInterface objects for the local
// machine.
NetworkInterface[] interfaces =
NetworkInterface.GetAllNetworkInterfaces();
        // Iterate through the interfaces and display information.
foreach (NetworkInterface ni in interfaces)
{
// Report basic interface information.
Console.WriteLine("Interface Name: {0}", ni.Name);
Console.WriteLine(" Description: {0}", ni.Description);
Console.WriteLine(" ID: {0}", ni.Id);
Console.WriteLine(" Type: {0}", ni.NetworkInterfaceType);
Console.WriteLine(" Speed: {0}", ni.Speed);
Console.WriteLine(" Status: {0}", ni.OperationalStatus);
// Report physical address.
Console.WriteLine(" Physical Address: {0}",
ni.GetPhysicalAddress().ToString());
// Report network statistics for the interface.
Console.WriteLine(" Bytes Sent: {0}",
ni.GetIPv4Statistics().BytesSent);
Console.WriteLine(" Bytes Received: {0}",
ni.GetIPv4Statistics().BytesReceived);
// Report IP configuration.
Console.WriteLine(" IP Addresses:");
foreach (UnicastIPAddressInformation addr
in ni.GetIPProperties().UnicastAddresses)
{
Console.WriteLine(" - {0} (lease expires {1})",
addr.Address,
DateTime.Now +
new TimeSpan(0, 0, (int)addr.DhcpLeaseLifetime));
}
Console.WriteLine(Environment.NewLine);
}
}
else
{
Console.WriteLine("No network available.");
}
*/
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lbDisplay.Items.Clear();
        }

        // Declare a method to handle NetworkAvailabilityChanged events.
        private void NetworkAvailabilityChanged(
        object sender, NetworkAvailabilityEventArgs e)
        {
            // Report whether the network is now available or unavailable.
            if (e.IsAvailable)
            {
                lbDisplay.Items.Add("Network Available");
            }
            else
            {
                lbDisplay.Items.Add("Network Unavailable");
            }
        }

        // Declare a method to handle NetworkAdressChanged events.
        private void NetworkAddressChanged(object sender, EventArgs e)
        {
            lbDisplay.Items.Add("Current IP Addresses:");
            // Iterate through the interfaces and display information.
            foreach (NetworkInterface ni in
            NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation addr
                in ni.GetIPProperties().UnicastAddresses)
                {
                    lbDisplay.Items.Add(addr.Address
                        + " - " + DateTime.Now 
                        + "(lease expires " + new TimeSpan(0, 0, (int)addr.DhcpLeaseLifetime) 
                        + ")");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Add the handlers to the NetworkChange events.
            NetworkChange.NetworkAvailabilityChanged +=
            NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged +=
            NetworkAddressChanged;
        }
    }
}
