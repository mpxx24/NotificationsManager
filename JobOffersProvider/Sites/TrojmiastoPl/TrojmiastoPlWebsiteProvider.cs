﻿using System;
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
        private static string defaultLogoAddress => "https://cdn2.iconfinder.com/data/icons/line-weather/130/No_Data-128.png";

        public async Task<IEnumerable<JobModel>> GetJobOffers(string searchText) {
            var result = new List<JobModel>();

            var httpClient = new HttpClient();
            var searchUrl = this.GetSearchUrl(searchText);
            var doc = await httpClient.GetByteArrayAsync(searchUrl).ConfigureAwait(false);
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
                var offerLink = li.Descendants(HtmlElementsHelper.HeaderTwo)?.First()?.Descendants(HtmlElementsHelper.Link)?.First()?.Attributes[HtmlElementsHelper.Address].Value;

                var text = li.Descendants(HtmlElementsHelper.HeaderTwo)?.First()?.InnerText;

                var companyName = li.Descendants(HtmlElementsHelper.Paragraph).Any(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Equals("company"))
                    ? li.Descendants(HtmlElementsHelper.Paragraph)?.First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Equals("company")).InnerText
                    : "N/A";

                var companyLogoLink = li.Descendants(HtmlElementsHelper.Image).Any()
                    ? li.Descendants(HtmlElementsHelper.Image)?.First()?.Attributes[HtmlElementsHelper.SourceLink]?.Value
                    : defaultLogoAddress;

                var cities = (li?.Descendants(HtmlElementsHelper.ListElement)).Any(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("place"))
                    ? PrepareCompanyCity(li?.Descendants(HtmlElementsHelper.ListElement).First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("place")).InnerText)
                    : new List<string>();

                var dateAdded = PrepareDateAdded(li?.Descendants(HtmlElementsHelper.ListElement)
                    .First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Contains("added"))
                    .InnerText);

                result.Add(new JobModel {
                        Id = Guid.NewGuid(),
                        Title = text,
                        Company = companyName,
                        Added = dateAdded,
                        Cities = cities,
                        Logo = companyLogoLink,
                        OfferAddress = offerLink,
                        WebsiteType = WebsiteType.TrojmiastoPl
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
            
            var offer = document.DocumentNode.Descendants(HtmlElementsHelper.Div)
                .Any(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Equals("ogl-content")) 
                ? document.DocumentNode.Descendants(HtmlElementsHelper.Div)
                    .First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Equals("ogl-content"))
                : document.DocumentNode.Descendants(HtmlElementsHelper.Div)
                    .First(x => x.Attributes.Contains(HtmlElementsHelper.Class) && x.Attributes[HtmlElementsHelper.Class].Value.Equals("ogl-details"));

            foreach (var descendant in offer.Descendants()) {
                if (descendant.Name == HtmlElementsHelper.Emphasized || descendant.Name == HtmlElementsHelper.Strong || descendant.Name == HtmlElementsHelper.Text) {
                    offerDescription.Append($"{descendant.InnerText}{Environment.NewLine}");
                }
                else if (descendant.Name == HtmlElementsHelper.List) {
                    foreach (var li in descendant.Descendants().Where(x => x.Name == HtmlElementsHelper.ListElement)) {
                        offerDescription.Append($"\t-{li.InnerText}{Environment.NewLine}");
                    }
                }
            }

            var result = new JobOfferDetailsModel {
                OfferDescription = offerDescription.ToString(),
                //CompanyDescription = companyDescription.ToString()
            };


            return result;
        }

        private string GetSearchUrl(string searchtText) {
            return $"http://ogloszenia.trojmiasto.pl/praca/s,.{Uri.EscapeDataString(searchtText)},slb,4,o0,1.html";
        }

        private static DateTime PrepareDateAdded(string dateAdded) {
            var result = new DateTime();
            var test = dateAdded.Trim().Split(' ');
            var number = int.TryParse(test[1], out int delta);

            //oh come on
            if (number) {
                if (test[2].Contains("godz")) {
                    result = DateTime.Now.AddHours(-delta).Date;
                } else if (test[2].Contains("min")) {
                    result = DateTime.Now.AddMinutes(-delta).Date;
                } else {
                    result = DateTime.Today.AddDays(-delta).Date;
                }
            }

            return result;
        }

        private static IList<string> PrepareCompanyCity(string cities) {
            return new List<string> {cities};
        }
    }
}