using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Extender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<String, List<String>> DicMarged { get; set; }
        //public ObservableCollection<MargeCol> MargedCol { get; set; }
        List<string> lastKnownFiles=new List<string>();
        Random rnd = new Random();
        private String FilePath = "C:\\suhas\\Expand\\";
        private string prefix = "exp";
        public MainWindow()
        {
            InitializeComponent();
            //MargedCol=new ObservableCollection<MargeCol>();
            //lstMargedFile.DataContext = MargedCol;
            

            DicMarged=new Dictionary<string, List<string>>();
           
            //DicMarged.Add("suhas",new List<string>());
            //DicMarged["suhas"].Add("1");
            //DicMarged["suhas"].Add("2");
            //DicMarged["suhas"].Add("3");
            //DicMarged["suhas"].Add("4");
            //DicMarged["suhas"].Add("5");


            //DicMarged.Add("Roshni", new List<string>());
            //DicMarged["Roshni"].Add("5");
            //DicMarged["Roshni"].Add("4");
            //DicMarged["Roshni"].Add("1");
            //DicMarged["Roshni"].Add("2");
            //DicMarged["Roshni"].Add("3");

            grdMargedItem.DataContext = DicMarged;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(FilePath);
            Thread th = new Thread(GetFile);
            th.Start();

            //GetFile();
        }

        

        private void GetFile()
        {
            while (true)
            {
                int x = rnd.Next();
                string margedFile = FilePath + prefix + x + ".txt";
                Dispatcher.BeginInvoke(DispatcherPriority.Send, new ThreadStart(() => AddExpanderFile(margedFile)));

                //AddExpanderFile(margedFile);
                FileStream fsRandomFile = File.Create(margedFile);
                StreamWriter swFile = new StreamWriter(fsRandomFile);
                while (swFile.BaseStream.Length <= 100000000)
                {
                    string newFile = HasNewFiles();
                    if (!string.IsNullOrEmpty(newFile))
                    {
                        string contain = File.ReadAllText(newFile);
                        swFile.WriteLine(contain);
                        File.Delete(newFile);
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, new ThreadStart(() => AddGeneratorFile(newFile, margedFile)));

                        //AddGeneratorFile(newFile, margedFile);
                    }
                }
            }
        }

        private void AddGeneratorFile(string newFile, string margedFile)
        {
            if (DicMarged.ContainsKey(margedFile))
            {
                DicMarged[margedFile].Add(newFile);
                grdMargedItem.InvalidateVisual();
            }
        }

        private void AddExpanderFile(string margedFile)
        {
            DicMarged.Add(margedFile,new List<string>());
        }

        string HasNewFiles()
        {
            List<string> files = Directory.GetFiles("c:\\suhas\\").ToList();

            foreach (string s in files)
            {
                if (!lastKnownFiles.Contains(s))
                    return s;
            }

            return string.Empty;
        }
    }

    public class MargeCol
    {
        public string MargedFile = string.Empty;
        public ObservableCollection<string> MsrgedCol=new ObservableCollection<string>();
    }
}
