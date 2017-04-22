using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Common {
    public class PracujPlWebsiteProvider : IJobWebsiteTask {
        private static string pracujPlAddress => "https://www.pracuj.pl/";
        private static string searchUrl => $"{pracujPlAddress}/praca/.NET;kw/Gda%C5%84sk-x44-%20Gdynia-x44-%20Sopot;wp";
        private static string defaultLogoAddress => "https://cdn2.iconfinder.com/data/icons/line-weather/130/No_Data-128.png";

        public async Task<IEnumerable<JobModel>> GetJobOffers() {
            var result = new List<JobModel>();

            var site = new HttpClient();
            var doc = await site.GetByteArrayAsync(searchUrl).ConfigureAwait(false);
            var source = Encoding.GetEncoding("utf-8").GetString(doc, 0, doc.Length - 1);
            source = WebUtility.HtmlDecode(source);
            var document = new HtmlDocument();
            document.LoadHtml(source);

            var content = document.DocumentNode.Descendants(HtmlElementsHelper.Section)
                .First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("offer rightCol"));

            var offers = content.ChildNodes.FindFirst(HtmlElementsHelper.List)
                .Descendants(HtmlElementsHelper.ListElement)
                .Where(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("o-list_item "));

            foreach (var li in offers) {
                var offerLink = PrepareOfferLink(li.Descendants(HtmlElementsHelper.HeaderTwo)?.First()?
                    .Descendants(HtmlElementsHelper.Link)?.First()?.Attributes[HtmlElementsHelper.Address].Value);

                var text = PrepareOfferName(li.Descendants(HtmlElementsHelper.HeaderTwo)?.First()?.InnerText);

                var companyName = PrepareCompanyName(li.Descendants(HtmlElementsHelper.HeaderThree)?.First()?.InnerText);

                var companyLogoLink = li.Descendants(HtmlElementsHelper.Image).Any()
                    ? PrepareLogo(li.Descendants(HtmlElementsHelper.Image)?.First()?.Attributes[HtmlElementsHelper.DataOriginal]?.Value)
                    : defaultLogoAddress;

                var footerParagraph = li.Descendants(HtmlElementsHelper.Paragraph)?.First();

                var cities = PrepareCompanyCity(footerParagraph?.Descendants(HtmlElementsHelper.Span)
                    .First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("o-list_item_desc_location"))
                    .InnerText);

                var dateAdded = PrepareDateAdded(footerParagraph?.Descendants(HtmlElementsHelper.Span)
                    .First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("o-list_item_desc_date"))
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

        private static string  PrepareOfferLink(string link) {
            return $"{pracujPlAddress}{link}";
        }

        private static List<string> PrepareCompanyCity(string city) {
            var cities = new List<string>();
            var data = city.Split(',');

            if (data.Length == 2) cities.Add(data[0]);
            else if (data.Length > 2) cities.AddRange(data.Take(data.Length - 1));

            return cities.Select(c => c.Trim()).ToList();
        }

        private static DateTime PrepareDateAdded(string added) {
            DateTime.TryParse(added, out DateTime dateAdded);

            return dateAdded;
        }

        private static string PrepareOfferName(string name) {
            return name
                .Replace(Environment.NewLine, "")
                .Replace("!SUPER OFERTA", "")
                .Trim();
        }

        private static string PrepareCompanyName(string companyName) {
            return companyName
                .Replace(Environment.NewLine, "")
                .Trim();
        }

        private static string PrepareLogo(string logo) {
            return $"http:{logo}";
        }
    }
}