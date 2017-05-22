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

namespace JobOffersProvider.Sites.PracujPl {
    public class PracujPlWebsiteProvider : IJobWebsiteTask {
        private static string pracujPlAddress => "https://www.pracuj.pl/";
        private static string searchUrl => $"{pracujPlAddress}/praca/.NET;kw/Gda%C5%84sk-x44-%20Gdynia-x44-%20Sopot;wp";
        private static string defaultLogoAddress => "https://cdn2.iconfinder.com/data/icons/line-weather/130/No_Data-128.png";

        public async Task<IEnumerable<JobModel>> GetJobOffers() {
            var result = new List<JobModel>();

            var httpClient = new HttpClient();
            var doc = await httpClient.GetByteArrayAsync(searchUrl).ConfigureAwait(false);
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
                var offerLink = PrepareOfferLink(li.Descendants(HtmlElementsHelper.HeaderTwo)?.First()?.Descendants(HtmlElementsHelper.Link)?.First()?.Attributes[HtmlElementsHelper.Address].Value);

                var text = PrepareOfferName(li.Descendants(HtmlElementsHelper.HeaderTwo)?.First()?.InnerText);

                var companyName = PrepareCompanyName(li.Descendants(HtmlElementsHelper.HeaderThree)?.First()?.InnerText);

                var companyLogoLink = li.Descendants(HtmlElementsHelper.Image).Any()
                    ? PrepareLogo(li.Descendants(HtmlElementsHelper.Image)?.First()?.Attributes[HtmlElementsHelper.DataOriginal]?.Value)
                    : defaultLogoAddress;

                var footerParagraph = li.Descendants(HtmlElementsHelper.Paragraph)?.First();

                var cities = PrepareCompanyCity(footerParagraph?.Descendants(HtmlElementsHelper.Span)
                    .First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("o-list_item_desc_location")).InnerText);

                var dateAdded = PrepareDateAdded(footerParagraph?.Descendants(HtmlElementsHelper.Span)
                    .First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("o-list_item_desc_date")).InnerText);

                result.Add(new JobModel {
                        Id = Guid.NewGuid(),
                        Title = text,
                        Company = companyName,
                        Added = dateAdded,
                        Cities = cities,
                        Logo = companyLogoLink,
                        OfferAddress = offerLink,
                        WebsiteType = JobWebsiteTaskProviderType.PracujPl
                    }
                );
            }

            return result;
        }

        public async Task<JobOfferDetailsModel> GetJobOfferDetails(string offerAddress) {
            var companyDescription = new StringBuilder();
            var offerDescription = new StringBuilder();

            var httpClient = new HttpClient();
            var doc = await httpClient.GetByteArrayAsync(offerAddress).ConfigureAwait(false);
            var source = Encoding.GetEncoding("utf-8").GetString(doc, 0, doc.Length - 1);
            source = WebUtility.HtmlDecode(source);
            var document = new HtmlDocument();
            document.LoadHtml(source);

            var content = document.DocumentNode.Descendants(HtmlElementsHelper.Div)
                .First(x => x.Attributes.Contains(HtmlElementsHelper.Id) && x.Attributes[HtmlElementsHelper.Id].Value.Equals("main"));

            var company = content.Descendants(HtmlElementsHelper.Div)
                .First(x => x.Attributes.Contains(HtmlElementsHelper.Id) && x.Attributes[HtmlElementsHelper.Id].Value.Equals("company"));

            foreach (var descendant in company.Descendants()) {
                if (descendant.Name == HtmlElementsHelper.Paragraph) {
                    companyDescription.Append($"{descendant.InnerText}{Environment.NewLine}");
                }
            }

            var offer = content.Descendants(HtmlElementsHelper.Div)
                .First(x => x.Attributes.Contains(HtmlElementsHelper.Id) && x.Attributes[HtmlElementsHelper.Id].Value.Equals("description"));

            foreach (var descendant in offer.Descendants()) {
                if (descendant.Name == HtmlElementsHelper.Paragraph) {
                    offerDescription.Append($"{descendant.InnerText}{Environment.NewLine}");
                } else if (descendant.Name == HtmlElementsHelper.List) {
                    foreach (var li in descendant.Descendants().Where(x => x.Name == HtmlElementsHelper.ListElement)) {
                        offerDescription.Append($"\t-{li.InnerText}{Environment.NewLine}");
                    }
                }

            }

            var result = new JobOfferDetailsModel {
                OfferDescription = offerDescription.ToString(),
                CompanyDescription = companyDescription.ToString()
            };


            return result;
        }

        private static string PrepareOfferLink(string link) {
            return $"{pracujPlAddress}{link}";
        }

        private static List<string> PrepareCompanyCity(string city) {
            var cities = new List<string>();
            var data = city.Split(',');

            if (data.Length == 2) {
                cities.Add(data[0]);
            } else if (data.Length > 2) {
                cities.AddRange(data.Take(data.Length - 1));
            }

            return cities.Select(c => c.Trim()).ToList();
        }

        private static DateTime PrepareDateAdded(string added) {
            DateTime.TryParse(added, out DateTime dateAdded);

            return dateAdded;
        }

        private static string PrepareOfferName(string name) {
            return name
                .Replace(Environment.NewLine, string.Empty)
                .Replace("!SUPER OFERTA", string.Empty)
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