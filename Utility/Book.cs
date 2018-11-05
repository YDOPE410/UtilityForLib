using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Utility
{
    public class Book
    {
        [XmlAttribute] public string kode;
        [XmlAttribute] public string author;
        [XmlAttribute] public string name;
        [XmlAttribute] public string city;
        [XmlAttribute] public string pubHouse;
        [XmlAttribute] public int year;
        [XmlAttribute] public int page;
        [XmlAttribute] public int instance;
        [XmlAttribute] public bool image;
        [XmlAttribute] public bool cd;

        public Book()
        {
            kode = "";
            author = "";
            name = "";
            city = "";
            pubHouse = "";
            year = 0;
            page = 0;
            instance = 0;
            image = false;
            cd = false;
        }

        public Book(string kode, string author, string name, string city, string pubHouse, int year, int page, int instance, bool image, bool cd)
        {
            this.kode = kode;
            this.author = author;
            this.name = name;
            this.city = city;
            this.pubHouse = pubHouse;
            this.year = year;
            this.page = page;
            this.instance = instance;
            this.image = image;
            this.cd = cd;
        }
    }
}