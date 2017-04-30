using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IslamiTexts.Common.Web;
using IslamiTexts.Models;

namespace Tests
{
    [TestClass]
    public class PaginationTests
    {
        [TestMethod]
        public void SinglePageMaximumResults()
        {
            SearchResults results = new SearchResults(0, 10, 10);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(1, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(1, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForFirstCurrentPageEvenNumberLimit()
        {
            SearchResults results = new SearchResults(0, 10, 200);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(1, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(10, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForFirstCurrentPageOddNumberLimit()
        {
            SearchResults results = new SearchResults(0, 11, 200);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(1, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(11, pagination.LastPageNoToShow, "LastPageNoToShow");
        }


        [TestMethod]
        public void FirstAndLastPagesCorrectForThirdCurrentPageEvenNumberLimit()
        {
            SearchResults results = new SearchResults(20, 10, 200);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(3, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(10, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForThirdCurrentPageOddNumberLimit()
        {
            SearchResults results = new SearchResults(20, 10, 200);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(3, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(11, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForMidCurrentPageEvenNumberLimit()
        {
            SearchResults results = new SearchResults(40, 10, 200);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(5, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(10, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForMidCurrentPageOddNumberLimit()
        {
            SearchResults results = new SearchResults(50, 10, 200);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(6, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(11, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForLastCurrentPageEvenNumberLimit()
        {
            SearchResults results = new SearchResults(200, 10, 204);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(12, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(21, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(21, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForLastCurrentPageOddNumberLimit()
        {
            SearchResults results = new SearchResults(200, 10, 204);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(11, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(21, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(21, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForThirdLastCurrentPageEvenNumberLimit()
        {
            SearchResults results = new SearchResults(180, 10, 204);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(12, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(19, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(21, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForThirdLastCurrentPageOddNumberLimit()
        {
            SearchResults results = new SearchResults(180, 10, 204);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(11, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(19, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(21, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForMidFromLastCurrentPageEvenNumberLimit()
        {
            SearchResults results = new SearchResults(150, 10, 204);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(12, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(16, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(21, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForMidFromLastCurrentPageOddNumberLimit()
        {
            SearchResults results = new SearchResults(150, 10, 204);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(11, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(16, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(21, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForInbetweenCurrentPageEvenNumberLimit()
        {
            SearchResults results = new SearchResults(100, 10, 204);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(7, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(11, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(16, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForInbetweenCurrentPageOddNumberLimit()
        {
            SearchResults results = new SearchResults(100, 10, 204);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(6, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(11, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(16, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForFirstCurrentPageWithSmallNumberOfPagesEvenNumberLimit()
        {
            SearchResults results = new SearchResults(10, 10, 29);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(2, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(3, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForFirstCurrentPageWithSmallNumberOfPagesOddNumberLimit()
        {
            SearchResults results = new SearchResults(10, 10, 29);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(2, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(3, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForMidCurrentPageWithSmallNumberOfPagesEvenNumberLimit()
        {
            SearchResults results = new SearchResults(20, 10, 49);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(3, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(5, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForMidCurrentPageWithSmallNumberOfPagesOddNumberLimit()
        {
            SearchResults results = new SearchResults(20, 10, 49);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(3, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(5, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForSinglePageEvenNumberLimit()
        {
            SearchResults results = new SearchResults(0, 10, 7);
            Pagination pagination = new Pagination(results, 10);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(1, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(1, pagination.LastPageNoToShow, "LastPageNoToShow");
        }

        [TestMethod]
        public void FirstAndLastPagesCorrectForSinglePageOddNumberLimit()
        {
            SearchResults results = new SearchResults(0, 10, 7);
            Pagination pagination = new Pagination(results, 11);
            Assert.AreEqual(1, pagination.FirstPageNoToShow, "FirstPageNoToShow");
            Assert.AreEqual(1, pagination.CurrentPageNo, "CurrentPageNo");
            Assert.AreEqual(1, pagination.LastPageNoToShow, "LastPageNoToShow");
        }
    }
}
