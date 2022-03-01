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
using HaloBlobViewer.source.helpers;

namespace HaloBlobViewer.source.formats
{
    class BlobIndex
    {

        /// <summary>
        ///     A Tag Cache Blob Index.
        /// </summary>

        /*
            Blob Index Format:
                File Header: size(0x7C)
                    Offset:     Length:     Type:       Other Info:
                    0x0         10-byte     unknown     unknown
                    --------------------------------------------------------------------------------------------------------------  Blob Index Info
                    0x10        4-byte      string      "tgin" (tag index)
                    0x14        4-byte      Int32       unkown
                    0x18        4-byte      Int32       Size of Entire Blob Index (not including the file header)
                    -------------------------------------------------------------------------------------------------------------- Tag Blob Info?
                    0x1C        4-byte      string      "mtfi" (meta file (index)?)
                    0x20        4-byte      Int32       unknown
                    0x24        4-byte      Int32       offset to "mtdp" from current offset (meta dependencies? meta data points?)
                    -------------------------------------------------------------------------------------------------------------- Cache Blob Info?
                    0x28        4-byte      string      "mtag" (meta ag?)
                    0x2C        4-byte      Int32       unknown
                    0x30        4-byte      Int32       offset to "mtdp" from current offset (meta dependencies? meta data points?)
                    -------------------------------------------------------------------------------------------------------------- Partition (Tag Index) Info
                    0x34        4-byte      string      "indx" (index)
                    0x38        4-byte      Int32       unknown
                    0x3C        4-byte      Int32       offset to 1st index from end of header
                    -------------------------------------------------------------------------------------------------------------- Basic Tag Info
                    0x40        4-byte      Int32       offset to 1st index from end of header
                    0x44        4-byte      Int32       loadid (seems to only be six)
                    0x48        4-byte      Int32       total number of tags
                    --------------------------------------------------------------------------------------------------------------
                    0x4C        4-byte      Int32       unknown (0)
                    0x50        15-byte     unkown      unknown
                    0x60        4-byte      Int32       total number of tags
                    0x64        4-byte      Int32       unkown
                    0x68        4-byte      Int32       unkown
                    0x6C        4-byte      Int32       unkown
                    0x70        4-byte      Int32       unkown (0)
                    0x74        4-byte      Int32       unkown
                    0x78        4-byte      Int32       unkown
        */

        //setup some globals that will be used and changed

        public Int32 BlobIndexSize = 0;
        public SeekOrigin MtfiMtdp = 0;
        public SeekOrigin MtagMtdp = 0;
        public SeekOrigin FirstIndex = 0;
        public Int32 NumberOfTags = 0;

        public static FileStream Blob = new FileStream(Globals.BIPath, FileMode.Open);
        public static BinaryReader BinRead = new BinaryReader(Blob);

        public Int32 GetBlobIndexSize()
        {
            if(BlobIndexSize == 0)
            {
                Blob.Seek(24, SeekOrigin.Begin);
                BlobIndexSize = endianessconverter.ReadInt32BE(BinRead);
                Blob.Seek(0, SeekOrigin.Begin);
                return BlobIndexSize;
            }
            else
            {
                return BlobIndexSize;
            }
        }
        public Int32 GetNumberOfTags()
        {
            if (NumberOfTags == 0)
            {
                Blob.Seek(72, SeekOrigin.Begin);
                NumberOfTags = endianessconverter.ReadInt32BE(BinRead);
                Blob.Seek(0, SeekOrigin.Begin);
                return NumberOfTags;
            }
            else
            {
                return NumberOfTags;
            }
        }

        public Int32 NumMetaBlobs()
        {
            //get the number of meta blobs by seeing how many indexes for each we can locate in the file

            Int32 NumBlobs = 0;

            Blob.Seek(64, SeekOrigin.Begin);
            Int32 FirstIndexOffset = endianessconverter.ReadInt32BE(BinRead) + 124;
            
            if (FirstIndexOffset - 124 >= Int32.MaxValue)
            {
                return NumBlobs;
            }
            
            Blob.Seek(FirstIndexOffset, SeekOrigin.Begin);

            string PartStr = Encoding.ASCII.GetString(BinRead.ReadBytes(4));

            bool AllIndexesFound = false;
            while(AllIndexesFound == false)
            {
                if (PartStr == "part")
                {
                    NumBlobs++;
                    Blob.Seek(4, SeekOrigin.Current);
                    Int32 NextIndexOffset = endianessconverter.ReadInt32BE(BinRead);
                    Blob.Seek(NextIndexOffset, SeekOrigin.Current);
                    PartStr = Encoding.ASCII.GetString(BinRead.ReadBytes(4));
                }
                else
                {
                    AllIndexesFound = true;
                }
            }
            Blob.Seek(0, SeekOrigin.Begin);
            return NumBlobs;
        }

        public Int32 NumTagsInCache(Int32 CacheIndex)
        {
            Blob.Seek(64, SeekOrigin.Begin);
            Int32 FirstIndexOffset = endianessconverter.ReadInt32BE(BinRead) + 124;
            Blob.Seek(FirstIndexOffset, SeekOrigin.Begin);

            Int32 NumTags = 0;

            for(int i=0; i <= CacheIndex; i++)
            {
                Blob.Seek(112, SeekOrigin.Current);
                NumTags = endianessconverter.ReadInt32BE(BinRead);
                Blob.Seek(-108, SeekOrigin.Current);
                Int32 NextIndexOffset = endianessconverter.ReadInt32BE(BinRead);
                Blob.Seek(NextIndexOffset, SeekOrigin.Current);
            }
            Blob.Seek(0, SeekOrigin.Begin);
            return NumTags;
        }

        public Int32 NumTagsBeforeXCache(int index)
        {
            Int32 NumTagsBefore = 0;
            for (int i = 0; i <= NumMetaBlobs(); i++)
            {
                if (i < index)
                {
                    NumTagsBefore = NumTagsBefore + NumTagsInCache(i);
                }
                else
                {
                    return NumTagsBefore;
                }
            }
            return NumTagsBefore;
        }

        public List<string> LoadTagCache(int index)
        {
            Int32 NumTagsBefore = 0;
            Int32 NumTagsToLoad = 0;
            Int32 NumTagsAfter = 0;

            Blob.Seek(104, SeekOrigin.Begin);
            Int32 name_table_offset = endianessconverter.ReadInt32BE(BinRead) + 124;

            List<string> tag_list = new List<string> { };

            for (int i=0;i <= NumMetaBlobs(); i++)
            {
                if(i < index)
                {
                    NumTagsBefore = NumTagsBefore + NumTagsInCache(i);
                }
                if(i == index)
                {
                    NumTagsToLoad = NumTagsToLoad + NumTagsInCache(i);
                    Globals.form.CacheLoadPBar.Maximum = NumTagsToLoad;
                }
                if(i > index)
                {
                    NumTagsAfter = NumTagsAfter + NumTagsInCache(i);
                }
            }

            TextBox shitfix = new TextBox();

            for (Int32 i = 0; i < NumTagsToLoad; i++)
            {
                //get tag type to determine the tag name's extension the the actual tag name then combine them

                Blob.Seek(124 + (NumTagsBefore * 28) + (i * 28), SeekOrigin.Begin); //go to the tag's header

                string group = Encoding.ASCII.GetString(BinRead.ReadBytes(4)); //read the tag group

                Blob.Seek(20, SeekOrigin.Current);

                Int32 TagNameOffset = endianessconverter.ReadInt32BE(BinRead);

                Blob.Seek(name_table_offset + TagNameOffset, SeekOrigin.Begin);

                bool stringisfinished = false;
                

                while (stringisfinished == false)
                {
                    Byte shitbyte = BinRead.ReadByte();
                    short value = Convert.ToInt16(shitbyte);
                    if (value == 0)
                    {
                        stringisfinished = true;
                    }
                    Blob.Seek(-1, SeekOrigin.Current);
                    Char shitchar = BinRead.ReadChar();
                    shitfix.AppendText(Convert.ToString(shitchar));
                }

                if (stringisfinished == true)
                {
                    string tag_path = shitfix.Text;
                    shitfix.Text = "";

                    string tag_name = tag_path + "." + group;

                    Globals.form.debug_output.AppendText(Environment.NewLine + tag_name);
                    
                    tag_list.Add(tag_name);

                    //dump tags to txt file
                    if (Globals.cacheForm.DumpTagscheckBox.Checked)
                        using (StreamWriter TaglistDump = File.AppendText("Tags.txt"))
                        {
                            TaglistDump.WriteLine(tag_name);
                            TaglistDump.Flush();
                            TaglistDump.Close();
                        }
                    else
                        {

                        }
                    
                }
                Globals.form.CacheLoadPBar.Value = i;

                //show the number of tags being loaded
                Globals.form.tagStatus.Text = Convert.ToString(i) + "/" + NumTagsToLoad;
                Globals.form.tagStatus.Update();
            }

            Globals.form.CacheLoadPBar.Maximum = 0;
            Globals.form.CacheLoadPBar.Value = 0;
            Globals.form.tagStatus.Text = "0";
            Blob.Seek(0, SeekOrigin.Begin);
            return tag_list;
        }

        public void ParseTagCache()
        {
            if (File.Exists(Globals.BIPath))
            {
                var NumTags = GetNumberOfTags();
                switch(NumMetaBlobs())
                {
                    case 0:
                        throw new ApplicationException("NumMetaBlobs() == 0");
                        //break;
                    case 1:
                        LoadTagCache(1);
                        break;
                    default:
                        CacheSelector CS = new CacheSelector();
                        CS.Enabled = true;
                        for(int i=0; i < NumMetaBlobs(); i++)
                        {
                            CS.CachesToLoadCheckList.Items.Add("tags_" + i + "    Number of Tags:" + NumTagsInCache(i), false);
                        }
                        CS.ShowDialog();
                        break;

                }
            }
            else
            {
                throw new FileNotFoundException("No Tag Cache Found!", "blob_index.dat");
            }
        }
    }
}
