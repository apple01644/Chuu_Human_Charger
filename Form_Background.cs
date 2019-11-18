using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chuu__The_human_charger
{
    public partial class Form_Background : Form
    {
        Random rng = new Random();
        void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public Form_Background()
        {
            InitializeComponent();
        }

        List<String> text = new List<string>();
        string str = "";

        BufferedGraphicsContext ctx;
        BufferedGraphics bg;
        Image img;

        //Graphics g;
        Font f; 
        StringFormat format = new StringFormat();
        
        private void Form_Background_Load(object sender, EventArgs e)
        {
            this.Size = new Size(
                Screen.PrimaryScreen.Bounds.Width,
                Screen.PrimaryScreen.Bounds.Height);

            panel1.Size = this.Size;
            
            ctx = BufferedGraphicsManager.Current;
            ctx.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);

            bg = ctx.Allocate(panel1.CreateGraphics(), new Rectangle(this.Location, this.Size));
            bg.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            f = new Font("Nanum NaEuiANaeSonGeurSsi", 80, FontStyle.Bold);
            try
            {
                img = Image.FromFile("../../chuu.jpg");
            }
            catch
            {
                img = Image.FromFile("chuu.jpg");
            }

            string[] strs;
            try
            {
                strs = File.ReadAllLines("../../text.txt");
            }
            catch
            {
                strs = File.ReadAllLines("text.txt");
            }

            for (int x = 0; x < strs.Length; ++x)
            {
                text.Add(strs[x]);
            }
            Shuffle<string>(text);
            str = text[0];
            l = 0;

            format.LineAlignment = StringAlignment.Far;
            format.Alignment = StringAlignment.Center;
        }

        int k = 0;
        int l = 0;

        private void timerMain_Tick(object sender, EventArgs e)
        {
            bg.Graphics.Clear(Color.White);
            bg.Graphics.DrawImage(img, (panel1.Width - img.Width) / 2, (panel1.Height - img.Height) / 2, img.Width, img.Height);

            //Animation(0.5f);
            Animation(
                (float)Math.Pow(k / 200f - 0.5f, 3) / 0.25f + 0.5f
                );

            bg.Render(panel1.CreateGraphics());
            if (k == 0)
            {
                Thread.Sleep(5000);
            }
            ++k;
            if (k > 200)
            {
                k = 0;
                ++l;
                if (l >= text.Count)
                {
                    Shuffle<string>(text);
                    str = text[0];
                    l = 0;
                }
                else
                {
                    str = text[l];
                }
            }
        }

        void Animation(float percent)
        {
            if (percent < 0.5f) //Rising
            {
                int dy = (int)((0.5f - percent) * 2 * 100);
                int alpha = 255 - (int)(Math.Abs(0.5f - percent) * 2 * 255);
                int alpha_shadow = 120 - (int)(Math.Abs(0.5f - percent) * 2 * 120);
                bg.Graphics.DrawString(str, f, new SolidBrush(Color.FromArgb(alpha_shadow, 0, 0, 0)), panel1.Width / 2 + 4, Screen.PrimaryScreen.WorkingArea.Height + dy + 4, format);
                bg.Graphics.DrawString(str, f, new SolidBrush(Color.FromArgb(alpha, 255, 255, 255)), panel1.Width / 2, Screen.PrimaryScreen.WorkingArea.Height + dy, format);
            }
            else if (percent > 0.5f) //Falling
            {
                //int dy = (int)((percent - 0.5f) * 2 * 100);
                int alpha = 255 - (int)(Math.Abs(0.5f - percent) * 2 * 255);
                int alpha_shadow = 120 - (int)(Math.Abs(0.5f - percent) * 2 * 120);
                bg.Graphics.DrawString(str, f, new SolidBrush(Color.FromArgb(alpha_shadow, 0, 0, 0)), panel1.Width / 2 + 4, Screen.PrimaryScreen.WorkingArea.Height + 4, format);
                bg.Graphics.DrawString(str, f, new SolidBrush(Color.FromArgb(alpha, 255, 255, 255)), panel1.Width / 2, Screen.PrimaryScreen.WorkingArea.Height, format);
            }
            else //Normal
            {
                bg.Graphics.DrawString(str, f, new SolidBrush(Color.FromArgb(120, 0, 0, 0)), panel1.Width / 2 + 4, Screen.PrimaryScreen.WorkingArea.Height + 4, format);
                bg.Graphics.DrawString(str, f, new SolidBrush(Color.FromArgb(255, 255, 255, 255)), panel1.Width / 2, Screen.PrimaryScreen.WorkingArea.Height, format);
            }
        }
    }
}
