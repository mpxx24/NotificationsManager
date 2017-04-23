using System;

namespace JobOffersProvider.Exceptions {
    public class HtmlElementNotFountException : Exception {
        public HtmlElementNotFountException(string elementName) : base(elementName) {
        }
    }
}