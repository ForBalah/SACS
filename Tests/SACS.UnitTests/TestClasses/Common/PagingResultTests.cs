using System;
using System.Collections.Generic;
using NUnit.Framework;
using SACS.Common.DTOs;

namespace SACS.UnitTests.TestClasses.Common
{
    [TestFixture]
    public class PagingResultTests
    {
        [Category("PagingResultTests")]
        [Test]
        public void Constructor_CanInitializeEmptyList()
        {
            PagingResult<string> emptyResult = new PagingResult<string>();

            Assert.IsNotNull(emptyResult.Collection, "Collection in struct is null now?");
            Assert.AreEqual(0, emptyResult.NumberOfPages, "struct NumberOfPages must be zero");
            Assert.AreEqual(0, emptyResult.Total, "struct Total must be zero");
            Assert.AreEqual(0, emptyResult.PageSize, "struct PageSize must be zero");
        }

        [Category("PagingResultTests")]
        [Test]
        public void Constructor_CannotInitializeNullList()
        {
            try
            {
                PagingResult<string> nullResult = new PagingResult<string>(null, 0, 0);
                Assert.Fail("struct Collection not meant to be null");
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }
            catch
            {
                throw;
            }
        }

        [Category("PagingResultTests")]
        [Test]
        [TestCase(-1, 0, "Total incorrectly assigned negative")]
        [TestCase(0, -1, "PageSize incorrectly assigned negative")]
        public void Constructor_CannotInitializeWithAnyNegativeValues(int total, int pageSize, string failMessage)
        {
            try
            {
                PagingResult<string> negative1 = new PagingResult<string>(new List<string>(), total, pageSize);
                Assert.Fail(failMessage);
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
            catch
            {
                throw;
            }
        }

        [Category("PagingResultTests")]
        [Test]
        public void Constructor_TotalCannotBeLessThanCollectionSize()
        {
            try
            {
                PagingResult<string> noPageResult = new PagingResult<string>(new List<string> { "a", "b" }, 0, 10);
                Assert.Fail("Total was made less than collection size");
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
            catch
            {
                throw;
            }
        }

        [Category("PagingResultTests")]
        [Test]
        public void NumberOfPages_ZeroTotalCountResultsInZeroNumberOfPages()
        {
            PagingResult<string> zeroTotal = new PagingResult<string>(new List<string>(), 0, 10);
            Assert.AreEqual(0, zeroTotal.NumberOfPages);
        }

        [Category("PagingResultTests")]
        [Test]
        public void NumberOfPages_ZeroPageSizeAlwaysEqualsOne()
        {
            PagingResult<string> zeroPageSize = new PagingResult<string>(new List<string> { "a", "b" }, 2, 0);
            Assert.AreEqual(1, zeroPageSize.NumberOfPages);
        }

        [Category("PagingResultTests")]
        [Test]
        public void NumberOfPages_PageSizeOfOneAlwaysReturnsTotal()
        {
            PagingResult<string> onePageSize = new PagingResult<string>(new List<string> { "a", "b" }, 10, 1);
            Assert.AreEqual(10, onePageSize.NumberOfPages);
        }

        [Category("PagingResultTests")]
        [Test]
        public void NumberOfPages_PageSizeOf5_OneLessThanTheEdgeCaseShouldReturnOnePage()
        {
            PagingResult<string> fivePager = new PagingResult<string>(new List<string> { "a", "b" }, 4, 5);
            Assert.AreEqual(1, fivePager.NumberOfPages);
        }

        [Category("PagingResultTests")]
        [Test]
        public void NumberOfPages_PageSizeOf5_OnFiveTotalShouldReturnOnePage()
        {
            PagingResult<string> fivePager = new PagingResult<string>(new List<string> { "a", "b" }, 5, 5);
            Assert.AreEqual(1, fivePager.NumberOfPages);
        }

        [Category("PagingResultTests")]
        [Test]
        public void NumberOfPages_PageSizeOf5_OnOneMoreTheEdgeShouldReturn2Pages()
        {
            PagingResult<string> fivePager = new PagingResult<string>(new List<string> { "a", "b" }, 6, 5);
            Assert.AreEqual(2, fivePager.NumberOfPages);
        }
    }
}