using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using testMvc.Models;

namespace testMvc.Controllers
{
    internal class BL
    {
        private masterEntities db = new masterEntities();

        public BL()
        {
        }

        internal void Create(citeis citeis)
        {
            int idNew = 0;
            List<citeis> tempCities = db.citeis.ToList();
            tempCities.ForEach(e => {
                idNew = e.id > idNew ? e.id : idNew;
            }
            );
            citeis.id = idNew + 1;
            db.citeis.Add(citeis);
            db.SaveChanges();
        }

        internal citeis find(int? id)
        {
            return db.citeis.Find(id);
        }

        static string HexToAscii(string hexString)
        {
            string[] hexValuesSplit = hexString.Split('-'); // Split by '-'
            byte[] bytes = new byte[hexValuesSplit.Length];

            for (int i = 0; i < hexValuesSplit.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexValuesSplit[i], 16); // Convert hex to byte
            }

            return Encoding.ASCII.GetString(bytes); // Convert bytes to ASCII string
        }
        static string BytesToHebrew(byte[] bytes)
        {
            return Encoding.GetEncoding("Windows-1255").GetString(bytes); // Convert bytes to Hebrew string
        }


        internal object GetCitiesPageing(string sortOrder, string currentFilter, string searchString, int? page, dynamic viewBag, System.Web.HttpRequestBase request)
        {
            string chineseText = "担"; // Chinese text
            Encoding chineseEncoding = Encoding.GetEncoding("GB2312"); // Encoding for Chinese characters (GB2312)

            // Convert Chinese text to byte array using the specified encoding
            byte[] chineseBytes = chineseEncoding.GetBytes(chineseText);

            // Convert byte array to Unicode string
            string unicodeText = Encoding.Unicode.GetString(chineseBytes);

            Console.WriteLine("Unicode representation of Chinese text: ");
            Console.WriteLine(unicodeText);


            
                string chineseText9 = "担"; // Chinese text
                byte[] bytes = Encoding.Unicode.GetBytes(chineseText9); // Convert to Unicode bytes
                string unicodeString = BitConverter.ToString(bytes);
            string englishString = HexToAscii(unicodeString);
            Console.WriteLine(englishString);
            string hebrewString = BytesToHebrew(bytes);
            Console.WriteLine(hebrewString);



            List<citeis> employees = db.citeis.ToList();

            viewBag.CurrentSort = sortOrder;
            if (request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            viewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (viewBag.CurrentSort != "DESC")
            {
                employees = employees.OrderByDescending(e => e.name).ToList();
            }
            else
            {
                employees = employees.OrderBy(e => e.name).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return employees.ToPagedList(pageNumber, pageSize);
        }
    }
}