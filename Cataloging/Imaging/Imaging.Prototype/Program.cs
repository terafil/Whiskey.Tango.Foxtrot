using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Imaging.Prototype
{
    class Program
    {
        private static void CatalogImages(string path)
        {
            IEnumerable<string> imageFiles = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

            Dictionary<string, IList<string>> imageCatalog = new Dictionary<string, IList<string>>();

            Console.WriteLine("{0} image files found", imageFiles.Count());
            float processed = 0;
            foreach (string file in imageFiles)
            {
                Regex r = new Regex(":");
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    //Console.WriteLine("processing file {0}", file);
                    string key = "unknown";
                    if (myImage.PropertyIdList.Contains(36867))
                    {
                        PropertyItem propItem = myImage.GetPropertyItem(36867);
                        string dateTakenString = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                        DateTime dateTaken;
                        if(DateTime.TryParse(dateTakenString, out dateTaken))
                        {
                            key = string.Format("{0}-{1}", dateTaken.Year, CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTaken.Month));
                        }
                    }

                    if (imageCatalog.ContainsKey(key))
                    {
                        imageCatalog[key].Add(file);
                    }
                    else
                    {
                        imageCatalog.Add(key, new List<string> { file });
                    }

                    processed++;
                    Console.Clear();
                    Console.WriteLine("Processed file {0}%", (processed/imageFiles.Count())*100);

                }
            }

            using (FileStream stream = File.Create("ImageCatalog.txt"))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                foreach (var pair in imageCatalog)
                {
                    writer.WriteLine(string.Format("----{0}----", pair.Key));
                    foreach (string image in pair.Value)
                    {
                        writer.WriteLine(image);
                    }
                }
            }

            Console.WriteLine("File Created");
        }
        private static void ListProperties(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                foreach(var prop in myImage.PropertyItems)
                {
                    if (prop.Type == 2)
                    {
                        Console.WriteLine("({2}){0}-{1}", prop.Id, Encoding.UTF8.GetString(prop.Value), prop.Type);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            string path = null;
            if(args.Length < 1)
            {
                Console.WriteLine("Specify Image Directory");
                path = Console.ReadLine();
            }
            else
            {
                path = args[0];
            }

            CatalogImages(path);
            //ListProperties(path);
        }
    }
}
