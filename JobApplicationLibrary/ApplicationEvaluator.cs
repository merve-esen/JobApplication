using JobApplicationLibrary.Models;

namespace JobApplicationLibrary
{
    public class ApplicationEvaluator
    {
        private const int minAge = 18;
        private const int autoAcceptedYearsOfExperience = 15;
        private List<string> techStackList = new() { "C#", "RabitMQ", "Microservice", "Visual Studio" };

        public ApplicationResult Evaulate(JobApplication form)
        {
            if (form.Applicant.Age < minAge)
                return ApplicationResult.AutoRejected;

            var similarityRate = GetTechStackSimilarityRate(form.TechStackList);
            if(similarityRate < 25)
                return ApplicationResult.AutoRejected;

            if (similarityRate > 75 && form.YearsOfExperience >= autoAcceptedYearsOfExperience)
                return ApplicationResult.AutoAccepted;

            return ApplicationResult.AutoAccepted;
        }

        private int GetTechStackSimilarityRate(List<string> techStacks) {
            var matchedCount = techStacks.Where(i => techStackList.Contains(i, StringComparer.OrdinalIgnoreCase)).Count();
            return (int)((double)matchedCount / techStackList.Count) * 100;
        }
    }

    public enum ApplicationResult
    {
        AutoRejected,
        TransferredToHR,
        TransferredToLead,
        TransferredToCTO,
        AutoAccepted
    }
}
