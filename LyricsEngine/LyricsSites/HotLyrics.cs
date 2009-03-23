using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Timers;

namespace LyricsEngine.LyricSites
{
    class HotLyrics
    {
        string lyric = "";
        private bool complete;
        System.Timers.Timer timer;
        int timeLimit;

        public string Lyric
        {
            get { return lyric; }
        }

        public HotLyrics(string artist, string title, ManualResetEvent m_EventStop_SiteSearches, int timeLimit)
        {
            this.timeLimit = timeLimit;
            timer = new System.Timers.Timer();

            if (LyricDiagnostics.TraceSource != null) LyricDiagnostics.TraceSource.TraceEvent(TraceEventType.Information, 0, LyricDiagnostics.ElapsedTimeString() + "SeekLyrics(" + artist + ", " + title + ")");

            artist = LyricUtil.RemoveFeatComment(artist);
            artist = LyricUtil.TrimForParenthesis(artist);
            artist = LyricUtil.CapatalizeString(artist);

            artist = artist.Replace(" ", "_");
            artist = artist.Replace(",", "_");
            artist = artist.Replace(".", "_");
            artist = artist.Replace("'", "_");
            artist = artist.Replace("(", "%28");
            artist = artist.Replace(")", "%29");
            artist = artist.Replace(",", "");
            artist = artist.Replace("#", "");
            artist = artist.Replace("%", "");
            artist = artist.Replace("+", "%2B");
            artist = artist.Replace("=", "%3D");
            artist = artist.Replace("-", "_");

            // French letters
            artist = artist.Replace("�", "%E9");

            title = LyricUtil.RemoveFeatComment(title);
            title = LyricUtil.TrimForParenthesis(title);
            title = LyricUtil.CapatalizeString(title);

            title = title.Replace(" ", "_");
            title = title.Replace(",", "_");
            title = title.Replace(".", "_");
            title = title.Replace("'", "_");
            title = title.Replace("(", "%28");
            title = title.Replace(")", "%29");
            title = title.Replace(",", "_");
            title = title.Replace("#", "_");
            title = title.Replace("%", "_");
            title = title.Replace("?", "_");
            title = title.Replace("+", "%2B");
            title = title.Replace("=", "%3D");
            title = title.Replace("-", "_");
            title = title.Replace(":", "_");

            // German letters
            title = title.Replace("�", "%FC");
            title = title.Replace("�", "%FC");
            title = title.Replace("�", "%E4");
            title = title.Replace("�", "%C4");
            title = title.Replace("�", "%F6");
            title = title.Replace("�", "%D6");
            title = title.Replace("�", "%DF");

            // Danish letters
            title = title.Replace("�", "%E5");
            title = title.Replace("�", "%C5");
            title = title.Replace("�", "%E6");
            title = title.Replace("�", "%F8");

            // French letters
            title = title.Replace("�", "%E9");

            string firstLetter = "";
            if (artist.Length > 0)
                firstLetter = artist[0].ToString();

            string urlString = "http://www.hotlyrics.net/lyrics/" + firstLetter + "/" + artist + "/" + title + ".html";

            WebClient client = new WebClient();

            timer.Enabled = true;
            timer.Interval = timeLimit;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();

            Uri uri = new Uri(urlString);
            client.OpenReadCompleted += new System.Net.OpenReadCompletedEventHandler(callbackMethod);
            client.OpenReadAsync(uri);

            while (complete == false)
            {
                if (m_EventStop_SiteSearches.WaitOne(1, true))
                {
                    complete = true;
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
        }

        private void callbackMethod(object sender, OpenReadCompletedEventArgs e)
        {
            bool thisMayBeTheCorrectLyric = true;
            StringBuilder lyricTemp = new StringBuilder();

            WebClient client = (WebClient)sender;
            Stream reply = null;
            StreamReader sr = null;

            try
            {
              reply = (Stream)e.Result;
              sr = new StreamReader(reply, Encoding.Default);

              string line = "";

              while (line.IndexOf("GOOGLE END") == -1)
              {
                if (sr.EndOfStream)
                {
                  thisMayBeTheCorrectLyric = false;
                  break;
                }
                else
                {
                  line = sr.ReadLine();
                }
              }

              if (thisMayBeTheCorrectLyric)
              {
                lyricTemp = new StringBuilder();
                line = sr.ReadLine();

                while (line.IndexOf("<script type") == -1)
                {
                  lyricTemp.Append(line);
                  if (sr.EndOfStream)
                  {
                    thisMayBeTheCorrectLyric = false;
                    break;
                  }
                  else
                  {
                    line = sr.ReadLine();
                  }
                }

                lyricTemp.Replace("?s", "'s");
                lyricTemp.Replace("?t", "'t");
                lyricTemp.Replace("?m", "'m");
                lyricTemp.Replace("?l", "'l");
                lyricTemp.Replace("?v", "'v");
                lyricTemp.Replace("<br>", "\r\n");
                lyricTemp.Replace("<br />", "\r\n");
                lyricTemp.Replace("&quot;", "\"");
                lyricTemp.Replace("</p>", "");
                lyricTemp.Replace("<BR>", "");
                lyricTemp.Replace("<br/>", "\r\n");

                lyric = lyricTemp.ToString().Trim();

                // if warning message from Evil Labs' sql-server, then lyric isn't found
                if (lyric.Contains("<td"))
                {
                  lyric = "Not found";
                }
              }
            }
            catch (Exception)
            {
              lyric = "Not found";
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }

                if (reply != null)
                {
                    reply.Close();
                }

                if (timer != null)
                {
                    timer.Stop();
                    timer.Close();
                }
                complete = true;
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            timer.Close();
            timer.Dispose();

            lyric = "Not found";
            complete = true;
            Thread.CurrentThread.Abort();
        }
    }
}
