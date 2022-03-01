using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HaloBlobViewer.source.formats;

namespace HaloBlobViewer
{
    public partial class CacheSelector : Form
    {
        public CacheSelector()
        {
            InitializeComponent();
            Globals.cacheForm = this;
        }

        private void LoadCachesButton_Click(object sender, EventArgs e)
        {
            if (CachesToLoadCheckList.CheckedItems.Count == 0)
            {
                MessageBox.Show("No Tag Caches Selected!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Globals.form.tag_tree.Nodes.Clear();

                List<Int32> Indexes = new List<Int32> { };
                for (int i = 0; i < CachesToLoadCheckList.CheckedItems.Count; i++)
                {
                    object Item = CachesToLoadCheckList.CheckedItems[i];
                    Int32 Index = CachesToLoadCheckList.Items.IndexOf(Item);
                    Indexes.Add(Index);
                }
                Globals.form.LoadSomeCaches(Indexes);
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CachesToLoadCheckList_SelectedIndexChanged(object sender, EventArgs e)
        {
                Int32 NumTags = 0;
                if (CachesToLoadCheckList.CheckedItems.Count > 0)
                {
                    for (int i = 0; i < CachesToLoadCheckList.CheckedItems.Count; i++)
                    {
                        string SelItemStr = CachesToLoadCheckList.CheckedItems[i].ToString();
                        string TagNum = SelItemStr.Split(':')[1];
                        NumTags = NumTags + Convert.ToInt32(TagNum);
                    }
                }

                NumTagsText.Text = "Number of tags that will be loaded: " + NumTags;
        }
    }
}
