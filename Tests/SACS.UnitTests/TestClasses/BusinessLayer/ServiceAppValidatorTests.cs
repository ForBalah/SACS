using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SACS.BusinessLayer.BusinessLogic.Validation;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestClass]
    public class ServiceAppValidatorTests
    {
        private ServiceAppValidator _validator;
        [TestInitialize]
        public void Initialize()
        {
            _validator = new ServiceAppValidator();
        }

        [TestMethod]
        public void ValidateAppName_InvalidEmptyString()
        {
            bool isValid = _validator.ValidateAppName("");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAppName_InvalidWhitespaceString()
        {
            bool isValid = _validator.ValidateAppName("     ");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAppName_InvalidNullString()
        {
            bool isValid = _validator.ValidateAppName(null);
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAppName_InvalidSymbolString()
        {
            bool isValid = _validator.ValidateAppName("\":");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAppName_InvalidSpaceAtStart()
        {
            bool isValid = _validator.ValidateAppName(" (1)_TestApp");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAppName_InvalidSpaceString()
        {   
            bool isValid = _validator.ValidateAppName("TestApp  2");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateStartupType_InvalidSelection()
        {
            bool isValid = _validator.ValidateStartupType(Common.Enums.StartupType.NotSet);
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateEnvironmentName_InvalidSpaceString()
        {
            bool isValid = _validator.ValidateEnvironmentName("Environment 2");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAppPath_InvalidRelativePath()
        {
            bool isValid = _validator.ValidateAppPath("RelativePath\\SubPath");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAppPath_ValidFullPath()
        {
            bool isValid = _validator.ValidateAppPath("C:\\Full Path\\Full Sub Path With Space");
            Assert.IsTrue(isValid);
            Assert.IsFalse(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAssemblyName_ValidSingleName()
        {
            bool isValid = _validator.ValidateAssemblyName("Assembly_Name");
            Assert.IsTrue(isValid);
            Assert.IsFalse(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAssemblyName_ValidMultiPartNameName()
        {
            bool isValid = _validator.ValidateAssemblyName("_AssemblyName.Part1.Part_2");
            Assert.IsTrue(isValid);
            Assert.IsFalse(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAssemblyName_InvalidSingleName()
        {
            bool isValid = _validator.ValidateAssemblyName("134AssemblyName");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateAssemblyName_InvalidMultiNameWithDots()
        {
            bool isValid = _validator.ValidateAssemblyName("AssemblyName..Part2.");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateEntryFileName_InvalidFullPath()
        {
            bool isValid = _validator.ValidateEntryFileName("C:\\Hello.exe");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateEntryFileName_InvalidMissingExtension()
        {
            bool isValid = _validator.ValidateEntryFileName("123asdf");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [TestMethod]
        public void ValidateEntryFileName_ValidName()
        {
            bool isValid = _validator.ValidateEntryFileName("~SACS.Test App (3_3).exe");
            Assert.IsTrue(isValid);
            Assert.IsFalse(_validator.ErrorMessages.Any());
        }
    }
}
