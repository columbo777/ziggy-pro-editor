using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using EditorResources.Components;
using ProUpgradeEditor.Common;

using Sanford.Multimedia.Midi;
using X360;
using X360.FATX;
using X360.Other;
using XPackage;
using ZipLib.SharpZipLib.Core;
using ZipLib.SharpZipLib.Zip;


namespace ProUpgradeEditor.UI
{
    public class ListViewColumnSorter : IComparer
    {
        enum USBColumns
        {
            Name = 0,

            Size,
            UpdateDate,
            CreateDate,
        }


        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            var sc = listviewX.SubItems[ColumnToSort];

            // Compare the two items
            if (ColumnToSort == (int)USBColumns.Name)
            {
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            }
            else if (ColumnToSort == (int)USBColumns.Size)
            {
                if (listviewX.Tag != null &&
                    listviewY.Tag != null)
                {
                    compareResult = ObjectCompare.Compare(
                        (listviewX.Tag as FATXFileEntry).Size,
                        (listviewY.Tag as FATXFileEntry).Size);
                }
                else
                {
                    compareResult = 0;

                }
            }
            else if (ColumnToSort == (int)USBColumns.UpdateDate)
            {
                compareResult = ObjectCompare.Compare(
                    (listviewX.SubItems[ColumnToSort].Text).ToDateTime(),
                    (listviewY.SubItems[ColumnToSort].Text).ToDateTime());
            }
            else if (ColumnToSort == (int)USBColumns.CreateDate)
            {
                compareResult = ObjectCompare.Compare(
                    (listviewX.SubItems[ColumnToSort].Text).ToDateTime(),
                    (listviewY.SubItems[ColumnToSort].Text).ToDateTime());
            }
            else
            {
                compareResult = 0;
            }

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }

    }
}