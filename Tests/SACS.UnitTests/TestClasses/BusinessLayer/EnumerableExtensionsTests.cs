using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SACS.Common.Extensions;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Category("EnumerableExtensions")]
        [Test]
        public void GetIntervals_WorksForEmptyList()
        {
            var data = new List<string>();

            var result = data.GetIntervals(0);
            Assert.AreEqual(0, result.Count());
        }

        [Category("EnumerableExtensions")]
        [Test]
        public void GetIntervals_BreaksForInvalidMaxPoints()
        {
            var data = new List<string>();

            try
            {
                var result = data.GetIntervals(-1);
                Assert.Fail("Should not support negative 'maxPoints'");
            }
            catch (AssertionException)
            {
                throw;
            }
            catch
            {
                Assert.Pass();
            }
        }

        [Category("EnumerableExtensions")]
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetIntervals_ReturnsEquidistantIntervals(int testIndex)
        {
            // source list contains the max points, and the list to act on.
            // result list contains what the final result should be
            // the testIndex is the indexes for sourceList and resultList to use in the test
            var sourceList = new List<MaxPointList>
            {
                new MaxPointList(1, Enumerable.Range(0, 1)),
                new MaxPointList(1, Enumerable.Range(0, 3)),
                new MaxPointList(3, Enumerable.Range(0, 5)),
                new MaxPointList(4, Enumerable.Range(0, 10))
            };

            var resultList = new List<List<int>>
            {
                new List<int> { 0 },
                new List<int> { 0, 2 },
                new List<int> { 0, 2, 4 },
                new List<int> { 0, 3, 6, 9 }
            };

            var finalResult = sourceList[testIndex].ListSource.GetIntervals(sourceList[testIndex].MaxPoint);

            CollectionAssert.AreEqual(resultList[testIndex], finalResult);
        }

        [Category("EnumerableExtensions")]
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void GetIntervals_ReturnsNonEquidistantIntervalsAsEvenlyAsPossible(int testIndex)
        {
            // source list contains the max points, and the list to act on.
            // result list contains what the final result should be
            // the testIndex is the indexes for sourceList and resultList to use in the test
            var sourceList = new List<MaxPointList>
            {
                new MaxPointList(6, Enumerable.Range(0, 8)),
                new MaxPointList(10, Enumerable.Range(0, 11)),
            };

            var resultList = new List<List<int>>
            {
                new List<int> { 0, 1, 3, 4, 6, 7 },
                new List<int> { 0, 1, 2, 3, 4, 6, 7, 8, 9, 10 },
            };

            var finalResult = sourceList[testIndex].ListSource.GetIntervals(sourceList[testIndex].MaxPoint);

            CollectionAssert.AreEqual(resultList[testIndex], finalResult);
        }

        [Category("EnumerableExtensions")]
        [Test]
        public void GetIntervals_ReturnsAllIndicesForMaxPointGreaterThanCollectionCount()
        {
            var sourceList = new List<int> { 9, 9, 9, 9, 9 };
            var result = sourceList.GetIntervals(10);
            Assert.AreEqual(new List<int> { 0, 1, 2, 3, 4 }, result);
        }

        // Just a wrapper class to clean up writing out the "Tuple<int, List<int>>"
        private class MaxPointList : Tuple<int, List<int>>
        {
            public int MaxPoint
            {
                get { return this.Item1; }
            }

            public IList<int> ListSource
            {
                get { return this.Item2; }
            }

            public MaxPointList(int maxPoints, IEnumerable<int> list)
                : base(maxPoints, list.ToList())
            {
            }
        }
    }
}