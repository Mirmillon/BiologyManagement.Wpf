using BiologyManagement.Models.LoincFrance.EnumTable;
using System.Collections.Generic;
using System.Windows;
using BiologyManagement.Wpf.Db;
using BiologyManagement.Models.LoincFrance;


namespace BiologyManagement.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Maintenance : Window
    {



        Management management = new Management();

        public Maintenance()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            //versions = new List<VersionUtil>();
            //versions = management.GetVersionsRemplacement();

            //gridData.ItemsSource = versions;

            //tbChapitre.Text = versions.Count.ToString();
            //tbShort.Text = codes.Count.ToString();

            // management.TrTDataVersion(versions);

            List<VersionMAJ> listeReference = new List<VersionMAJ>();
            List<VersionUtil> listeATraiter = new List<VersionUtil>();
            List<VersionMAJ> newListeLOINCVersion = new List<VersionMAJ>();

            listeReference = management.GetTR_Version();
            listeATraiter = management.GetVersionsRemplacement();
            management.TrTDataVersion(listeATraiter);
            newListeLOINCVersion = management.SetListeActeVersion(listeATraiter, listeReference);
            management.SetVersion(newListeLOINCVersion);


        }

        private void btnLoadShort_Click(object sender, RoutedEventArgs e)
        {
            /*
            shortCodes = new List<ShortCode>();
            shortCodes = management.GetListeShortCodeTemps();
            gridShort.ItemsSource = shortCodes;
            tbShort.Text = shortCodes.Count.ToString();
            */
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            /*
            newShortCode = new List<ShortCode>();
            newShortCode = management.ChangeTemps(shortCodes);
            gridChange.ItemsSource = newShortCode;
            tbChange.Text = management.NbChangement.ToString();
            */

        }

        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            /*
            management.UpdateTemps(newShortCode);
            */
            
        }

       
    }
}
