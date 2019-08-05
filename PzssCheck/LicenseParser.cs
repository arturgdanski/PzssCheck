using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace PzssCheck
{
    class LicenseParser
    {
        public LicenseParser()
        {

        }

        public License[] Parse(string htmlLicenses)
        {
            List<License> licenses = new List<License>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlLicenses);

            try
            {
                var licensesTable = htmlDoc.DocumentNode.SelectSingleNode("//table[@id='list']")
                    .Descendants("tr")
                    .Skip(1)
                    .Where(tr => tr.Elements("td").Count() > 1)
                    .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                    .ToList();

                if (licensesTable.Count > 0)
                {
                    if (licensesTable[0].Count > 2)
                    {
                        foreach (var entry in licensesTable)
                        {
                            licenses.Add(new License(entry[0], entry[2]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }

            return licenses.ToArray();
        }

    }
}
