using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;

namespace BulkFileRe_Namer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> paths = new List<string>();
        private string selectedFolder = System.Windows.Forms.Application.StartupPath;
        private string extension = "jpg";

        public MainWindow()
        {
            InitializeComponent();
            radBtnFileDate.IsChecked = true;

            txtBxPrefixCustom.IsEnabled = false;
            txtBxPath.IsEnabled = false;
        }


        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowser.Description = "Browse for Folder";
            folderBrowser.SelectedPath = selectedFolder;

            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedFolder = folderBrowser.SelectedPath;
                txtBxPath.Text = selectedFolder;
            }
        }

        private void btnRename_Click(object sender, RoutedEventArgs e)
        {
            string[] files = Directory.GetFiles(txtBxPath.Text, "*." + extension, SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                string directory = Path.GetDirectoryName(files[i]);
                //string currentExtension = Path.GetExtension(files[i]);
                //if (string.Compare(extension, currentExtension) == 0)
                //{
                    string fileName = Path.GetFileNameWithoutExtension(files[i]);
                    if (radBtnFileDate.IsChecked == true)
                    {
                        DateTime fileDate = File.GetLastAccessTime(files[i]);
                        fileName = fileDate.ToString("yyyyMMdd-HHmmss_") + fileName;
                    }
                    else if (radBtnCustom.IsChecked == true)
                    {
                        fileName = txtBxPrefixCustom.Text + "_" + fileName;
                    }
                    File.Move(files[i], directory + Path.DirectorySeparatorChar + fileName + "." + extension);
               // }
            }
        }

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static List<string> ProcessDirectory(string targetDirectory)
        {
            List<string> files = new List<string>();
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                files.Add(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                List<string> subDirectoryFiles = ProcessDirectory(subdirectory);
                foreach (string file in subDirectoryFiles)
                {

                }
            }

            return files;
        }

        private void radBtnCustom_Checked(object sender, RoutedEventArgs e)
        {
            txtBxPrefixCustom.IsEnabled = true;
        }
        private void radBtnCustom_Unchecked(object sender, RoutedEventArgs e)
        {
            txtBxPrefixCustom.IsEnabled = false;
        }

        private void txtBxPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(txtBxPath.Text))
            {
                txtBxPath.IsEnabled = true;
            }
        }

        private void radBtnFileDate_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
