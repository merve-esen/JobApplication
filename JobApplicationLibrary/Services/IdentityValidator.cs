﻿namespace JobApplicationLibrary.Services
{
    public class IdentityValidator : IIdentityValidator
    {
        public ICountryDataProvider CountryDataProvider => throw new NotImplementedException();

        public bool CheckConnectionToRemoteServer()
        {
            throw new NotImplementedException();
        }

        public bool IsValid(string identityNumber)
        {
            return true;
        }
    }
}
