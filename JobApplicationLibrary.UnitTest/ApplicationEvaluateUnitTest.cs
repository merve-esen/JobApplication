using JobApplicationLibrary.Models;

namespace JobApplicationLibrary.UnitTest
{
    public class ApplicationEvaluateUnitTest
    {
        // UnitOfWork_Condition_ExpectedResult
        [Test]
        public void Application_WithUnderAge_TransferredToAutoRejected()
        {
            // Arrange
            var evaluator = new ApplicationEvaluator();
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
            var evaluator = new ApplicationEvaluator();
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
            var evaluator = new ApplicationEvaluator();
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
    }
}