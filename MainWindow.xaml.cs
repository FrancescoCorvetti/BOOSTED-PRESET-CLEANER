using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PuliziaPC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public DirectoryInfo winTemp;
        public DirectoryInfo appTemp;


        public MainWindow()
        {
            InitializeComponent();
            winTemp = new DirectoryInfo(@"C:\Windows\temp");
            GetDate();
            appTemp = new DirectoryInfo(System.IO.Path.GetTempPath());
        }

        public long DirSize(DirectoryInfo dir)
        {
            return dir.GetFiles().Sum(fi => fi.Length) + dir.GetDirectories().Sum(di => DirSize(di));
        }

        private void Button_Site_Click(object sender, RoutedEventArgs e)
        {
            try { 
            Process.Start(new ProcessStartInfo("https://google.com")
            {
                UseShellExecute = true
            });
            } catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void CrealTempData(DirectoryInfo di)
        {
            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                    Console.WriteLine(file.FullName);
                } catch (Exception ex)
                {
                    continue;
                }
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                try
                {
                    dir.Delete(true);
                    Console.WriteLine(dir.FullName);
                } catch (Exception ex)
                {
                    continue;
                }
            }
        }

        private void Button_cron_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TODO: Cronologia", "Cronologia", MessageBoxButton.OKCancel, MessageBoxImage.Information);

        }




        private void analizzare1_Click(object sender, RoutedEventArgs e)
        {
            AnalyseFolders();
        }

        public void AnalyseFolders()
        {
            Console.WriteLine("Analisi iniziata... ");
            long totalSize = 0;

            try
            {
            totalSize += DirSize(winTemp) / 1000000;
            totalSize += DirSize(appTemp) / 1000000;

            } catch (Exception ex)
            {
                Console.WriteLine("no file caso: " + ex.Message);
            }

            UltimaData.Content = totalSize + "MB";
            SpazioFormatta.Content = DateTime.Today;
            SaveDate();
        }
        private void Button_Pul_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Pulizia in corso... ");
            btnClear.Content = "PULIZIA IN CORSO";

            Clipboard.Clear();

            try
            {

            CrealTempData(winTemp);
            CrealTempData(appTemp);
            } catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }


            btnClear.Content = "PULIZIA TERMINATA";
           AnalisiTitolo.Content = "Nessun analisi da effettuare";
            UltimaData.Content = "0 MB";

        }

        public void SaveDate()
        {
            string date = DateTime.Today.ToString();
            File.WriteAllText("Date.txt", date);
        }
        public void GetDate()
        {
            try
            {
            string datatesto = File.ReadAllText("Date.txt");
                SpazioFormatta.Content = datatesto;
            } catch
            {
                Console.WriteLine("Niente Date.txt creato");
            }
 
        }

    }
}
