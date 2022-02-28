using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HaloBlobViewer.source.formats;

namespace HaloBlobViewer
{
    public partial class mainform : Form
    {
        public mainform form;
        string BlobIndexFile = "";

        public mainform()
        {
            InitializeComponent();
            Globals.form = this;
        }

        private void toolstripfile_Click(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tag_cache_folder_browser.ShowDialog();
            string cache_folder = tag_cache_folder_browser.SelectedPath;
            BlobIndexFile = cache_folder + "\\blob_index.dat";
            Globals.BIPath = BlobIndexFile;
            debug_output.Text = cache_folder;
            BlobIndex BI = new BlobIndex();
            BI.ParseTagCache();
            //ParseTagCache(cache_folder);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void debug_output_TextChanged(object sender, EventArgs e)
        {

        }

        private void tag_tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if((Int32)tag_tree.SelectedNode.Tag >= 0)
            {
                //TagViewToolStrip.Show();
                //TagViewTSLoadedTag.Text = tag_tree.SelectedNode.Text;
                //LoadTag(tag_tree.SelectedNode.Text, (Int32)tag_tree.SelectedNode.Tag);
            }
            else
            {
                //TagViewTSLoadedTag.Text = "No Tag Loaded";
            }
        }

        private void loaded_cache_string_TextChanged(object sender, EventArgs e)
        {

        }

        private void maintoolstrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public void LoadSomeCaches(List<Int32> indexes)
        {
            BlobIndex BI = new BlobIndex();
            TreeNode TagsRootNode = tag_tree.Nodes.Add("Tags");
            foreach (Int32 index in indexes)
            {
                List<string> Tags = BI.LoadTagCache(index);

                CacheLoadPBar.Maximum = Tags.Count;
                CacheLoadPBar.Value = 0;
                this.tag_tree.Show();
                TagsRootNode.Tag = -1;
                foreach (var path in Tags.Where(x => !string.IsNullOrEmpty(x.Trim())))
                {
                    var currentNode = TagsRootNode;
                    var pathItems = path.Split('\\');
                    foreach (var item in pathItems)
                    {
                        var tmp = currentNode.Nodes.Cast<TreeNode>().Where(x => x.Text.Equals(item));
                        currentNode = tmp.Count() > 0 ? tmp.Single() : currentNode.Nodes.Add(item);
                        if (currentNode.Text.Contains("."))
                        {
                            currentNode.ImageIndex = 2;
                            currentNode.SelectedImageIndex = 2;
                            currentNode.Tag = Tags.IndexOf(path + BI.NumTagsBeforeXCache(index));
                        }
                        else
                        {
                            currentNode.ImageIndex = 0;
                            currentNode.SelectedImageIndex = 1;
                            currentNode.Tag = -1;
                        }
                    }
                    CacheLoadPBar.Value++;
                }
                CacheLoadPBar.Maximum = 0;
                CacheLoadPBar.Value = 0;
            }
        }


        /*
         * 
         * Old Code Ignore
         * 
         * 
int GetNumTags(string header_bytes)
{
        
return;
}

string ReadFileHeader(string file)
{
BinaryReader()
}
*/

        /*
        public void ParseTagCache(string cache_path)
        {
            string blob_index = cache_path + "\\blob_index.dat";
            //check if the required files exist
            if (File.Exists(blob_index))
            {
                Thread.Sleep(50);
                this.debug_output.AppendText(Environment.NewLine + "Successfully found blob index file!");
                this.loaded_cache_string.Text = "Loaded: " + cache_path;
                //setup some stuff from header

                this.tag_tree.Show();
                //TreeNode TagsRootNode = this.tag_tree.GetNodeAt(0,0);
                TreeNode TagsRootNode = tag_tree.Nodes.Add("Tags");
                TagsRootNode.Tag = -1;
                List<string> Taglist = GetTagList(blob_index);
                foreach (var path in Taglist.Where(x => !string.IsNullOrEmpty(x.Trim())))
                {
                    var currentNode = TagsRootNode;
                    var pathItems = path.Split('\\');
                    foreach (var item in pathItems)
                    {
                        var tmp = currentNode.Nodes.Cast<TreeNode>().Where(x => x.Text.Equals(item));
                        currentNode = tmp.Count() > 0 ? tmp.Single() : currentNode.Nodes.Add(item);
                        if (currentNode.Text.Contains("."))
                        {
                            currentNode.ImageIndex = 2;
                            currentNode.SelectedImageIndex = 2;
                            currentNode.Tag = Taglist.IndexOf(path);
                        }
                        else
                        {
                            currentNode.ImageIndex = 0;
                            currentNode.SelectedImageIndex = 1;
                            currentNode.Tag = -1;
                        }
                    }
                }
                CacheLoadPBar.Value = 0;
            }
            else
            {
                //throw new FileNotFoundException();
                Thread.Sleep(50);
                this.debug_output.AppendText(Environment.NewLine + "Could not find the blob index!");
                this.loaded_cache_string.Text = "No Tag Cache Currently Loaded";
                //this.tag_tree.
                this.tag_tree.Hide();
            }
        }
        
        public List<string> GetTagList(string blobindexfile)
        {
            FileStream blobstream = new FileStream(blobindexfile, FileMode.Open);
            blobstream.Seek(96, 0);
            BinaryReader binary_reader = new BinaryReader(blobstream);

            Int32 numtags = Helpers.ReadInt32BE(binary_reader);

            CacheLoadPBar.Maximum = numtags;

            this.debug_output.AppendText(Environment.NewLine + "Number of tags: " + numtags);

            List<string> tag_list = new List<string> { };
            long next_tag_position = 124;

            for (Int32 i = 1; i <= numtags; i++)
            {

                CacheLoadPBar.Value = CacheLoadPBar.Value + 1;

                //get tag type to determine the tag name's extension the the actual tag name then combine them
                blobstream.Seek(104, SeekOrigin.Begin);
                Int32 path_table_offset = Helpers.ReadInt32BE(binary_reader)+124;
                blobstream.Seek(next_tag_position, SeekOrigin.Begin);
                string tag_group = Encoding.ASCII.GetString(binary_reader.ReadBytes(4));
                blobstream.Seek(20, SeekOrigin.Current);
                Int32 path_offset = Helpers.ReadInt32BE(binary_reader);
                next_tag_position = blobstream.Position;
                blobstream.Seek(path_table_offset + path_offset, SeekOrigin.Begin);

                //string tag_path = "0";
                
                //AppendString(blobstream, binary_reader, tag_path);

                bool stringisfinished = false;
                while (stringisfinished == false)
                {
                    Byte shitbyte = binary_reader.ReadByte();
                    short value = Convert.ToInt16(shitbyte);
                    if (value == 0)
                    {
                        stringisfinished = true;
                    }
                    blobstream.Seek(-1, SeekOrigin.Current);
                    Char shitchar = binary_reader.ReadChar();
                    shitfix.AppendText(Convert.ToString(shitchar));
                }

                //tag_path.Insert(0, Encoding.ASCII.GetString(finalbytes));
                if (stringisfinished == true)
                {

                    //Thread.Sleep(5);

                    string tag_path = shitfix.Text;
                    shitfix.Text = "";

                    string tag_name = tag_path + "." + tag_group;

                    tag_list.Add(tag_name);

                    //this.debug_output.AppendText(Environment.NewLine + tag_group);

                    this.debug_output.AppendText(Environment.NewLine + tag_name);

                    //Thread.Sleep(1);
                }
            }
            return tag_list;
        }
        
        public void LoadTag(String TagName, Int32 Index)
        {
            //Use XML plugin to build UI and guide the stream and bin reader to generate our meta viewer

            //setup the stream and reader
            FileStream Indexstream = new FileStream(BlobIndexFile, FileMode.Open);
            BinaryReader Index_reader = new BinaryReader(Indexstream);

            //get the tag cache alloc table offset
            Indexstream.Seek(54, SeekOrigin.Begin);
            SeekOrigin TagCache = (SeekOrigin)Helpers.ReadInt32BE(Index_reader)+124;

            //go to the tag's header
            Indexstream.Seek(124 + (Index * 28), SeekOrigin.Begin);

            TagViewTSLoadedTag.Text = TagName;
            string taggroup = TagName.Split('.')[1];
            String Xmlfile = AppDomain.CurrentDomain.BaseDirectory + "plugins\\Reach\\" + taggroup + ".xml";
            debug_output.AppendText(Environment.NewLine + Xmlfile);
            //foreach

            //setup stream and reader for the tag's specified cache
            //FileStream Tagstream = new FileStream();
            //BinaryReader Tag_reader = new BinaryReader(Tagstream);
        }
        */

    }
}
