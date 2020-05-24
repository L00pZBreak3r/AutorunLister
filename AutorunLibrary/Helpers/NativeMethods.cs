using System;
using System.Runtime.InteropServices;

namespace AutorunLibrary.Helpers
{
  internal static class NativeMethods
  {
    internal enum UnionChoice { File = 1, Catalog, Blob, Signer, Cert };
    internal enum UiChoice { All = 1, NoUI, NoBad, NoGood };
    internal enum RevocationCheckFlags { None, WholeChain };
    internal enum StateAction { Ignore, Verify, Close, AutoCache, AutoCacheFlush };
    internal enum TrustProviderFlags
    {
      UseIE4Trust = 1,
      NoIE4Chain = 2,
      NoPolicyUsage = 4,
      RevocationCheckNone = 16,
      RevocationCheckEndCert = 32,
      RevocationCheckChain = 64,
      RecovationCheckChainExcludeRoot = 128,
      Safer = 256,
      HashOnly = 512,
      UseDefaultOSVerCheck = 1024,
      LifetimeSigning = 2048,
      CacheOnlyUrlRetrieval = 4096,
      DisableMD2MD4 = 8192
    };
    internal enum UIContext { Execute = 0, Install };
    internal enum WssFlags { VerifySpecific = 1, GetSecondarySigCount };

    internal enum TrustE : uint { Success = 0, FileError = 0x80092003, BadMsg = 0x8009200d, SecuritySettings = 0x80092026, ASN1BadTag = 0x8009310b, CounterSigner = 0x80096003, BadDigest = 0x80096010, Unknown = 0x800b0000, ProviderUnknown, ActionUnknown, SubjectFormUnknown, SubjectNotTrusted, NoSignature = 0x800b0100, CertExpired = 0x800b0101, CertUntrustedRoot = 0x800b0109, CertChaining = 0x800b010a, CertRevoked = 0x800b010c, CertWrongUsage = 0x800b0110, ExplicitDistrust = 0x800b0111 };

#if USE_WINTRUST_SIGNATURE_SETTINGS
    [StructLayout(LayoutKind.Sequential)]
    internal struct WINTRUST_SIGNATURE_SETTINGS
    {
      int cbStruct;
      int dwIndex;
      WssFlags dwFlags;
      int cSecondarySigs;
      int dwVerifiedSigIndex;
      IntPtr pCryptoPolicy;
    }
#endif

    internal const int ERROR_SUCCESS = 0x0;


    #region Win32 API

    [DllImport("Wintrust.dll")]
    internal static extern uint WinVerifyTrust(IntPtr hWnd, IntPtr pgActionID, IntPtr pWinTrustData);

    [DllImport("kernel32", SetLastError = true)]
    internal static extern int CloseHandle(IntPtr hObject);

    #endregion Win32 API

  }

}
