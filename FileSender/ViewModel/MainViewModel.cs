using FileSender.Additional_Classes;
using FileSender.Command;
using FileSender.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileSender.ViewModel
{
    public class MainViewModel
    {
        public string Path1 { get; set; }

        public RelayCommand ConnectBtn { get; set; }
        public RelayCommand DisConnectBtn { get; set; }
        public RelayCommand SelectBtn { get; set; }
        byte[] b;

        public MainViewModel(MainWindow mainWindow)
        {
            ConnectBtn = new RelayCommand((sender) =>
            {

                var client = new TcpClient();
                var ip = IPAddress.Parse(Connect.IPAdress);
                var port = Connect.Port;
                var ep = new IPEndPoint(ip, port);
                try
                {
                    client.Connect(ep);
                    if (client.Connected)
                    {
                        var writer = Task.Run(() =>
                        {
                            while (true)
                            {

                                var stream = client.GetStream();
                                var bw = new BinaryWriter(stream);


                                bw.Write(Path1);
                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    bw.Write(mainWindow.NametxtBox.Text);
                                });
                                break;
                            };
                        });
                    }
                    else
                    {
                        MessageBox.Show("CLinet doesnt connected");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
            DisConnectBtn = new RelayCommand((sender) =>
            {

            });
            SelectBtn = new RelayCommand((sender) =>
            {
                try
                {
                    var open = new Microsoft.Win32.OpenFileDialog();

                    open.Multiselect = false;
                    open.Filter = "Pdf Files (*.txt)|*.pdf|Image Files|*.jpg;*.jpeg;*.png;";
                    open.ShowDialog();
                    if (".pdf".Equals(Path.GetExtension(open.FileName), StringComparison.OrdinalIgnoreCase))
                    {
                        Path1 = open.FileName;
                    }
                    else
                    {
                        Path1 = open.FileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
    }
}
