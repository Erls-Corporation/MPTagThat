using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.GridView
{
  public class GridViewColumns
  {
    #region Variables
    private GridViewSettings _settings;
    private GridViewColumn _filename;
    private GridViewColumn _status;
    private GridViewColumn _track;
    private GridViewColumn _artist;
    private GridViewColumn _albumartist;
    private GridViewColumn _album;
    private GridViewColumn _title;
    private GridViewColumn _year;
    private GridViewColumn _genre;
    private GridViewColumn _creationtime;
    private GridViewColumn _lastwritetime;
    private GridViewColumn _tagtype;
    private GridViewColumn _disc;
    private GridViewColumn _rating;
    private GridViewColumn _comment;
    private GridViewColumn _composer;
    private GridViewColumn _conductor;
    private GridViewColumn _numpics;
    private GridViewColumn _duration;
    private GridViewColumn _filesize;
    private GridViewColumn _bitrate;
    private GridViewColumn _bpm;
    private GridViewColumn _samplerate;
    private GridViewColumn _channels;
    private GridViewColumn _version;
    private GridViewColumn _artistSortName;
    private GridViewColumn _albumSortName;
    private GridViewColumn _commercialInformation;
    private GridViewColumn _copyright;
    private GridViewColumn _copyrightInformation;
    private GridViewColumn _encodedBy;
    private GridViewColumn _interpreter;
    private GridViewColumn _grouping;
    private GridViewColumn _lyrics;
    private GridViewColumn _mediaType;
    private GridViewColumn _officialAudioFileInformation;
    private GridViewColumn _officialArtistInformation;
    private GridViewColumn _officialAudioSourceInformation;
    private GridViewColumn _officialInternetRadioInformation;
    private GridViewColumn _officialPaymentInformation;
    private GridViewColumn _officialPublisherInformation;
    private GridViewColumn _OriginalAlbum;
    private GridViewColumn _originalFileName;
    private GridViewColumn _originalLyricsWriter;
    private GridViewColumn _originalArtist;
    private GridViewColumn _originalOwner;
    private GridViewColumn _originalRelease;
    private GridViewColumn _publisher;
    private GridViewColumn _subTitle;
    private GridViewColumn _textWriter;
    private GridViewColumn _titleSortName;
    #endregion

    #region Constructor
    public GridViewColumns()
    {
      _filename = new GridViewColumn("FileName", "text", 200, true, false, true, true);
      _status = new GridViewColumn("Status", "text", 45, true, true, false, true);
      _track = new GridViewColumn("Track", "text", 40, true, false, true, false);
      _artist = new GridViewColumn("Artist", "text", 150, true, false, true, false);
      _albumartist = new GridViewColumn("AlbumArtist", "text", 150, true, false, true, false);
      _album = new GridViewColumn("Album", "text", 150, true, false, true, false);
      _title = new GridViewColumn("Title", "text", 250, true, false, true, false);
      _year = new GridViewColumn("Year", "number", 40, true, false, true, false);
      _genre = new GridViewColumn("Genre", "text", 100, true, false, true, false);
      _creationtime = new GridViewColumn("CreationTime", "text", 100, true, true, true, false);
      _lastwritetime = new GridViewColumn("LastWriteTime", "text", 100, true, true, true, false);
      _tagtype = new GridViewColumn("TagType", "text", 100, true, true, true, false);
      _disc = new GridViewColumn("Disc", "^text", 45, true, false, true, false);
      _bpm = new GridViewColumn("BPM", "number", 40, true, false, true, false);
      _rating = new GridViewColumn("Rating", "rating", 90, true, false, true, false);
      _comment = new GridViewColumn("Comment", "text", 200, true, false, true, false);
      _composer = new GridViewColumn("Composer", "text", 150, true, false, true, false);
      _conductor = new GridViewColumn("Conductor", "text", 150, true, false, true, false);
      _numpics = new GridViewColumn("NumPics", "number", 40, true, false, true, false);
      _duration = new GridViewColumn("Duration", "text", 100, true, true, true, false);
      _filesize = new GridViewColumn("FileSize", "text", 80, true, true, true, false);
      _bitrate = new GridViewColumn("BitRate", "text", 50, true, true, true, false);
      _samplerate = new GridViewColumn("SampleRate", "text", 70, true, true, true, false);
      _channels = new GridViewColumn("Channels", "text", 40, true, true, true, false);
      _version = new GridViewColumn("Version", "text", 100, true, true, true, false);

      // Initially Hidden Columns
      _artistSortName = new GridViewColumn("ArtistSortName", "text", 100, false, false, true, false);
      _albumSortName = new GridViewColumn("AlbumSortName", "text", 100, false, false, true, false);
      _commercialInformation = new GridViewColumn("CommercialInformation", "text", 100, false, false, true, false);
      _copyright = new GridViewColumn("Copyright", "text", 100, false, false, true, false);
      _copyrightInformation = new GridViewColumn("CopyrightInformation", "text", 100, false, false, true, false);
      _encodedBy = new GridViewColumn("EncodedBy", "text", 100, false, false, true, false);
      _interpreter = new GridViewColumn("Interpreter", "text", 100, false, false, true, false);
      _grouping = new GridViewColumn("Grouping", "text", 100, false, false, true, false);
      _lyrics = new GridViewColumn("Lyrics", "text", 100, false, false, true, false);
      _mediaType = new GridViewColumn("MediaType", "text", 100, false, false, true, false);
      _officialAudioFileInformation = new GridViewColumn("OfficialAudioFileInformation", "text", 100, false, false, true, false);
      _officialArtistInformation = new GridViewColumn("OfficialArtistInformation", "text", 100, false, false, true, false);
      _officialAudioSourceInformation = new GridViewColumn("OfficialAudioSourceInformation", "text", 100, false, false, true, false);
      _officialInternetRadioInformation = new GridViewColumn("OfficialInternetRadioInformation", "text", 100, false, false, true, false);
      _officialPaymentInformation = new GridViewColumn("OfficialPaymentInformation", "text", 100, false, false, true, false);
      _officialPublisherInformation = new GridViewColumn("OfficialPublisherInformation", "text", 100, false, false, true, false);
      _OriginalAlbum = new GridViewColumn("OriginalAlbum", "text", 100, false, false, true, false);
      _originalFileName = new GridViewColumn("OriginalFileName", "text", 100, false, false, true, false);
      _originalLyricsWriter = new GridViewColumn("OriginalLyricsWriter", "text", 100, false, false, true, false);
      _originalArtist = new GridViewColumn("OriginalArtist", "text", 100, false, false, true, false);
      _originalOwner = new GridViewColumn("OriginalOwner", "text", 100, false, false, true, false);
      _originalRelease = new GridViewColumn("OriginalRelease", "text", 100, false, false, true, false);
      _publisher = new GridViewColumn("Publisher", "text", 100, false, false, true, false);
      _subTitle = new GridViewColumn("SubTitle", "text", 100, false, false, true, false);
      _textWriter = new GridViewColumn("TextWriter", "text", 100, false, false, true, false);
      _titleSortName = new GridViewColumn("TitleSortName", "text", 100, false, false, true, false);

      LoadSettings();
    }
    #endregion


    #region Properties
    public GridViewSettings Settings
    {
      get { return _settings; }
    }
    #endregion

    #region Private Methods
    private void LoadSettings()
    {
      _settings = new GridViewSettings();
      _settings.Name = "Tracks";
      ServiceScope.Get<ISettingsManager>().Load(_settings);
      if (_settings.Columns.Count == 0)
      {
        // Setup the Default Columns to display on first use of the program
        List<GridViewColumn> columnList = new List<GridViewColumn>();
        columnList = SetDefaultColumns();
        _settings.Columns.Clear();
        foreach (GridViewColumn column in columnList)
        {
          _settings.Columns.Add(column);
        }
        ServiceScope.Get<ISettingsManager>().Save(_settings);
      }
    }

    public void SaveSettings()
    {
      _settings.Name = "Tracks";
      ServiceScope.Get<ISettingsManager>().Save(_settings);
    }

    public void SaveColumnSettings(DataGridViewColumn column, int colIndex)
    {
      _settings.Columns[colIndex].Width = column.Width;
      _settings.Columns[colIndex].DisplayIndex = column.DisplayIndex;
      _settings.Columns[colIndex].Display = column.Visible;
    }


    private List<GridViewColumn> SetDefaultColumns()
    {
      List<GridViewColumn> columnList = new List<GridViewColumn>();
      columnList.Add(_filename);
      columnList.Add(_status);
      columnList.Add(_track);
      columnList.Add(_artist);
      columnList.Add(_albumartist);
      columnList.Add(_album);
      columnList.Add(_title);
      columnList.Add(_year);
      columnList.Add(_genre);
      columnList.Add(_disc);
      columnList.Add(_rating);
      columnList.Add(_bpm);
      columnList.Add(_comment);
      columnList.Add(_composer);
      columnList.Add(_conductor);
      columnList.Add(_numpics);
      // Initially hidden columns
      columnList.Add(_artistSortName);
      columnList.Add(_albumSortName);
      columnList.Add(_commercialInformation);
      columnList.Add(_copyright);
      columnList.Add(_copyrightInformation);
      columnList.Add(_encodedBy);
      columnList.Add(_interpreter);
      columnList.Add(_grouping);
      columnList.Add(_lyrics);
      columnList.Add(_mediaType);
      columnList.Add(_officialAudioFileInformation);
      columnList.Add(_officialArtistInformation);
      columnList.Add(_officialAudioSourceInformation);
      columnList.Add(_officialInternetRadioInformation);
      columnList.Add(_officialPaymentInformation);
      columnList.Add(_officialPublisherInformation);
      columnList.Add(_OriginalAlbum);
      columnList.Add(_originalFileName);
      columnList.Add(_originalLyricsWriter);
      columnList.Add(_originalArtist);
      columnList.Add(_originalOwner);
      columnList.Add(_originalRelease);
      columnList.Add(_publisher);
      columnList.Add(_subTitle);
      columnList.Add(_textWriter);
      columnList.Add(_titleSortName);
      // end of initially hidden columns
      columnList.Add(_tagtype);
      columnList.Add(_duration);
      columnList.Add(_filesize);
      columnList.Add(_bitrate);
      columnList.Add(_samplerate);
      columnList.Add(_channels);
      columnList.Add(_version);
      columnList.Add(_creationtime);
      columnList.Add(_lastwritetime);

      return columnList;
    }
    #endregion
  }
}