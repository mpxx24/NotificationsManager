using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Sites.TrojmiastoPl {
    public class TrojmiastoPlWebsiteProvider : IJobWebsiteTask {
        private static string searchUrl => $"http://ogloszenia.trojmiasto.pl/praca/s,.net,slb,4,o0,1.html";
        private static string defaultLogoAddress => "https://cdn2.iconfinder.com/data/icons/line-weather/130/No_Data-128.png";

        public async Task<IEnumerable<JobModel>> GetJobOffers() {
            var result = new List<JobModel>();

            var site = new HttpClient();
            var doc = await site.GetByteArrayAsync(searchUrl).ConfigureAwait(false);
            var source = Encoding.GetEncoding("utf-8").GetString(doc, 0, doc.Length - 1);
            source = WebUtility.HtmlDecode(source);
            var document = new HtmlDocument();
            document.LoadHtml(source);

            var content = document.DocumentNode.Descendants(HtmlElementsHelper.Div)
                .First(x => x.Attributes.Contains(HtmlElementsHelper.Id) && x.Attributes[HtmlElementsHelper.Id].Value.Contains("wcontent"));

            var offers = content.ChildNodes.FindFirst(HtmlElementsHelper.List)
                .Descendants(HtmlElementsHelper.ListElement)
                .Where(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("list-work-elem"));

            foreach (var li in offers) { 
                    var offerLink = li.Descendants(HtmlElementsHelper.HeaderTwo)?
                        .First()?.Descendants(HtmlElementsHelper.Link)?
                        .First()?.Attributes[HtmlElementsHelper.Address].Value;

                    var text = li.Descendants(HtmlElementsHelper.HeaderTwo)?.First()?.InnerText;

                    var companyName = li.Descendants(HtmlElementsHelper.Paragraph).Any(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Equals("company"))
                        ? li.Descendants(HtmlElementsHelper.Paragraph)?.First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Equals("company")).InnerText
                        : "-";

                    var companyLogoLink = li.Descendants(HtmlElementsHelper.Image).Any()
                        ? li.Descendants(HtmlElementsHelper.Image)?.First()?.Attributes[HtmlElementsHelper.SourceLink]?.Value
                        : defaultLogoAddress;

                    var cities = (li?.Descendants(HtmlElementsHelper.ListElement)).Any(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("place"))
                        ? PrepareCompanyCity(li?.Descendants(HtmlElementsHelper.ListElement).First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("place")).InnerText)
                        : new List<string>();

                    var dateAdded = PrepareDateAdded(li?.Descendants(HtmlElementsHelper.ListElement)
                        .First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("added"))
                        .InnerText);

                    result.Add(new JobModel
                        {
                            Title = text,
                            Company = companyName,
                            Added = dateAdded,
                            Cities = cities,
                            Logo = companyLogoLink,
                            OfferAddress = offerLink
                        }
                    );
                }

            return result;
        }
        private static DateTime PrepareDateAdded(string dateAdded) {
            var result = new DateTime();
            var test = dateAdded.Trim().Split(' ');
            var number = int.TryParse(test[1], out int delta);

            //oh come on
            if (number)
                if (test[2].Contains("godz")) {
                    result = DateTime.Now.AddHours(-delta).Date;
                } else if (test[2].Contains("min")) {
                    result = DateTime.Now.AddMinutes(-delta).Date;
                } else {
                    result = DateTime.Today.AddDays(-delta).Date;
                }

            return result;
        }

        private static IList<string> PrepareCompanyCity(string cities) {
            return new List<string> {cities};
        }
    }
}