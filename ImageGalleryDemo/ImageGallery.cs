using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using C1.Win.C1Tile;
namespace ImageGalleryDemo
{
    public partial class ImageGallery : Form
    {
        DataFetcher datafetch = new DataFetcher();
        List<ImageItem> imagesList;
        int checkedItems = 0;
       

        public ImageGallery()
        {
            InitializeComponent();
        }

        private void ImageGallery_Load(object sender, EventArgs e)
        {

        }

        private void c1SplitContainer1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = _searchBox.Bounds;
            r.Inflate(3, 3);
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
        }

        private async void _search_Click(object sender, EventArgs e)
        {
            statusStrip1.Visible = true;
            imagesList = await datafetch.GetImageData(_searchBox.Text);
            AddTiles(imagesList);
            statusStrip1.Visible = false;
        }

        private void _searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void _exportImage_Click(object sender, EventArgs e)
        {
            List<Image> images = new List<Image>();
            foreach (Tile tile in _imageTileControl.Groups[0].Tiles)
            {
                if (tile.Checked)
                {
                    images.Add(tile.Image);
                }
            }
            ConvertToPdf(images);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "pdf";
            saveFile.Filter = "PDF files (*.pdf)|*.pdf*";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                ImagePdfDocument.Save(saveFile.FileName);
            }
        }

        private void _exportImage_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(_exportImage.Location.X, _exportImage.Location.Y, _exportImage.Width, _exportImage.Height);
            r.X -= 29;
            r.Y -= 3;
            r.Width--;
            r.Height--;
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawRectangle(p, r);
            e.Graphics.DrawLine(p, new Point(0, 43), new Point(this.Width, 43));
        }

        private void _imageTileControl_TileChecked(object sender, C1.Win.C1Tile.TileEventArgs e)
        {
            checkedItems++;
            _exportImage.Visible = true;
        }

        private void _imageTileControl_TileUnchecked(object sender, C1.Win.C1Tile.TileEventArgs e)
        {
            checkedItems--;
            _exportImage.Visible = checkedItems > 0;
        }

        private void _imageTileControl_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(Color.LightGray);
            e.Graphics.DrawLine(p, 0, 43, 800, 43);
        }
        private void AddTiles(List<ImageItem> imageList)
        {
            _imageTileControl.Groups[0].Tiles.Clear();
            foreach (var imageitem in imageList)
            {
                Tile tile = new Tile();
                tile.HorizontalSize = 2;
                tile.VerticalSize = 2;
                _imageTileControl.Groups[0].Tiles.Add(tile);
                Image img = Image.FromStream(new MemoryStream(imageitem.Base64));
                Template tl = new Template();
                ImageElement ie = new ImageElement();
                ie.ImageLayout = ForeImageLayout.Stretch;
                tl.Elements.Add(ie);
                tile.Template = tl;
                tile.Image = img;
            }
        }
        private void ConvertToPdf(List<Image> images)
        {
            RectangleF rect = ImagePdfDocument.PageRectangle;
            bool firstPage = true;
            foreach (var selectedimg in images)
            {
                if (!firstPage)
                {
                    ImagePdfDocument.NewPage();
                }
                firstPage = false;
                rect.Inflate(-72, -72);
                ImagePdfDocument.DrawImage(selectedimg, rect);
            }
        }
    }
}
