using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System.IO.Compression;

namespace Utility
{
    public partial class MainForm : Form
    {
        private SQLiteConnection db;
        List<string> paths = new List<string>();
        List<Book> books = new List<Book>();
        int i;
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog browser = new OpenFileDialog();
            
            if (browser.ShowDialog() == DialogResult.OK) {
                if (!paths.Contains(browser.FileName))
                {
                    lbFiles.Items.Add($"{++i}) {browser.FileName.Substring(browser.FileName.LastIndexOf("\\") + 1)}");
                    paths.Add(browser.FileName);
                    if (browser.FileName.Contains(".xml"))
                    {
                        ClassSerialisation.DeserializeFromXML<List<Book>>(ref books, browser.FileName);
                        for (int j = 0; j < books.Count; j++)
                        {
                            dgvTables.Rows.Add(books[j].kode, books[j].author, books[j].name, books[j].city, books[j].pubHouse, books[j].year, books[j].page, books[j].instance, books[j].cd, books[j].image);
                        }
                    }
                }
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClassSerialisation.SeriliazeToXMl<List<string>>(ref paths, "paths.xml");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            db = new SQLiteConnection("Data Source=db1.db");

            db.Open();
            ClassSerialisation.DeserializeFromXML<List<string>>(ref paths, "paths.xml");
            for (int j = 0; j < paths.Count; j++)
            {
                lbFiles.Items.Add($"{++i}) {paths[j].Substring(paths[j].LastIndexOf("\\") + 1)}");
            }
        }
    }
}
