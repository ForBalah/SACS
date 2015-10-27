using System;
using System.Collections.Generic;
using NUnit.Framework;
using SACS.Common.DTOs;

namespace SACS.UnitTests.TestClasses.Common
{
    [TestFixture]
    public class PagingResultTests
    {
        [Test]
        public void Constructor_CanInitializeEmptyList()
        {
            PagingResult<string> emptyResult = new PagingResult<string>();

            Assert.IsNotNull(emptyResult.Collection, "Collection in struct is null now?");
            Assert.AreEqual(0, emptyResult.NumberOfPages, "struct NumberOfPages must be zero");
            Assert.AreEqual(0, emptyResult.Total, "struct Total must be zero");
            Assert.AreEqual(0, emptyResult.PageSize, "struct PageSize must be zero");
        }

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
                // test passes if there is an argument exception
            }
        }

        [Test]
        public void Constructor_CannotInitializeWithAnyNegativeValues()
        {
            try
            {
                PagingResult<string> negative1 = new PagingResult<string>(new List<string>(), -1, 0);
                Assert.Fail("Total incorrectly assigned negative");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                PagingResult<string> negative2 = new PagingResult<string>(new List<string>(), 0, -1);
                Assert.Fail("PageSize incorrectly assigned negative");
            }
            catch (ArgumentException)
            {
            }
        }

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
            }
        }

        [Test]
        public void NumberOfPages_ZeroTotalCountResultsInZeroNumberOfPages()
        {
            PagingResult<string> zeroTotal = new PagingResult<string>(new List<string>(), 0, 10);
            Assert.AreEqual(0, zeroTotal.NumberOfPages);
        }

        [Test]
        public void NumberOfPages_ZeroPageSizeAlwaysEqualsOne()
        {
            PagingResult<string> zeroPageSize = new PagingResult<string>(new List<string> { "a", "b" }, 2, 0);
            Assert.AreEqual(1, zeroPageSize.NumberOfPages);
        }

        [Test]
        public void NumberOfPages_PageSizeOfOneAlwaysReturnsTotal()
        {
            PagingResult<string> onePageSize = new PagingResult<string>(new List<string> { "a", "b" }, 10, 1);
            Assert.AreEqual(10, onePageSize.NumberOfPages);
        }

        [Test]
        public void NumberOfPages_PageSizeOf5_OneLessThanTheEdgeCaseShouldReturnOnePage()
        {
            PagingResult<string> fivePager = new PagingResult<string>(new List<string> { "a", "b" }, 4, 5);
            Assert.AreEqual(1, fivePager.NumberOfPages);
        }

        [Test]
        public void NumberOfPages_PageSizeOf5_OnFiveTotalShouldReturnOnePage()
        {
            PagingResult<string> fivePager = new PagingResult<string>(new List<string> { "a", "b" }, 5, 5);
            Assert.AreEqual(1, fivePager.NumberOfPages);
        }

        [Test]
        public void NumberOfPages_PageSizeOf5_OneOneMoreTheEdgeShouldReturn2Pages()
        {
            PagingResult<string> fivePager = new PagingResult<string>(new List<string> { "a", "b" }, 6, 5);
            Assert.AreEqual(2, fivePager.NumberOfPages);
        }
    }
}
