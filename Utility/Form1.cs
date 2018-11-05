using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Utility
{
    public partial class Form1 : Form
    {
       
        public List<string> paths = new List<string>();
        int i = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog browser = new OpenFileDialog
            {
                Multiselect = true
            };
            if (browser.ShowDialog() == DialogResult.OK) {
                foreach (string file in browser.FileNames) {
                    if (!paths.Contains(file)) { 
                        lBFiles.Items.Add($"{++i}) {file.Substring(file.LastIndexOf("\\")+1)}");
                    paths.Add(file);
                    }
                }
                
            }
        }
        
private void tsmiOpenFile_Click(object sender, EventArgs e)
        {
            string buffer = lBFiles.SelectedItem.ToString();
            
            Process.Start(paths[buffer[0]-49]);
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClassSerialisation.SeriliazeToXMl<List<string>>(ref paths, "allPaths.xml");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClassSerialisation.DeserializeFromXML<List<string>>(ref paths, "allPaths.xml");
            for(int j = 0; j < paths.Count; j++)
            {
                lBFiles.Items.Add($"{++i}) {paths[j].Substring(paths[j].LastIndexOf("\\") + 1)}");
            }
        }
    }
}
