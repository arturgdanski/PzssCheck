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

                foreach (var entry in licensesTable)
                {
                    // Check if this is a complete license entry (ignore "acquire license" entries)
                    if(entry.Count > 5)
                    {
                        licenses.Add(new License(
                            entry[LicenseNameColId], entry[LicenseNoCoId],
                            entry[LicenseTypeCoId], entry[LicenseBeginDateCoId], entry[LicenseEndDateCoId]));
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}", Properties.Resources.ExceptionTranslation, ex.Message);
            }

            return licenses.ToArray();
        }

        // Column descriptor
        private const int LicenseNameColId = 0;
        private const int LicenseNoCoId = 2;
        private const int LicenseTypeCoId = 4;
        private const int LicenseBeginDateCoId = 5;
        private const int LicenseEndDateCoId = 6;

    }
}
