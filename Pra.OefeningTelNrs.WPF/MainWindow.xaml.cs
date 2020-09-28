using System;
using System.Windows;
using System.Data;
using System.IO;

using Pra.OefeningTelNrs.Core;
using System.Windows.Controls;

namespace Pra.OefeningTelNrs.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }
        #region Global variables
        DataSet dsPhone;
        string XMLFolder = Directory.GetCurrentDirectory() + "/XMLData";
        string XMLFileName = Directory.GetCurrentDirectory() + "/XMLData/Phone.xml";
        bool isNew;
        #endregion
        #region Window events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewDefault();
            dsPhone = XMLHelper.ReadXML(XMLFolder, XMLFileName);
            if(dsPhone == null)
            {
                dsPhone = Phone.CreateDataSet();
            }
            if(dsPhone.Tables[0].Rows.Count == 0)
            {
                Phone.DoSomeSeedings(dsPhone);
            }
            FillListbox();
            ClearControls();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XMLHelper.WriteXML(XMLFolder, XMLFileName, dsPhone);
        }
        #endregion
        #region GUI stuff

        private void ViewDefault()
        {
            grpSearch.IsEnabled = true;
            lstPhoneNumbers.IsEnabled = true;
            grpCRUDButtons.Visibility = Visibility.Visible;
            grpDetails.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
        }
        private void ViewEdit()
        {
            grpSearch.IsEnabled = false;
            lstPhoneNumbers.IsEnabled = false;
            grpCRUDButtons.Visibility = Visibility.Hidden;
            grpDetails.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
        }
        private void ClearControls()
        {
            txtNumber.Text = "";
            txtName.Text = "";
            txtFirstname.Text = "";
            chkIsMobile.IsChecked = false;       
        }
        private void FillListbox()
        {
            lstPhoneNumbers.Items.Clear();
            ListBoxItem itm;
            string filter = "%";
            if (txtSearch.Text.Trim().Length > 0)
            {
                filter = "%" + txtSearch.Text.Trim() + "%";
            }
            DataView dv = new DataView(dsPhone.Tables[0]);
            dv.RowFilter = "naam like '" + filter + "' ";
            dv.Sort = "naam asc, voornaam asc";
            foreach (DataRowView rw in dv)
            {

                itm = new ListBoxItem();
                itm.Content = rw["naam"] + " " + rw["voornaam"] + " : " + rw["nummer"];
                itm.Tag = rw["nummer"];
                lstPhoneNumbers.Items.Add(itm);
            }
        }
        #endregion

        #region Event handlers

        #endregion

        private void lstPhoneNumbers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearControls();
            if (lstPhoneNumbers.SelectedIndex < 0) return;

            ListBoxItem itm = (ListBoxItem)lstPhoneNumbers.SelectedItem;
            string searchNumber = itm.Tag.ToString();
            DataView dv = new DataView(dsPhone.Tables[0]);
            dv.RowFilter = "nummer = '" + searchNumber + "' ";
            if (dv.Count > 0)
            {
                txtNumber.Text = dv[0]["nummer"].ToString();
                txtName.Text = dv[0]["naam"].ToString();
                txtFirstname.Text = dv[0]["voornaam"].ToString();
                if (dv[0]["mobile"].ToString() == "1")
                    chkIsMobile.IsChecked = true;
                else
                    chkIsMobile.IsChecked = false;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FillListbox();
        }

        private void btnEndSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            FillListbox();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ViewEdit();
            ClearControls();
            isNew = true;
            txtNumber.Focus();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstPhoneNumbers.SelectedIndex < 0) return;
            ViewEdit();
            isNew = false;
            txtNumber.Focus();
            txtNumber.SelectAll();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ViewDefault();
            lstPhoneNumbers_SelectionChanged(null, null);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string number = txtNumber.Text.Trim();
            string name = txtName.Text.Trim();
            string firstname = txtFirstname.Text.Trim();
            byte ismobile = 0;
            if ((bool)chkIsMobile.IsChecked)
                ismobile = 1;
            try
            {
                if (isNew)
                {
                    DataRow rw = dsPhone.Tables[0].NewRow();
                    rw["nummer"] = number;
                    rw["naam"] = name;
                    rw["voornaam"] = firstname;
                    rw["mobile"] = ismobile;
                    dsPhone.Tables[0].Rows.Add(rw);
                }
                else
                {
                    ListBoxItem itm = (ListBoxItem)lstPhoneNumbers.SelectedItem;
                    string zoeknummer = itm.Tag.ToString();
                    DataRow rw = dsPhone.Tables[0].Rows.Find(zoeknummer);
                    rw["nummer"] = number;
                    rw["naam"] = name;
                    rw["voornaam"] = firstname;
                    rw["mobile"] = ismobile;
                }
            }
            catch (Exception fout)
            {
                MessageBox.Show(fout.Message, "DB ERROR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            FillListbox();
            ViewDefault();
            foreach (ListBoxItem itm in lstPhoneNumbers.Items)
            {
                if (itm.Tag.ToString() == number)
                {
                    itm.IsSelected = true;
                    lstPhoneNumbers_SelectionChanged(null, null);
                }
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstPhoneNumbers.SelectedIndex < 0) return;
            if (MessageBox.Show("Ben je zeker", "Nummer wissen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ListBoxItem itm = (ListBoxItem)lstPhoneNumbers.SelectedItem;
                string deleteNumber = itm.Tag.ToString();
                DataRow rw = dsPhone.Tables[0].Rows.Find(deleteNumber);
                dsPhone.Tables[0].Rows.Remove(rw);
                FillListbox();
                ClearControls();
            }
        }
    }
}
