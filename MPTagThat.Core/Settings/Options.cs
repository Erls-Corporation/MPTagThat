using System;
using System.Collections.Generic;
using System.Text;

namespace MPTagThat.Core
{
  public sealed class Options
  {
    #region private Variables
    static readonly Options instance = new Options();

    private static MPTagThatSettings _MPTagThatSettings;
    private static CaseConversionSettings _caseConversionSettings;
    private static FileNameToTagFormatSettings _fileNameToTagSettings;
    private static List<string> _fileNameToTagSettingsTemp;

    private static TagToFileNameFormatSettings _tagToFileNameSettings;
    private static List<string> _tagToFileNameSettingsTemp;

    private static OrganiseFormatSettings _organiseSettings;
    private static List<string> _organiseSettingsTemp;

    private static string _configDir;
    private static string[] availableThemes = new string[] { "ControlDefault", "Office2007Silver", "Office2007Black" };
    #endregion

    #region public Variables
    public static string HelpLocation = "http://www.team-mediaportal.com/manual/MediaPortalTools/MPTagThat";

    public static Item[] WmaStandardSampleVBR = new Item[] { new Item("16 bits, stereo, 44100 Hz", "16,2,44100", ""),
                                                       new Item("16 bits, stereo, 48000 Hz", "16,2,48000", ""),
                                                     };

    public static Item[] WmaStandardSampleCBR = new Item[] { new Item("16 bits, mono, 8000 Hz", "16,1,8000", ""),
                                                       new Item("16 bits, stereo, 8000 Hz", "16,2,8000", ""),
                                                       new Item("16 bits, mono, 11025 Hz", "16,1,11025", ""),
                                                       new Item("16 bits, mono, 16000 Hz", "16,1,16000", ""),
                                                       new Item("16 bits, stereo, 16000 Hz", "16,2,16000", ""),
                                                       new Item("16 bits, mono, 22050 Hz", "16,1,22050", ""),
                                                       new Item("16 bits, stereo, 22050 Hz", "16,2,22050", ""),
                                                       new Item("16 bits, mono, 32000 Hz", "16,1,32000", ""),
                                                       new Item("16 bits, stereo, 32000 Hz", "16,2,32000", ""),
                                                       new Item("16 bits, mono, 44100 Hz", "16,1,44100", ""),
                                                       new Item("16 bits, stereo, 44100 Hz", "16,2,44100", ""),
                                                       new Item("16 bits, stereo, 48000 Hz", "16,2,48000", ""),
                                                     };

    public static Item[] WmaLosslessSampleVBR = new Item[] { new Item("16 bits, stereo, 44100 Hz", "16,2,44100", ""),
                                                       new Item("24 bits, stereo, 44100 Hz", "24,2,44100", ""),
                                                       new Item("24 bits, stereo, 48000 Hz", "24,2,48000", ""),
                                                       new Item("24 bits, 6 Channels, 48000 Hz", "24,6,48000", ""),
                                                       new Item("24 bits, stereo, 88200 Hz", "24,2,88200", ""),
                                                       new Item("24 bits, 6 Channels, 88200 Hz", "24,6,88200", ""),
                                                       new Item("24 bits, stereo, 96000 Hz", "24,2,96000", ""),
                                                       new Item("24 bits, 6 Channels, 96000 Hz", "24,6,96000", ""),
                                                     };


    public static Item[] WmaProSampleCBR = new Item[] { new Item("16 bits, stereo, 32000 Hz", "16,2,32000", ""),
                                                       new Item("24 bits, stereo, 44100 Hz", "24,2,44100", ""),
                                                       new Item("16 bits, stereo, 44100 Hz", "16,2,44100", ""),
                                                       new Item("24 bits, 6 Channels, 44100 Hz", "24,6,44100", ""),
                                                       new Item("16 bits, 6 Channels, 44100 Hz", "16,6,44100", ""),
                                                       new Item("24 bits, stereo, 48000 Hz", "24,2,48000", ""),
                                                       new Item("16 bits, stereo, 48000 Hz", "16,2,48000", ""),
                                                       new Item("24 bits, 6 Channels, 48000 Hz", "24,6,48000", ""),
                                                       new Item("16 bits, 6 Channels, 48000 Hz", "16,6,48000", ""),
                                                       new Item("24 bits, 8 Channels, 48000 Hz", "24,8,48000", ""),
                                                       new Item("16 bits, 8 Channels, 48000 Hz", "16,8,48000", ""),
                                                       new Item("24 bits, stereo, 88200 Hz", "24,2,88200", ""),
                                                       new Item("24 bits, stereo, 96000 Hz", "24,2,96000", ""),
                                                       new Item("24 bits, 6 Channels, 96000 Hz", "24,6,96000", ""),
                                                       new Item("24 bits, 8 Channels, 96000 Hz", "24,8,96000", ""),
                                                     };

    public static Item[] WmaProSampleVBR = new Item[] { new Item("24 bits, stereo, 44100 Hz", "24,2,44100", ""),
                                                       new Item("16 bits, 6 Channels, 44100 Hz", "16,6,44100", ""),
                                                       new Item("24 bits, stereo, 48000 Hz", "24,2,48000", ""),
                                                       new Item("24 bits, 6 Channels, 48000 Hz", "24,6,48000", ""),
                                                       new Item("24 bits, stereo, 88200 Hz", "24,2,88200", ""),
                                                       new Item("24 bits, 6 Channels, 88200 Hz", "24,6,88200", ""),
                                                       new Item("24 bits, stereo, 96000 Hz", "24,2,96000", ""),
                                                       new Item("24 bits, 6 Channels, 96000 Hz", "24,6,96000", ""),
                                                     };

    public static string[] BitRatesAACPlus = new string[] { "128 kbps", "112 kbps", "96 kbps", "80 kbps",  
                                                      "64 kbps", "56 kbps", "48 kbps", "40 kbps",
                                                      "32 kbps", "28 kbps", "24 kbps", "20 kbps",
                                                      "16 kbps", "12 kbps", "10 kbps", "8 kbps"};

    // 0 = Stereo, 1 = Mono, 2 = Stereo/Mono, 3 = Parametric Stero/Stereo/Mono, 4 = Parametric Stereo/Mono
    public static int[] ModesAACPlus = new int[] { 0, 0, 0, 0,  
                                             2, 3, 3, 3,
                                             3, 3, 3, 3,
                                             3, 4, 1, 1};

    public static string[] BitRatesAACPlusHigh = new string[] { "256 kbps", "224 kbps", "192 kbps", "160 kbps",  
                                                          "128 kbps", "112 kbps", "96 kbps", "80 kbps",
                                                          "64 kbps"};

    // 0 = Stereo, 1 = Mono, 2 = Stereo/Mono, 3 = Parametric Stero/Stereo/Mono, 4 = Parametric Stereo/Mono
    public static int[] ModesPlusHigh = new int[] { 0, 0, 0, 2,  
                                              2, 2, 2, 1,
                                              1};


    public static string[] BitRatesLCAAC = new string[] { "320 kbps", "288 kbps", "256 kbps", "224 kbps",  
                                                    "192 kbps", "160 kbps", "128 kbps", "112 kbps",
                                                    "96 kbps", "80 kbps", "64 kbps", "56 kbps",
                                                    "48 kbps", "40 kbps", "32 kbps", "28 kbps",
                                                    "24 kbps", "20 kbps", "16 kbps", "12 kbps",
                                                    "10 kbps", "8 kbps" };

    // 0 = Stereo, 1 = Mono, 2 = Stereo/Mono, 3 = Parametric Stero/Stereo/Mono, 4 = Parametric Stereo/Mono
    public static int[] ModesLCAAC = new int[] { 0, 0, 0, 0,  
                                           0, 2, 2, 2,
                                           2, 2, 2, 2,
                                           2, 2, 2, 2,
                                           2, 2, 2, 1,
                                           1, 1};
    #endregion

    #region Enums
    public enum ParameterFormat
    {
      FileNameToTag = 1,
      TagToFileName = 2,
      RipFileName = 3,
      Organise = 4
    }

    public enum LamePreset
    {
      Medium = 0,
      Standard = 1,
      Extreme = 2,
      Insane = 3,
      ABR = 4
    }
    #endregion

    #region Properties
    public static Options Instance
    {
      get
      {
        return instance;
      }
    }

    public static string ConfigDir
    {
      get { return _configDir; }
    }

    public static string[] Themes
    {
      get { return availableThemes; }
    }

    public static MPTagThatSettings MainSettings
    {
      get { return _MPTagThatSettings; }
    }

    public static CaseConversionSettings ConversionSettings
    {
      get { return _caseConversionSettings; }
    }

    public static FileNameToTagFormatSettings FileNameToTagSettings
    {
      get { return _fileNameToTagSettings; }
    }

    public static List<string> FileNameToTagSettingsTemp
    {
      get { return _fileNameToTagSettingsTemp; }
    }

    public static TagToFileNameFormatSettings TagToFileNameSettings
    {
      get { return _tagToFileNameSettings; }
    }

    public static List<string> TagToFileNameSettingsTemp
    {
      get { return _tagToFileNameSettingsTemp; }
    }

    public static OrganiseFormatSettings OrganiseSettings
    {
      get { return _organiseSettings; }
    }

    public static List<string> OrganiseSettingsTemp
    {
      get { return _organiseSettingsTemp; }
    }
    #endregion

    #region Constructor
    // Singleton Constructor.
    static Options()
    { }

    public Options()
    {
      _configDir = String.Format(@"{0}\MPTagThat\Config", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

      _MPTagThatSettings = new MPTagThatSettings();
      ServiceScope.Get<ISettingsManager>().Load(_MPTagThatSettings);

      _caseConversionSettings = new CaseConversionSettings();
      ServiceScope.Get<ISettingsManager>().Load(_caseConversionSettings);
      // Set Default Values, when starting the first Time
      if (_caseConversionSettings.CaseConvExceptions.Count == 0)
      {
        _caseConversionSettings.CaseConvExceptions.Add("I");
        _caseConversionSettings.CaseConvExceptions.Add("II");
        _caseConversionSettings.CaseConvExceptions.Add("III");
        _caseConversionSettings.CaseConvExceptions.Add("IV");
        _caseConversionSettings.CaseConvExceptions.Add("V");
        _caseConversionSettings.CaseConvExceptions.Add("VI");
        _caseConversionSettings.CaseConvExceptions.Add("VII");
        _caseConversionSettings.CaseConvExceptions.Add("VIII");
        _caseConversionSettings.CaseConvExceptions.Add("IX");
        _caseConversionSettings.CaseConvExceptions.Add("X");
        _caseConversionSettings.CaseConvExceptions.Add("XI");
        _caseConversionSettings.CaseConvExceptions.Add("XII");
        _caseConversionSettings.CaseConvExceptions.Add("feat.");
        _caseConversionSettings.CaseConvExceptions.Add("vs.");
        _caseConversionSettings.CaseConvExceptions.Add("DJ");
        _caseConversionSettings.CaseConvExceptions.Add("I'm");
        _caseConversionSettings.CaseConvExceptions.Add("I'll");
        _caseConversionSettings.CaseConvExceptions.Add("I'd");
        _caseConversionSettings.CaseConvExceptions.Add("UB40");
        _caseConversionSettings.CaseConvExceptions.Add("U2");
        _caseConversionSettings.CaseConvExceptions.Add("NRG");
        _caseConversionSettings.CaseConvExceptions.Add("ZZ");
        _caseConversionSettings.CaseConvExceptions.Add("OMD");
        _caseConversionSettings.CaseConvExceptions.Add("A1");
        _caseConversionSettings.CaseConvExceptions.Add("U96");
        _caseConversionSettings.CaseConvExceptions.Add("2XLC");
        _caseConversionSettings.CaseConvExceptions.Add("ATB");
        _caseConversionSettings.CaseConvExceptions.Add("EMF");
        _caseConversionSettings.CaseConvExceptions.Add("CD");
        _caseConversionSettings.CaseConvExceptions.Add("CD1");
        _caseConversionSettings.CaseConvExceptions.Add("CD2");
        _caseConversionSettings.CaseConvExceptions.Add("MC");
        _caseConversionSettings.CaseConvExceptions.Add("USA");
        _caseConversionSettings.CaseConvExceptions.Add("UK");
        _caseConversionSettings.CaseConvExceptions.Add("TLC");
        _caseConversionSettings.CaseConvExceptions.Add("UFO");
        _caseConversionSettings.CaseConvExceptions.Add("AC");
        _caseConversionSettings.CaseConvExceptions.Add("DC");
        _caseConversionSettings.CaseConvExceptions.Add("DMX");
        _caseConversionSettings.CaseConvExceptions.Add("ABBA");
      }


      _fileNameToTagSettings = new FileNameToTagFormatSettings();
      ServiceScope.Get<ISettingsManager>().Load(_fileNameToTagSettings);

      // Set Default Values, when starting the first Time
      if (_fileNameToTagSettings.FormatValues.Count == 0)
      {
        // Add Default Values
        _fileNameToTagSettings.FormatValues.Add(@"<K> - <T>");
        _fileNameToTagSettings.FormatValues.Add(@"<A> - <T>");
        _fileNameToTagSettings.FormatValues.Add(@"<K> - <A> - <T>");
        _fileNameToTagSettings.FormatValues.Add(@"<A> - <K> - <T>");
        _fileNameToTagSettings.FormatValues.Add(@"<A>\<B>\<K> - <T>");
        _fileNameToTagSettings.FormatValues.Add(@"<A>\<B>\<A> - <K> - <T>");
        _fileNameToTagSettings.FormatValues.Add(@"<A>\<B>\<K> - <A> - <T>");
      }

      _fileNameToTagSettingsTemp = new List<string>(_fileNameToTagSettings.FormatValues);

      _tagToFileNameSettings = new TagToFileNameFormatSettings();
      ServiceScope.Get<ISettingsManager>().Load(_tagToFileNameSettings);

      // Set Default Values, when starting the first Time
      if (_tagToFileNameSettings.FormatValues.Count == 0)
      {
        // Add Default Values
        _tagToFileNameSettings.FormatValues.Add(@"<K> - <T>");
        _tagToFileNameSettings.FormatValues.Add(@"<A> - <T>");
        _tagToFileNameSettings.FormatValues.Add(@"<K> - <A> - <T>");
        _tagToFileNameSettings.FormatValues.Add(@"<A> - <K> - <T>");
      }

      _tagToFileNameSettingsTemp = new List<string>(_tagToFileNameSettings.FormatValues);

      _organiseSettings = new OrganiseFormatSettings();
      ServiceScope.Get<ISettingsManager>().Load(_organiseSettings);

      // Set Default Values, when starting the first Time
      if (_organiseSettings.FormatValues.Count == 0)
      {
        // Add Default values
        _organiseSettings.FormatValues.Add(@"<A>\<B>");
        _organiseSettings.FormatValues.Add(@"<A:1>\<A>\<B>");
      }

      _organiseSettingsTemp = new List<string>(_organiseSettings.FormatValues);
    }
    #endregion

    #region Public Methods
    public static void SaveAllSettings()
    {
      ServiceScope.Get<ISettingsManager>().Save(_MPTagThatSettings);
      ServiceScope.Get<ISettingsManager>().Save(_fileNameToTagSettings);
      ServiceScope.Get<ISettingsManager>().Save(_tagToFileNameSettings);
      ServiceScope.Get<ISettingsManager>().Save(_caseConversionSettings);
      ServiceScope.Get<ISettingsManager>().Save(_organiseSettings);
    }
    #endregion
  }
}