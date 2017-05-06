using System;
using System.Collections.Generic;

namespace JobOffersProvider.Common.Models {
    public class JobModel {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Company { get; set; }

        public IList<string> Cities { get; set; }

        public DateTime Added { get; set; }

        public string Logo { get; set; }

        public string OfferAddress { get; set; }

        public string Description { get; set; }
    }
}