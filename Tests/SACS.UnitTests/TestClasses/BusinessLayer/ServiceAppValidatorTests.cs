using System;
using System.Linq;
using NUnit.Framework;
using SACS.BusinessLayer.BusinessLogic.Validation;

namespace SACS.UnitTests.TestClasses.BusinessLayer
{
    [TestFixture]
    public class ServiceAppValidatorTests
    {
        private ServiceAppValidator _validator;

        [SetUp]
        public void Initialize()
        {
            _validator = new ServiceAppValidator();
        }

        [Test]
        public void ValidateAppName_InvalidEmptyString()
        {
            bool isValid = _validator.ValidateAppName("");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateAppName_InvalidWhitespaceString()
        {
            bool isValid = _validator.ValidateAppName("     ");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateAppName_InvalidNullString()
        {
            bool isValid = _validator.ValidateAppName(null);
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateAppName_InvalidSymbolString()
        {
            bool isValid = _validator.ValidateAppName("\":");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateAppName_InvalidSpaceAtStart()
        {
            bool isValid = _validator.ValidateAppName(" (1)_TestApp");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateAppName_InvalidSpaceString()
        {
            bool isValid = _validator.ValidateAppName("TestApp  2");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateStartupType_InvalidSelection()
        {
            bool isValid = _validator.ValidateStartupType(SACS.Common.Enums.StartupType.NotSet);
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateEnvironmentName_InvalidSpaceString()
        {
            bool isValid = _validator.ValidateEnvironmentName("Environment 2");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateEntryFileName_ValidFullPath()
        {
            bool isValid = _validator.ValidateAppFilePath("C:\\fadsf asd\\Hello.exe");
            Assert.IsTrue(isValid);
            Assert.IsTrue(!_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateEntryFileName_InvalidMissingExtension()
        {
            bool isValid = _validator.ValidateAppFilePath("123asdf");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }

        [Test]
        public void ValidateEntryFileName_InvalidName()
        {
            bool isValid = _validator.ValidateAppFilePath("~SACS.Test App (3_3).exe");
            Assert.IsFalse(isValid);
            Assert.IsTrue(_validator.ErrorMessages.Any());
        }
    }
}