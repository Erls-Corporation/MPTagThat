using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using LyricsEngine;
using LyricsEngine.LyricSites;


namespace LyricsEngine
{
  /// <summary>
  /// Class emulates long process which runs in worker thread
  /// and makes synchronous user UI operations.
  /// </summary>
  public class LyricSearch : IDisposable
  {

    #region Members

    // Reference to the lyric controller used to make syncronous user interface calls:
    LyricsController m_lc;

    // The lyric specific information to search for
    string m_artist = "";
    string m_title = "";
    string m_originalArtist = "";
    string m_originalTrack = "";

    const int TIME_LIMIT = 30 * 1000;
    const int TIME_LIMIT_FOR_SITE = 15 * 1000;

    bool lyricFound;

    int m_row;
    int m_sitesSearched;

    bool m_SearchHasEnded;
    bool m_allowAllToComplete;
    bool m_automaticUpdate;

    public static string[] LyricsSites;
    private System.Timers.Timer timer;

    // Uses to inform the specified site searches to stop searching and exit
    ManualResetEvent m_EventStop_SiteSearches;

    #endregion

    #region Functions
    internal LyricSearch(LyricsController lc, string artist, string title, string strippedArtistName, int row, bool allowAllToComplete, bool automaticUpdate)
    {
      m_lc = lc;

      m_artist = strippedArtistName;
      m_title = title;

      m_row = row;

      m_originalArtist = artist;
      m_originalTrack = title;

      m_allowAllToComplete = allowAllToComplete;
      m_automaticUpdate = automaticUpdate;

      m_EventStop_SiteSearches = new ManualResetEvent(false);

      timer = new System.Timers.Timer();
      timer.Enabled = true;
      timer.Interval = TIME_LIMIT;
      timer.Elapsed += new System.Timers.ElapsedEventHandler(StopDueToTimeLimit);
      timer.Start();
    }

    public void Dispose()
    {
      m_SearchHasEnded = true;
      m_EventStop_SiteSearches.Set();
      timer.Enabled = false;
      timer.Stop();
      timer.Close();
      timer.Dispose();
    }

    public void Run()
    {
      bool lyricWikiThreadIsRunning = false;
      bool actionextIsRunning = false;
      bool lyrDBIsRunning = false;
      bool lyrics007IsRunning = false;
      bool lyricsOnDemandIsRunning = false;
      bool seekLyricsIsRunning = false;
      bool hotLyricsIsRunning = false;

      while (m_SearchHasEnded == false)
      {

        if (System.Array.IndexOf<string>(LyricsSites, "LyricsOnDemand") != -1 && lyricsOnDemandIsRunning == false)
        {
          lyricsOnDemandIsRunning = true;
          Thread lyricsOnDemandThread;
          ThreadStart job = delegate
          {
            LyricSites.LyricsOnDemand lyricsOnDemand = new LyricSites.LyricsOnDemand(m_artist, m_title, m_EventStop_SiteSearches, TIME_LIMIT_FOR_SITE);
            if (m_allowAllToComplete)
            {
              ValidateSearchOutputForAllowAllToComplete(lyricsOnDemand.Lyric, "LyricsOnDemand");
            }
            else
            {
              ValidateSearchOutput(lyricsOnDemand.Lyric, "LyricsOnDemand");
            }
          };
          lyricsOnDemandThread = new Thread(job);
          lyricsOnDemandThread.Start();
        }

        if (System.Array.IndexOf<string>(LyricsSites, "LyricWiki") != -1 && lyricWikiThreadIsRunning == false)
        {
          lyricWikiThreadIsRunning = true;
          Thread lyricWikiThread;
          ThreadStart job = delegate
          {
            SearchLyricWiki(m_artist, m_title);
          };
          lyricWikiThread = new Thread(job);
          lyricWikiThread.Start();
        }

        if (System.Array.IndexOf<string>(LyricsSites, "Lyrics007") != -1 && lyrics007IsRunning == false)
        {
          lyrics007IsRunning = true;
          Thread lyrics007Thread;
          ThreadStart job = delegate
          {
            LyricSites.Lyrics007 lyrics007 = new LyricSites.Lyrics007(m_artist, m_title, m_EventStop_SiteSearches, TIME_LIMIT_FOR_SITE);
            if (m_allowAllToComplete)
            {
              ValidateSearchOutputForAllowAllToComplete(lyrics007.Lyric, "Lyrics007");
            }
            else
            {
              ValidateSearchOutput(lyrics007.Lyric, "Lyrics007");
            }

          };
          lyrics007Thread = new Thread(job);
          lyrics007Thread.Start();
        }

        if (System.Array.IndexOf<string>(LyricsSites, "Actionext") != -1 && actionextIsRunning == false)
        {
          actionextIsRunning = true;
          Thread actionextThread;
          ThreadStart job = delegate
          {
            LyricSites.Actionext actionext = new LyricSites.Actionext(m_artist, m_title, m_EventStop_SiteSearches, TIME_LIMIT_FOR_SITE);
            if (m_allowAllToComplete)
            {
              ValidateSearchOutputForAllowAllToComplete(actionext.Lyric, "Actionext");
            }
            else
            {
              ValidateSearchOutput(actionext.Lyric, "Actionext");
            }
          };
          actionextThread = new Thread(job);
          actionextThread.Start();
        }

        if (System.Array.IndexOf<string>(LyricsSites, "LyrDB") != -1 && lyrDBIsRunning == false)
        {
          lyrDBIsRunning = true;
          Thread lyrDBThread;
          ThreadStart job = delegate
          {
            LyricSites.LyrDB lyrDB = new LyricSites.LyrDB(m_artist, m_title, m_EventStop_SiteSearches, TIME_LIMIT_FOR_SITE);
            if (m_allowAllToComplete)
            {
              ValidateSearchOutputForAllowAllToComplete(lyrDB.Lyric, "LyrDB");
            }
            else
            {
              ValidateSearchOutput(lyrDB.Lyric, "LyrDB");
            }
          };
          lyrDBThread = new Thread(job);
          lyrDBThread.Start();
        }


        if (System.Array.IndexOf<string>(LyricsSites, "HotLyrics") != -1 && hotLyricsIsRunning == false)
        {
          hotLyricsIsRunning = true;
          Thread hotLyricsThread;
          ThreadStart job = delegate
          {
            LyricSites.HotLyrics hotLyrics = new LyricSites.HotLyrics(m_artist, m_title, m_EventStop_SiteSearches, TIME_LIMIT_FOR_SITE);

            if (m_allowAllToComplete)
            {
              ValidateSearchOutputForAllowAllToComplete(hotLyrics.Lyric, "HotLyrics");
            }
            else
            {
              ValidateSearchOutput(hotLyrics.Lyric, "HotLyrics");
            }
          };
          hotLyricsThread = new Thread(job);
          hotLyricsThread.Start();
        }

        if (System.Array.IndexOf<string>(LyricsSites, "SeekLyrics") != -1 && seekLyricsIsRunning == false)
        {
          seekLyricsIsRunning = true;
          Thread seekLyricsThread;
          ThreadStart job = delegate
          {
            LyricSites.SeekLyrics seekLyrics = new LyricSites.SeekLyrics(m_artist, m_title, m_EventStop_SiteSearches, TIME_LIMIT_FOR_SITE);

            if (m_allowAllToComplete)
            {
              ValidateSearchOutputForAllowAllToComplete(seekLyrics.Lyric, "SeekLyrics");
            }
            else
            {
              ValidateSearchOutput(seekLyrics.Lyric, "SeekLyrics");
            }
          };
          seekLyricsThread = new Thread(job);
          seekLyricsThread.Start();
        }

        Thread.Sleep(300);
      }

      Thread.CurrentThread.Abort();

    }

    #endregion


    private bool SearchLyricWiki(string artist, string title)
    {
      if (LyricDiagnostics.TraceSource != null) LyricDiagnostics.TraceSource.TraceEvent(TraceEventType.Information, 0, LyricDiagnostics.ElapsedTimeString() + "Start searchLyricWiki(" + artist + ", " + title + ")");

      string lyric = "";

      Wiki wiki = new Wiki(this);
      lyric = wiki.GetLyricAsynch(artist, title);

      bool validLyric = m_allowAllToComplete ? ValidateSearchOutputForAllowAllToComplete(lyric, "LyricWiki") : ValidateSearchOutput(lyric, "LyricWiki");
      return validLyric;
    }


    public bool ValidateSearchOutput(string lyric, string site)
    {
      if (m_SearchHasEnded == false)
      {
        Monitor.Enter(this);
        try
        {
          ++m_sitesSearched;

          // Parse the lyrics and find a suitable lyric, if any
          if (!lyric.Equals("Not found") && lyric.Length != 0)
          {
            // if the lyrics hasn't been found by another site, then we have found the lyrics to count!
            if (lyricFound == false)
            {
              lyricFound = true;
              m_lc.LyricFound(lyric, m_originalArtist, m_originalTrack, site, m_row);
              Dispose();
              return true;
            }
            // if another was quicker it is just too bad... return
            else
            {
              return false;
            }
          }
          // still other lyricsites to search
          else if (m_sitesSearched < LyricsSites.Length)
          {
            return false;
          }
          // the search got to end due to no more sites to search
          else
          {
            m_lc.LyricNotFound(m_originalArtist, m_originalTrack, "A matching lyric could not be found!", site, m_row);
            Dispose();
            return false;
          }
        }
        finally
        {
          Monitor.Exit(this);
        }
      }
      else
      {
        return false;
      }
    }

    public bool ValidateSearchOutputForAllowAllToComplete(string lyric, string site)
    {
      if (m_SearchHasEnded == false)
      {
        Monitor.Enter(this);
        try
        {
          if (!lyric.Equals("Not found") && lyric.Length != 0)
          {
            lyricFound = true;
            m_lc.LyricFound(lyric, m_originalArtist, m_originalTrack, site, m_row);
            if (++m_sitesSearched == LyricsSites.Length || m_automaticUpdate)
            {
              Dispose();
            }
            return true;
          }
          else
          {
            m_lc.LyricNotFound(m_originalArtist, m_originalTrack, "A matching lyric could not be found!", site, m_row);
            if (++m_sitesSearched == LyricsSites.Length)
            {
              Dispose();
            }
            return false;
          }
        }
        finally
        {
          Monitor.Exit(this);
        }
      }
      else
      {
        return false;
      }
    }

    private void StopDueToTimeLimit(object sender, System.EventArgs e)
    {
      if (LyricDiagnostics.TraceSource != null) LyricDiagnostics.TraceSource.TraceEvent(TraceEventType.Stop, 0, LyricDiagnostics.ElapsedTimeString() + m_artist + " - " + m_title + " was stopped due to time limit of " + TIME_LIMIT / 1000 + " seconds!");
      m_lc.LyricNotFound(m_originalArtist, m_originalTrack, "A matching lyric could not be found!", "All (timed out)", m_row);
      Dispose();
    }

    #region Properties
    public static bool Abort
    {
      set
      {
        Wiki.Abort = value;
      }
    }

    public bool SearchHasEnded
    {
      get { return m_SearchHasEnded; }
      set { m_SearchHasEnded = value; }
    }
    #endregion
  }
}
