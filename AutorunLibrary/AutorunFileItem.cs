using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;


using AutorunLibrary.Helpers;

namespace AutorunLibrary
{
  public class AutorunFileItem : IDisposable
  {
    public enum AutorunStorageType
    {
      Registry, StartMenu, Scheduler
    }

    public enum AutorunUserType
    {
      Common, User
    }

    private const string CERTIFICATE_ORG_TAG = "O=";
    private const string CERTIFICATE_LOCATION_TAG = "L=";

    public readonly string FileName;
    public readonly string FilePath;
    public readonly string Parameters;
    public readonly string Company;
    public readonly int DigitalSignature;
    public readonly string DigitalSignatureVerificationResult;
    public readonly Icon FileIconImage;
    public readonly AutorunStorageType StorageType;
    public readonly AutorunUserType UserType;
    public readonly string Description;
    public readonly string CertificateSubject;
    public readonly string FullDescription;
    public readonly bool IsActive;

    public int IconIndex;

    public AutorunFileItem(AutorunUserType userType, AutorunStorageType storageType, string path, string parameters = null, string description = null, bool isActive = true)
    {
      string name = "";
      string company = "";
      int signature = 0;
      string verificationResult = "";
      string certSubject = "";
      if (string.IsNullOrWhiteSpace(parameters))
        parameters = "";
      if (string.IsNullOrWhiteSpace(description))
        description = "";
      if (string.IsNullOrWhiteSpace(path))
        path = "";
      else
      {
        if (path[0] == '"')
          path = path.Substring(1, path.Length - 1);
        if ((path.Length > 0) && (path[path.Length - 1] == '"'))
          path = path.Substring(0, path.Length - 1);
        path = Environment.ExpandEnvironmentVariables(path);
        if (File.Exists(path))
        {
          var fi = new FileInfo(path);
          name = fi.Name;

          var fvi = FileVersionInfo.GetVersionInfo(path);
          company = fvi.CompanyName;

          DigitalSignatureVerifier.WinVerifyTrustResult sigcheck = DigitalSignatureVerifier.WinVerifyTrust(path);
          if (sigcheck != DigitalSignatureVerifier.WinVerifyTrustResult.NoSignature)
          {
            signature = (sigcheck == DigitalSignatureVerifier.WinVerifyTrustResult.Success) ? 1 : 2;
            var cert = X509Certificate.CreateFromSignedFile(path);
            certSubject = cert.Subject;
            int oind = certSubject.IndexOf(", " + CERTIFICATE_ORG_TAG);
            if (oind > 0)
            {
              oind += 4;
              int lind = certSubject.IndexOf(", " + CERTIFICATE_LOCATION_TAG, oind);
              if (lind < 0)
                lind = certSubject.Length;
              company = certSubject.Substring(oind, lind - oind);
              if ((company.Length > 0) && (company[0] == '"'))
                company = company.Substring(1, company.Length - 1);
              if ((company.Length > 0) && (company[company.Length - 1] == '"'))
                company = company.Substring(0, company.Length - 1);
            }
          }
          verificationResult = DigitalSignatureVerifier.WinVerifyTrustResultToString(sigcheck);

          FileIconImage = Icon.ExtractAssociatedIcon(path);
        }
      }

      FileName = name;
      IconIndex = -1;
      FilePath = path;
      Parameters = parameters;
      Company = company;
      DigitalSignature = signature;
      DigitalSignatureVerificationResult = verificationResult;
      StorageType = storageType;
      UserType = userType;
      Description = description;
      CertificateSubject = certSubject;
      IsActive = isActive;
      string autorunLevel = (UserType == AutorunUserType.Common) ? "All users" : "Current user";
      FullDescription = (string.IsNullOrWhiteSpace(FilePath)) ? "" : $"Name: {Description}\nCommand line: \"{FilePath}\" {Parameters}\nCompany: {Company}\nDigital signature: {DigitalSignatureVerificationResult}\nCertificate subject: {CertificateSubject}\nAutorun type: {StorageType.ToString()}\nAutorun level: {autorunLevel}\nActive: {IsActive}";
    }

    #region ToString

    public override string ToString()
    {
      return (string.IsNullOrWhiteSpace(Description)) ? FileName : Description;
    }

    #endregion

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: dispose managed state (managed objects).
          FileIconImage?.Dispose();
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~AutorunFileItem() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }
    #endregion
  }
}
