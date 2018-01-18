using HispRequirements.DTO;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HispRequirements
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SearchDelphiPkgReg()
        {
            using (StreamReader file = File.OpenText(@".\JsonFiles\RegDelphiPackages.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                DelphiPackageRootDto delphiPackageReg = (DelphiPackageRootDto)serializer.Deserialize(file, typeof(DelphiPackageRootDto));

                RegistryKey OurKey;
                if (delphiPackageReg.RootRegistry == "HKEY_CURRENT_USER")
                    OurKey = Registry.CurrentUser;
                else
                    OurKey = Registry.LocalMachine;
                
                foreach (DelphiPackageDto pkg in delphiPackageReg.Packages)
                {
                    RegistryKey subKey = OurKey.OpenSubKey(delphiPackageReg.Subkey);
                    
                    int row = dataGridView1.Rows.Add("Delphi Package", pkg.Package, pkg.Description);
                    if (subKey.GetValueNames().ToList().IndexOf(pkg.Package) >= 0)
                        dataGridView1.Rows[row].Cells[3].Value = Resource1.Ok;
                    else
                        dataGridView1.Rows[row].Cells[3].Value = Resource1.NotOk;
                }
            }
        }

        private void SearchOcxReg()
        {
            using (StreamReader file = File.OpenText(@".\JsonFiles\RegOcx.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                OcxRootDto ocxRoot = (OcxRootDto)serializer.Deserialize(file, typeof(OcxRootDto));

                RegistryKey OurKey;
                OurKey = Registry.ClassesRoot;
                
                foreach (OcxDto ocx in ocxRoot.Values)
                {
                    RegistryKey subKey = OurKey.OpenSubKey(ocxRoot.Subkey);

                    int row = dataGridView1.Rows.Add("OCX", ocx.Classe, ocx.Arquivo);
                    if (subKey.GetSubKeyNames().ToList().IndexOf(ocx.Classe) >= 0)
                        dataGridView1.Rows[row].Cells[3].Value = Resource1.Ok;
                    else
                        dataGridView1.Rows[row].Cells[3].Value = Resource1.NotOk;
                }
            }
        }

        private void GetSubKeys(RegistryKey SubKey)
        {
            if (SubKey.GetValue(null) != null)
            {
                if (SubKey.GetValue(null).ToString().ToLower().Contains("c:\\area de trabalho\\") ||
                    SubKey.GetValue(null).ToString().ToLower().Contains("informix"))
                {   
                    int row = dataGridView1.Rows.Add("TypeLib", SubKey.ToString(), SubKey.GetValue(null).ToString());
                    dataGridView1.Rows[row].Cells[3].Value = Resource1.Ok;
                }
            }

            foreach (string sub in SubKey.GetSubKeyNames())
            {
                RegistryKey local = Registry.ClassesRoot;
                
                local = SubKey.OpenSubKey(sub);
                GetSubKeys(local); // By recalling itselfit makes sure it get all the subkey names
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dataGridView1.Rows.Clear();
                try
                {
                    /*
                    RegistryKey OurKey = Registry.ClassesRoot;
                    OurKey = OurKey.OpenSubKey(@"TypeLib");
                    GetSubKeys(OurKey);

                    OurKey = Registry.ClassesRoot;
                    OurKey = OurKey.OpenSubKey(@"CLSID");
                    GetSubKeys(OurKey);
                    */
                    SearchDelphiPkgReg();
                    SearchOcxReg();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao acessar o registro: " + ex.Message);
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
