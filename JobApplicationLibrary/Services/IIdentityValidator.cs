using JobApplicationLibrary.Models;

namespace JobApplicationLibrary.Services
{
    public interface IIdentityValidator
    {
        bool IsValid(string identityNumber);
        bool CheckConnectionToRemoteServer();
        ICountryDataProvider CountryDataProvider { get; }
        ValidationMode ValidationMode { get; set; }
    }

    public enum ValidationMode
    {
        None,
        Detailed,
        Quick
    }

    public interface ICountryData
    {
        string Country { get; }
    }

    public interface ICountryDataProvider
    {
        ICountryData CountryData { get; }
    }
}