using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using ProUpgradeEditor.Common;
using System.Threading;
using System.Globalization;
using XPackage;
using System.Diagnostics;


namespace ProUpgradeEditor.UI
{

    

    public class SongCacheList : List<SongCacheItem>
    {

        public SongListSortMode SortMode { get; set; }
        public bool SortAscending { get; set; }
        public string SongListFilter { get; set; }

        ListBox listBoxSongList;

        public SongCacheList(ListBox lb)
        {
            SortMode = SongListSortMode.SortByName;
            SortAscending = true;
            this.listBoxSongList = lb;
        }

        public SongCacheItem SelectedSong { get; set; }


        public void AddSong(SongCacheItem item)
        {
            listBoxSongList.Items.Add(item);
            this.Add(item);
        }
        public List<SongCacheItem> RemovedSongs = new List<SongCacheItem>();
        public void RemoveSong(SongCacheItem item)
        {
            if (item != null)
            {
                this.Remove(item);

                RemovedSongs.Add(item);
                listBoxSongList.Items.Remove(item);
                listBoxSongList.SelectedIndex = -1;
                this.SelectedSong = null;
            }
        }

        public void UpdateSongCacheItem(SongCacheItem item)
        {
            var lb = listBoxSongList;
            for (int x = 0; x < this.Count; x++)
            {
                var lbitem = this[x] as SongCacheItem;
                if (lbitem.CacheSongID == item.CacheSongID)
                {
                    this[x] = item;
                    break;
                }
            }

            for (int x = 0; x < lb.Items.Count; x++)
            {
                var lbitem = lb.Items[x] as SongCacheItem;
                if (lbitem.CacheSongID == item.CacheSongID)
                {
                    lb.Items[x] = item;
                    break;
                }
            }
        }


        public IEnumerable<SongCacheItem> GetBatchSongList(bool selectedSongsOnly)
        {
            if (!selectedSongsOnly)
                return this.ToList();
            else
                return MultiSelectedSongs;
        }

        public IEnumerable<SongCacheItem> MultiSelectedSongs
        {
            get { return listBoxSongList.SelectedItems.ToEnumerable<SongCacheItem>().ToList(); }
        }

        public void PopulateList()
        {
            if (this.Contains(SelectedSong) == false)
            {
                SelectedSong = null;
            }

            List<SongCacheItem> songList = new List<SongCacheItem>();
            if (SongListFilter.IsNotEmpty())
            {
                songList.AddRange(this.ToList().Where(x =>
                    x.Description.ToLower().Contains(SongListFilter.ToLower())).ToArray());
            }
            else
            {
                songList.AddRange(this.ToArray());
            }

            if (SortMode == SongListSortMode.SortByName)
            {
                songList = songList.OrderBy(x => x.Description).ToList();
            }
            else if (SortMode == SongListSortMode.SortByID)
            {
                songList = songList.OrderBy(x => x.CacheSongID).ToList();
            }
            else if (SortMode == SongListSortMode.SortByCompleted)
            {
                songList = songList.OrderBy(x => x.IsComplete).ToList();
            }

            if (SortAscending == false)
            {
                songList.Reverse();
            }


            listBoxSongList.BeginUpdate();
            listBoxSongList.Items.Clear();

            listBoxSongList.Items.AddRange(songList.ToArray());

            listBoxSongList.EndUpdate();

            if (SelectedSong != null)
            {
                var lbList = listBoxSongList.Items.ToEnumerable<SongCacheItem>().ToList();
                if (lbList.Contains(SelectedSong))
                    listBoxSongList.SelectedItem = SelectedSong;
            }
        }
    }
}