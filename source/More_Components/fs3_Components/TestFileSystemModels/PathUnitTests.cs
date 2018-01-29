using FileSystemModels.Models.FSItems.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestFileSystemModels
{
    [TestClass]
    public class PathUnitTests
    {
        /// <summary>
        /// Test Requirement: One Letter or less is not a valid path
        /// -> null is returned
        /// </summary>
        [TestMethod]
        public void TestPathNormalization()
        {
            var ret = PathModel.NormalizePath("C");

            var ret1 = PathModel.NormalizePath(string.Empty);

            var ret2 = PathModel.NormalizePath(null);

            Assert.AreEqual(ret, ret1);
            Assert.AreEqual(ret1, ret2);
            Assert.AreEqual(ret2, null);
        }

        /// <summary>
        /// Test Requirement: 2 Letter or 3 Letter path
        /// could be drive reference and should end with a backslash
        /// </summary>
        [TestMethod]
        public void TestDrivePathNormalization()
        {
            var ret = PathModel.NormalizePath("C:");

            var ret1 = PathModel.NormalizePath(@"C:\");

            Assert.AreEqual(ret, ret1);
            Assert.AreEqual(ret1, @"C:\");
        }

        /// <summary>
        /// Test requirement: Path reference should always have ONE
        /// backslash after ':' character in drive reference
        /// </summary>
        [TestMethod]
        public void TestFolderPathNormalization()
        {
            var ret = PathModel.NormalizePath(@"C:\t");
            var ret1 = PathModel.NormalizePath(@"C:t");

            Assert.AreEqual(ret, ret1);
            Assert.AreEqual(ret1, @"C:\t");

            ret = PathModel.NormalizePath(@"C:\temp\mytest");
            ret1 = PathModel.NormalizePath(@"C:temp\mytest");

            Assert.AreEqual(ret, ret1);
            Assert.AreEqual(ret1, @"C:\temp\mytest");
        }

        /// <summary>
        /// Requirement: Directory references should not have a backslash
        /// as last character (while drive references should have a backslash
        /// as last character).
        /// </summary>
        [TestMethod]
        public void TestDirectoryPathBackslashTrim()
        {
            var ret = PathModel.NormalizePath(@"C:\t\");
            var ret1 = PathModel.NormalizePath(@"C:t");
            var ret2 = PathModel.NormalizePath(@"C:\t");
            var ret3 = PathModel.NormalizePath(@"C:\t\");

            Assert.AreEqual(ret, ret1);
            Assert.AreEqual(ret1, ret2);
            Assert.AreEqual(ret2, ret3);
            Assert.AreEqual(ret3, @"C:\t");

            ret = PathModel.NormalizePath(@"C:\temp\mytest\");
            ret1 = PathModel.NormalizePath(@"C:temp\mytest");
            ret2 = PathModel.NormalizePath(@"C:\temp\mytest");
            ret3 = PathModel.NormalizePath(@"C:\temp\mytest\");

            Assert.AreEqual(ret, ret1);
            Assert.AreEqual(ret1, @"C:\temp\mytest");
        }

        /// <summary>
        /// Requirement: A Directory path Split should result into a normalized root
        /// (drive) reference with each path item in each array bucket.
        /// 
        /// The resulting split items are case sensitive equal to the input path.
        /// </summary>
        [TestMethod]
        public void TestSplitPathNormalization()
        {
            var ret = PathModel.GetDirectories(@"c:temp\test\");

            Assert.AreNotEqual(ret, null);
            Assert.AreEqual(ret.Length, 3);

            Assert.AreEqual(ret[0], "c:\\");
            Assert.AreEqual(ret[1], "temp");
            Assert.AreEqual(ret[2], "test");
        }
    }
}
