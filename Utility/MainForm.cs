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
            object id;
            object[] authors;
            object[] codes;
            OpenFileDialog browser = new OpenFileDialog();
            SQLiteCommand cmd = new SQLiteCommand(db);
            dgvTables.Rows.Clear();
            if (browser.ShowDialog() == DialogResult.OK)
            {
                if (!paths.Contains(browser.FileName))
                {
                    if (browser.FileName.Contains(".xml"))
                    {
                        ClassSerialisation.DeserializeFromXML<List<Book>>(ref books, browser.FileName);
                        lbFiles.Items.Add($"{++i}) {browser.FileName.Substring(browser.FileName.LastIndexOf("\\") + 1)} {books.Count}");
                        File.Move(browser.FileName, browser.FileName.Insert(browser.FileName.Length - 4, $"_{books.Count}"));
                        paths.Add(browser.FileName.Insert(browser.FileName.Length - 4, $"_{books.Count}"));
                        for (int j = 0; j < books.Count; j++)
                        {
                            dgvTables.Rows.Add(books[j].kode, books[j].author, books[j].name, books[j].city, books[j].pubHouse, books[j].year, books[j].page, books[j].instance, books[j].cd, books[j].image);
                            authors = books[j].author.Split(new string[] { ", " }, StringSplitOptions.None);
                            for (int l = 0; l < authors.Length - 1; l++)
                            {
                                cmd.CommandText = $"insert into Authors('name') values('{authors[l]}')";
                                cmd.ExecuteNonQuery();
                            }
                            cmd.CommandText = $"DELETE FROM Authors WHERE id NOT IN(SELECT MIN(id) FROM Authors GROUP BY name)";
                            cmd.ExecuteNonQuery();

                            cmd.CommandText = $"insert into Publishers('city', 'name') values('{books[j].city}','{books[j].pubHouse}')";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = $"DELETE FROM Publishers WHERE id NOT IN( SELECT MIN(id) FROM Publishers GROUP BY name, city)";
                            cmd.ExecuteNonQuery();


                            cmd.CommandText = $"select id from Publishers where city = '{books[j].city}' and name = '{books[j].pubHouse}'";
                            id = cmd.ExecuteScalar();
                            cmd.CommandText = $"insert into Books('name','publisher_id','year','pages_count', 'is_pictures', 'examples_count', 'is_CD') values('{books[j].name.ToString().Replace("\'", "`")}','{id}', '{books[j].year}', '{books[j].page}', '{books[j].image}', '{books[j].instance}', '{books[j].cd}')";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = $"DELETE FROM Books WHERE id NOT IN(SELECT MIN(id) FROM Books GROUP BY name, publisher_id, year, pages_count, is_pictures, examples_count, is_CD)";
                            cmd.ExecuteNonQuery();

                            for (int l = 0; l < authors.Length - 1; l++)
                            {
                                cmd.CommandText = $"insert into Books_Authors('book_id', 'author_id') values((select id from Books where name = '{books[j].name.ToString().Replace("\'", "`")}'),(select id from Authors where name = '{authors[l]}'))";
                                cmd.ExecuteNonQuery();
                            }

                            codes = books[j].kode.Split(new string[] { ", " }, StringSplitOptions.None);
                            for (int l = 0; l < codes.Length - 1; l++)
                            {
                                cmd.CommandText = $"insert into Ciphers('book_id', 'cipher') values((select id from Books where name = '{books[j].name.ToString().Replace("\'", "`")}'),'{codes[l]}')";
                                cmd.ExecuteNonQuery();
                            }

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

                lbFiles.Items.Add($"{++i}) {paths[j].Substring(paths[j].LastIndexOf("\\") + 1)} ");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = new SQLiteCommand(db);
            cmd.CommandText = $"delete from Authors";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"delete from Books";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"delete from Publishers";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"delete from Ciphers";
            cmd.ExecuteNonQuery();
            cmd.CommandText = $"delete from Books_Authors";
            cmd.ExecuteNonQuery();


        }

    }
}
