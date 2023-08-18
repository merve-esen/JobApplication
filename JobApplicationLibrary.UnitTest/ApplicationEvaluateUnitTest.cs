using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services;
using Moq;

namespace JobApplicationLibrary.UnitTest
{
    public class ApplicationEvaluateUnitTest
    {
        // UnitOfWork_Condition_ExpectedResult
        [Test]
        public void Application_WithUnderAge_TransferredToAutoRejected()
        {
            // Arrange
            var evaluator = new ApplicationEvaluator(null);
            var form = new JobApplication()
            {
                Applicant = new Applicant
                {
                    Age = 17
                }
            };

            // Action
            var appResult = evaluator.Evaulate(form);

            // Assert
            Assert.AreEqual(ApplicationResult.AutoRejected, appResult);
        }

        [Test]
        public void Application_WithNoTechStack_TransferredToAutoRejected()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Strict);
            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKIYE");
            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(i => i.CheckConnectionToRemoteServer()).Returns(true);
            mockValidator.SetupProperty(i => i.ValidationMode);
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant { Age = 19 },
                TechStackList = new List<string>() { "" }
            };

            // Action
            var appResult = evaluator.Evaulate(form);

            // Assert
            Assert.AreEqual(ApplicationResult.AutoRejected, appResult);
        }

        [Test]
        public void Application_WithTechStackOver75P_TransferredToAutoAccepted()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Default);
            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKIYE");
            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(true);
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant { Age = 19 },
                TechStackList = new List<string>() { "C#", "RabitMQ", "Microservice", "Visual Studio" },
                YearsOfExperience = 16
            };

            // Action
            var appResult = evaluator.Evaulate(form);

            // Assert
            Assert.AreEqual(ApplicationResult.AutoAccepted, appResult);
        }

        [Test]
        public void Application_WithInvalidIdentityNumber_TransferredToHR()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>(MockBehavior.Loose);
            mockValidator.DefaultValue = DefaultValue.Mock;
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("TURKIYE");
            mockValidator.Setup(i => i.IsValid(It.IsAny<string>())).Returns(false);
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant { Age = 19 }
            };

            // Action
            var appResult = evaluator.Evaulate(form);

            // Assert
            Assert.AreEqual(ApplicationResult.TransferredToHR, appResult);
        }

        [Test]
        public void Application_WithOfficeLocation_TransferredToCTO()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("SPAIN");
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant { Age = 19 }
            };

            // Action
            var appResult = evaluator.Evaulate(form);

            // Assert
            Assert.AreEqual(ApplicationResult.TransferredToCTO, appResult);
        }

        [Test]
        public void Application_WithOver50_ValidationModeDetailed()
        {
            // Arrange
            var mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(i => i.CountryDataProvider.CountryData.Country).Returns("SPAIN");
            mockValidator.SetupProperty(i => i.ValidationMode);
            var evaluator = new ApplicationEvaluator(mockValidator.Object);
            var form = new JobApplication()
            {
                Applicant = new Applicant { Age = 51 }
            };

            // Action
            var appResult = evaluator.Evaulate(form);

            // Assert
            Assert.AreEqual(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
        }
    }
}