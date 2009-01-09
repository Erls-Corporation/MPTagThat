using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MPTagThat.Core;

namespace MPTagThat.CaseConversion
{
  public partial class CaseConversion : Form
  {
    #region Variables
    private Main _main;
    private ILocalisation localisation = ServiceScope.Get<ILocalisation>();
    private ILogger log = ServiceScope.Get<ILogger>();

    private TextInfo textinfo = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;

    private string strExcep;
    #endregion

    #region ctor
    public CaseConversion(Main main)
    {
      _main = main;
      InitForm(false);
    }

    public CaseConversion(Main main, bool batchMode)
    {
      _main = main;
      InitForm(batchMode);
    }
    #endregion

    #region Methods
    /// <summary>
    /// The form is used in Batch Mode, when saving tags
    /// </summary>
    /// <param name="batchMode"></param>
    private void InitForm(bool batchMode)
    {
      InitializeComponent();

      if (!batchMode)
      {
        this.BackColor = ServiceScope.Get<IThemeManager>().CurrentTheme.BackColor;
        ServiceScope.Get<IThemeManager>().NotifyThemeChange();

        LocaliseScreen();
      }

      // Bind the List with the Exceptions to the list box
      listBoxExceptions.DataSource = Options.ConversionSettings.CaseConvExceptions;

      // Load the Settings
      checkBoxConvertFileName.Checked = Options.ConversionSettings.ConvertFileName;
      checkBoxConvertTags.Checked = Options.ConversionSettings.ConvertTags;
      checkBoxArtist.Checked = Options.ConversionSettings.ConvertArtist;
      checkBoxAlbumArtist.Checked = Options.ConversionSettings.ConvertAlbumArtist;
      checkBoxAlbum.Checked = Options.ConversionSettings.ConvertAlbum;
      checkBoxTitle.Checked = Options.ConversionSettings.ConvertTitle;
      checkBoxComment.Checked = Options.ConversionSettings.ConvertComment;
      radioButtonAllLowerCase.Checked = Options.ConversionSettings.ConvertAllLower;
      radioButtonAllUpperCase.Checked = Options.ConversionSettings.ConvertAllUpper;
      radioButtonFirstLetterUpperCase.Checked = Options.ConversionSettings.ConvertFirstUpper;
      radioButtonAllFirstLetterUpperCase.Checked = Options.ConversionSettings.ConvertAllFirstUpper;
      checkBoxReplace20bySpace.Checked = Options.ConversionSettings.Replace20BySpace;
      checkBoxReplaceSpaceby20.Checked = Options.ConversionSettings.ReplaceSpaceBy20;
      checkBoxReplaceSpaceByUnderscore.Checked = Options.ConversionSettings.ReplaceSpaceByUnderscore;
      checkBoxReplaceUnderscoreBySpace.Checked = Options.ConversionSettings.ReplaceUnderscoreBySpace;
      checkBoxAlwaysUpperCaseFirstLetter.Checked = Options.ConversionSettings.ConvertAllWaysFirstUpper;
    }

    #region Localisation
    /// <summary>
    /// Localise the Screen
    /// </summary>
    private void LocaliseScreen()
    {
      Util.EnterMethod(Util.GetCallingMethod());
      this.Text = localisation.ToString("CaseConversion", "Header");
      Util.LeaveMethod(Util.GetCallingMethod());
    }
    #endregion

    /// <summary>
    /// Do Case Conversion for the given track.
    /// Called internally by the Convert button and by the Save clause, if set in Preferences
    /// </summary>
    /// <param name="track"></param>
    public void CaseConvert(TrackData track, int rowIndex)
    {
      bool bErrors = false;
      // Convert the Filename
      if (checkBoxConvertFileName.Checked)
      {
        string fileName = ConvertCase(System.IO.Path.GetFileNameWithoutExtension(track.FileName));

        // Now check the length of the filename
        if (fileName.Length > 255)
        {
          log.Debug("Filename too long: {0}", fileName);
          _main.TracksGridView.View.Rows[rowIndex].Cells[1].Value = localisation.ToString("tag2filename", "NameTooLong");
          _main.TracksGridView.AddErrorMessage(track.File.Name, String.Format("{0}: {1}", localisation.ToString("tag2filename", "NameTooLong"), fileName));
          bErrors = true;
        }

        // Check, if we would generate duplicate file names
        foreach (DataGridViewRow file in _main.TracksGridView.View.Rows)
        {
          // Don't compare the file with itself
          if (rowIndex == file.Index)
            continue;

          TrackData filedata = _main.TracksGridView.TrackList[file.Index];
          if (filedata.FileName.ToLowerInvariant() == fileName.ToLowerInvariant())
          {
            log.Debug("New Filename already exists: {0}", fileName);
            _main.TracksGridView.View.Rows[rowIndex].Cells[1].Value = localisation.ToString("tag2filename", "FileExists");
            _main.TracksGridView.AddErrorMessage(_main.TracksGridView.TrackList[rowIndex].File.Name, String.Format("{0}: {1}", localisation.ToString("tag2filename", "FileExists"), fileName));
            bErrors = true;
            break;
          }
        }
        if (!bErrors)
        {
          // Now that we have a correct Filename and no duplicates accept the changes
          track.FileName = string.Format("{0}{1}", fileName, System.IO.Path.GetExtension(track.FileName));
          track.Changed = true;
          _main.TracksGridView.Changed = true;
          _main.TracksGridView.SetBackgroundColorChanged(rowIndex);
        }
      }

      // Convert the Tags
      if (checkBoxConvertTags.Checked)
      {
        string strConv = "";
        bool bChanged = false;
        if (checkBoxArtist.Checked)
        {
          strConv = track.Artist;
          bChanged = (strConv = ConvertCase(strConv)) != track.Artist ? true : false || bChanged;
          if (bChanged)
            track.Artist = strConv;
        }
        if (checkBoxAlbumArtist.Checked)
        {
          strConv = track.AlbumArtist;
          bChanged = (strConv = ConvertCase(strConv)) != track.AlbumArtist ? true : false || bChanged;
          if (bChanged)
            track.AlbumArtist = strConv;
        }
        if (checkBoxAlbum.Checked)
        {
          strConv = track.Album;
          bChanged = (strConv = ConvertCase(strConv)) != track.Album ? true : false || bChanged;
          if (bChanged)
            track.Album = strConv;
        }
        if (checkBoxTitle.Checked)
        {
          strConv = track.Title;
          bChanged = (strConv = ConvertCase(strConv)) != track.Title ? true : false || bChanged;
          if (bChanged)
            track.Title = strConv;
        }
        if (checkBoxComment.Checked)
        {
          strConv = track.Comment;
          bChanged = (strConv = ConvertCase(strConv)) != track.Comment ? true : false || bChanged;
          if (bChanged)
            track.Comment = strConv;
        }

        if (bChanged)
        {
          track.Changed = true;
          _main.TracksGridView.Changed = true;
          _main.TracksGridView.SetBackgroundColorChanged(rowIndex);
        }
      }
    }

    public void CaseConvertSelectedTracks()
    {
      DataGridView tracksGrid = _main.TracksGridView.View;

      foreach (DataGridViewRow row in tracksGrid.Rows)
      {
        if (!row.Selected)
          continue;

        TrackData track = _main.TracksGridView.TrackList[row.Index];
        CaseConvert(track, row.Index);
      }

      foreach (TrackData track in _main.TracksGridView.TrackList)
      {
        if (track.Changed)
          _main.TracksGridView.Changed = true;
      }

      tracksGrid.Refresh();
      tracksGrid.Parent.Refresh();
    }

    private string ConvertCase(string strText)
    {
      if (strText == null || strText == string.Empty)
        return string.Empty;

      if (checkBoxReplace20bySpace.Checked)
        strText = strText.Replace("%20", " ");

      if (checkBoxReplaceSpaceby20.Checked)
        strText = strText.Replace(" ", "%20");

      if (checkBoxReplaceUnderscoreBySpace.Checked)
        strText = strText.Replace("_", " ");

      if (checkBoxReplaceSpaceByUnderscore.Checked)
        strText = strText.Replace(" ", "_");

      if (radioButtonAllLowerCase.Checked)
        strText = strText.ToLowerInvariant();
      else if (radioButtonAllUpperCase.Checked)
        strText = strText.ToUpperInvariant();
      else if (radioButtonFirstLetterUpperCase.Checked)
      {
        // Go to Lowercase first, in case that everything is already uppercase
        strText = strText.ToLowerInvariant();
        strText = strText.Substring(0, 1).ToUpperInvariant() + strText.Substring(1);
      }
      else if (radioButtonAllFirstLetterUpperCase.Checked)
      {
        // Go to Lowercase first, in case that everything is already uppercase
        strText = strText.ToLowerInvariant();
        strText = textinfo.ToTitleCase(strText);
      }

      // Handle the Exceptions
      foreach (string excep in Options.ConversionSettings.CaseConvExceptions)
      {
        strExcep = Regex.Escape(excep);
        strText = Regex.Replace(strText, @"(\W|^)" + strExcep + @"(\W|$)", new MatchEvaluator(RegexReplaceCallback), RegexOptions.Singleline | RegexOptions.IgnoreCase);
      }

      if (checkBoxAlwaysUpperCaseFirstLetter.Checked)
        strText = strText.Substring(0, 1).ToUpperInvariant() + strText.Substring(1);

      return strText;
    }

    /// <summary>
    /// Callback Method for every Match of the Regex
    /// </summary>
    /// <param name="Match"></param>
    /// <returns></returns>
    private string RegexReplaceCallback(Match Match)
    {
      strExcep = strExcep.Replace(@"\\", "\x0001").Replace(@"\", "").Replace("\x0001", @"\");
      return Util.ReplaceEx(Match.Value, strExcep, strExcep);
    }
    #endregion

    #region Event Handler
    /// <summary>
    /// Do the Conversion
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonConvert_Click(object sender, EventArgs e)
    {
      CaseConvertSelectedTracks();

      // Save the settings
      Options.ConversionSettings.ConvertFileName = checkBoxConvertFileName.Checked;
      Options.ConversionSettings.ConvertTags = checkBoxConvertTags.Checked;
      Options.ConversionSettings.ConvertArtist = checkBoxArtist.Checked;
      Options.ConversionSettings.ConvertAlbumArtist = checkBoxAlbumArtist.Checked;
      Options.ConversionSettings.ConvertAlbum = checkBoxAlbum.Checked;
      Options.ConversionSettings.ConvertTitle = checkBoxTitle.Checked;
      Options.ConversionSettings.ConvertComment = checkBoxComment.Checked;
      Options.ConversionSettings.ConvertAllLower = radioButtonAllLowerCase.Checked;
      Options.ConversionSettings.ConvertAllUpper = radioButtonAllUpperCase.Checked;
      Options.ConversionSettings.ConvertFirstUpper = radioButtonFirstLetterUpperCase.Checked;
      Options.ConversionSettings.ConvertAllFirstUpper = radioButtonAllFirstLetterUpperCase.Checked;
      Options.ConversionSettings.Replace20BySpace = checkBoxReplace20bySpace.Checked;
      Options.ConversionSettings.ReplaceSpaceBy20 = checkBoxReplaceSpaceby20.Checked;
      Options.ConversionSettings.ReplaceSpaceByUnderscore = checkBoxReplaceSpaceByUnderscore.Checked;
      Options.ConversionSettings.ReplaceUnderscoreBySpace = checkBoxReplaceUnderscoreBySpace.Checked;
      Options.ConversionSettings.ConvertAllWaysFirstUpper = checkBoxAlwaysUpperCaseFirstLetter.Checked;

      this.Close();
    }

    /// <summary>
    /// Cancel Has been pressed. Close Form
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    /// <summary>
    /// Add the Exception to the List
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonAddException_Click(object sender, EventArgs e)
    {
      foreach (string exc in Options.ConversionSettings.CaseConvExceptions)
      {
        if (exc == tbException.Text.Trim())
          return;
      }
      Options.ConversionSettings.CaseConvExceptions.Add(tbException.Text.Trim());
      tbException.Text = string.Empty;

      // Refresh the Listbox
      listBoxExceptions.DataSource = null;
      listBoxExceptions.DataSource = Options.ConversionSettings.CaseConvExceptions;
      listBoxExceptions.Refresh();
    }

    /// <summary>
    /// Remove the selected Exception from the List
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void buttonRemoveException_Click(object sender, EventArgs e)
    {
      int index = listBoxExceptions.SelectedIndex;
      if (index > -1)
      {
        Options.ConversionSettings.CaseConvExceptions.RemoveAt(index);

        // Refresh the Listbox
        listBoxExceptions.DataSource = null;
        listBoxExceptions.DataSource = Options.ConversionSettings.CaseConvExceptions;
        listBoxExceptions.Refresh();
      }
    }
    #endregion
  }
}