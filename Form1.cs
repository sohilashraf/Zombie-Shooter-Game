using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mini_Zombie
{
    class CActor 
    {
        public int x, y, pos, flagR;
        public List<Bitmap> imgs = new List<Bitmap>();
        public int iframe = 0;
    }
    class CAdvActor
    {
        public Rectangle rcDst;
        public Rectangle rcSrc;
        public int flag = 0;
        public Bitmap img;
    }
    class CBullet
    {
        public int x, y, w, h, flag;
    }
    class CNode 
    {
        public int x, y, dx = 1;
        public Bitmap img;
        public int flag = 0;
    }
    public partial class Form1 : Form
    {   
       
        Bitmap off;
        bool up, down, left, right;

        List<CAdvActor> LWorld = new List<CAdvActor>();
        List<CActor> LHero = new List<CActor>();
        List<CActor> LZombie = new List<CActor>();
        List<CNode> LDrink = new List<CNode>();
        List<CAdvActor> LHeart = new List<CAdvActor>();
        List<CAdvActor> LCH4 = new List<CAdvActor>();
        List<CBullet> LBullet = new List<CBullet>();
        List<CNode> LGas = new List<CNode>();
        List<CNode> LPop = new List<CNode>();

        List<CNode> LOver = new List<CNode>();
        List<CAdvActor> LStart = new List<CAdvActor>();


        Timer tt = new Timer();

        int ctTick = 0;
        int ctTick2 = 1;
        int ctTick3 = 0;
        int ctTick4 = -1;
        int ctTick5 = 0;
        int ctTick6 = 1;
        int ctTick7 = 1;


        int flagDel = 0;
        int NumKill = 0;
        int isel = -1;

        bool start = false;

        int t = 0; 
        public Form1()
        {
            this.Size = new Size(1000, 560);
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.MouseDown += Form1_MouseDown;
            tt.Tick += Tt_Tick;
            tt.Start();
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            start = true;
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    up = false;
                    break;
                case Keys.Down:
                    down = false;
                    break;
                case Keys.Left:
                    left = false;
                    break;
                case Keys.Right:
                    right = false;
                    break;
            }

        }
        private void Tt_Tick(object sender, EventArgs e)
        {

            if (start == true)
            {

                if (ctTick % 80 == 0)
                {
                    CreateSingleZombie(0, this.Width - 100, 0);
                    CreateSingleZombie(0, this.Width - 100, 430);
                }
                if (ctTick2 % 100 == 0)
                {
                    CreateSingleZombie_2(0, this.Height - 100, 0);
                    CreateSingleZombie_2(0, this.Height - 100, this.Width);
                }

                if (ctTick3 == 0)
                {
                    CreateEnergyDrink();
                }
                if (ctTick4 % 50 == 0 && flagDel == 1)
                {
                    CreateEnergyDrink();
                    flagDel = 0;
                    ctTick3 = 0;
                }
                if (ctTick5 % 10 == 0)
                {
                    if (LDrink.Count > 0)
                    {
                        LDrink[0].dx *= -1;
                    }
                    if (LPop.Count > 0)
                    {
                        LPop[0].dx *= -1;
                    }
                }
                if (ctTick6 % 30 == 0)
                {
                    if (LGas.Count > 0)
                    {
                       for (int i = 0; i < LGas.Count; i++)
                       {
                            if (LGas.Count > 0)
                            {
                                CreatePop(i);
                                LGas.RemoveAt(i);
                            }
                       }
                        
                    }
                }
                if (ctTick7 % 60 == 0)
                {
                    if (LPop.Count > 0)
                    {
                        for (int i = 0; i < LPop.Count; i++)
                        {
                            LPop.RemoveAt(i);

                        }
                    }
                }

                MoveZombies();
                MoveBullet();

                IsHitBullet();

                MoveDrink();
                MovePop();

                ctTick++;
                ctTick2++;
                ctTick3++;
                ctTick4++;
                ctTick6++;
                ctTick7++;

                IsHitZombie();

                if (up)
                {
                    if (LHero[0].y > 0)
                    {
                        LHero[0].y -= 25;
                    }
                    LHero[0].iframe = 0;


                    LWorld[0].rcSrc.Y -= 1 * 20;
                    if (LWorld[0].rcSrc.Y < 0)
                    {
                        LWorld[0].rcSrc.Y = 0;
                    }
                }
                if (down)
                {
                    if (LHero[0].y < 430)
                    {
                        LHero[0].y += 25;
                    }
                    LHero[0].iframe = 3;

                    if (LWorld[0].rcSrc.Y + this.Height <= LWorld[0].img.Height)
                    {
                        LWorld[0].rcSrc.Y += 1 * 20;
                    }
                }
                if (left)
                {
                    if (LHero[0].x > 0)
                    {
                        LHero[0].x -= 25;
                    }
                    LHero[0].iframe = 1;

                    LWorld[0].rcSrc.X -= 1 * 20;
                    if (LWorld[0].rcSrc.X < 0)
                    {
                        LWorld[0].rcSrc.X = 0;
                    }
                }
                if (right)
                {
                    if (LHero[0].x < 890)
                    {
                        LHero[0].x += 25;
                    }
                    LHero[0].iframe = 2;

                    if (LWorld[0].rcSrc.X + this.Width <= LWorld[0].img.Width - 5)
                    {
                        LWorld[0].rcSrc.X += 1 * 20;
                    }
                }

                if (LDrink.Count > 0)
                {
                    IsHitDrink();
                }

                IsHitPop();

                if (LHeart.Count == 0)
                {
                    LHero[0].iframe = 4;
                    tt.Stop();
                }
            }
            else
            {
                t++;
                if (t > 5)
                {
                    t = 0;
                }
            }
            DrawDubb(this.CreateGraphics());
        }
        void MoveDrink() 
        {
            for (int i = 0; i < LDrink.Count; i++)
            {
                LDrink[i].x += LDrink[i].dx;
            }
        }
        void MovePop()
        {
            for (int i = 0; i < LPop.Count; i++)
            {
                LPop[i].x += LPop[i].dx *5;
            }
        }
        void IsHitPop() 
        {
            for (int i = 0; i < LZombie.Count; i++)
            {
                for (int k = 0; k < LPop.Count; k++)
                {
                    if (LZombie[i].x >= LPop[k].x - LWorld[0].rcSrc.X && LZombie[i].x <= LPop[k].x+ LPop[k].img.Width - LWorld[0].rcSrc.X &&
                        LZombie[i].y >= LPop[k].y - LWorld[0].rcSrc.Y && LZombie[i].y <= LPop[k].y + LPop[k].img.Height - LWorld[0].rcSrc.Y)
                    {
                        if (LPop.Count > 0)
                        {
                            LZombie.RemoveAt(i);
                        }
                    }

                }
            }
        }
        void IsHitZombie() 
        {
            for (int i = 0; i < LZombie.Count; i++)
            {
                if (LHero[0].x >= LZombie[i].x &&
                    LHero[0].x <= LZombie[i].x + LZombie[i].imgs[0].Width &&
                    LHero[0].y >= LZombie[i].y &&
                    LHero[0].y <= LZombie[i].y + LZombie[i].imgs[0].Height)
                {
                    if (LHeart.Count >= 1)
                    {
                        LHeart.RemoveAt(LHeart.Count - 1);
                    }
                    if (LHeart.Count == 0)
                    {
                        LHero[0].iframe = 4;
                        CreateGameOver();
                        tt.Stop();
                    }
                    break;
                }
            }  
        }
        void IsHitDrink() 
        {
            if (LHero[0].x >= LDrink[0].x - LWorld[0].rcSrc.X && LHero[0].x <= LDrink[0].x + LDrink[0].img.Width - LWorld[0].rcSrc.X &&
                LHero[0].y >= 400 || LHero[0].x  <= LDrink[0].x - 50 && LHero[0].y >= 440) 
            {

                LDrink.RemoveAt(LDrink.Count - 1);
                flagDel = 1;

                if (LHeart.Count > 0)
                {
                    int x = LHeart[LHeart.Count - 1].rcDst.X + 15;
                    for (int i = 0; i < 3; i++)
                    {
                        CAdvActor pnn = new CAdvActor();
                        pnn.img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\heart.png");
                        pnn.rcDst.X = x;
                        pnn.rcDst.Y = LHeart[0].rcDst.Y;
                        pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                        pnn.rcDst = new Rectangle(pnn.rcDst.X, pnn.rcDst.Y, 15, 15);
                        LHeart.Add(pnn);
                        x += 15;
                    }
                }

            }
        }
        void MoveZombies() 
        {
            for (int i = 0; i < LZombie.Count; i++)
            {
                Random RZ = new Random();
                int z = RZ.Next(10, 18);

                if (LHero[0].y + z > LZombie[i].y )
                {
                    LZombie[i].y+=5;
                    LZombie[i].iframe = 3;
                }
                if (LHero[0].y - z < LZombie[i].y )
                {
                    LZombie[i].y-=5;
                    LZombie[i].iframe = 0;
                }
                if (LHero[0].x + z > LZombie[i].x )
                {
                    LZombie[i].x+=5;
                    if (LHero[0].y < LZombie[i].y)
                    {
                        LZombie[i].iframe = 2;
                    }
                }
                if (LHero[0].x - z < LZombie[i].x )
                {
                    LZombie[i].x-=5;
                    if (LHero[0].y < LZombie[i].y)
                    {
                        LZombie[i].iframe = 1;
                    }
                }
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    up = true;
                    break;
                case Keys.Down:
                    down = true;
                    break;
                case Keys.Left:
                    left = true;
                    break;
                case Keys.Right:
                    right = true;
                    break;
                case Keys.Space:
                    CreateBullet();
                    break;
                case Keys.Enter:
                    if (LCH4.Count > 0)
                    {
                        isel = LGas.Count;
                        CreateGas();
                        LCH4.RemoveAt(LCH4.Count - 1);
                    }
                    break;
            }
        }
        void MoveBullet() 
        {
            for (int i = 0; i < LBullet.Count; i++)
            {
                if (LBullet[i].flag == 0)
                {
                    LBullet[i].y -= 80;
                }
                if (LBullet[i].flag == 1)
                {
                    LBullet[i].x -= 80;
                }
                if (LBullet[i].flag == 2)
                {
                    LBullet[i].x += 80;
                }
                if (LBullet[i].flag == 3)
                {
                    LBullet[i].y += 80;
                }
            }  
        }
        void IsHitBullet() 
        {
            for(int i = 0;i < LBullet.Count; i++) 
            {
                for (int k = 0; k < LZombie.Count; k++)
                {
                    if (LBullet[i].x >= LZombie[k].x &&
                    LBullet[i].x <= LZombie[k].x + LZombie[k].imgs[0].Width &&
                    LBullet[i].y >= LZombie[k].y &&
                    LBullet[i].y <= LZombie[k].y + LZombie[k].imgs[0].Height)
                    {
                        LZombie.RemoveAt(k);
                        LBullet.RemoveAt(i);
                        NumKill++;
                        break;
                    }
                }
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            CreateMyWorld();
            CreateHero();
            CreateHeart();
            CreateCH4();
            CAdvActor pnn = new CAdvActor();
            Bitmap img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\purple.jpg");
            pnn.img = img;
            pnn.rcDst = new Rectangle(0,0,this.Width,this.Height);
            pnn.rcSrc = new Rectangle(0, 0, img.Width, img.Height);
            LStart.Add(pnn);
        }
        void CreateMyWorld()
        {
            CAdvActor pnn = new CAdvActor();
            pnn.rcDst = new Rectangle(0, 0, this.Width, this.Height);
            pnn.rcSrc = new Rectangle(300,220, this.Width, this.Height);
            pnn.img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\world.jpeg");
            LWorld.Add(pnn);
        }
        void CreateHero() 
        {
            CActor pnn = new CActor();
            pnn.x = this.Width/2;
            pnn.y = this.Height/2;
            for (int i = 1; i < 6; i++)
            {
                Bitmap img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\" + i + ".png");
                Color clr = img.GetPixel(0, 0);
                img.MakeTransparent(clr);
                pnn.imgs.Add(img);
            }
            LHero.Add(pnn);
        }
        void CreateSingleZombie(int a, int b, int c)
        {
            Random RR = new Random();
            Random RR2 = new Random();

            CActor pnn = new CActor();
            pnn.x = RR.Next(a,b);
            pnn.y =  c;
            for (int i = 1; i < 5; i++)
            {
                Bitmap img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\Z" + i + ".png");
                Color clr = img.GetPixel(0, 0);
                img.MakeTransparent(clr);
                pnn.imgs.Add(img);
            }
            LZombie.Add(pnn);
        }
        void CreateSingleZombie_2(int a, int b,int c)
        {
            Random RR = new Random();

            CActor pnn = new CActor();
            pnn.x = c;
            pnn.y = RR.Next(a, b);
            for (int i = 1; i < 5; i++)
            {
                Bitmap img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\Z" + i + ".png");
                Color clr = img.GetPixel(0, 0);
                img.MakeTransparent(clr);
                pnn.imgs.Add(img);
            }
            LZombie.Add(pnn);
        }
        void CreateEnergyDrink() 
        {
            Random RR = new Random();
            CNode pnn = new CNode();

            Bitmap img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\drink.bmp");
            Color clr = img.GetPixel(0, 0);
            img.MakeTransparent(clr);
            pnn.img = img;
            int x = 65;
            int y = LWorld[0].img.Height - 140;
            pnn.x = x;
            pnn.y = y;
            LDrink.Add(pnn);
        }
        void CreateHeart() 
        {
            int xHeart = 85;
            for (int i = 0;i< 6; i++) 
            {
                CAdvActor pnn = new CAdvActor();
                pnn.img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\heart.png");
                pnn.rcDst.X= xHeart;
                pnn.rcDst.Y = 6;
                pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                pnn.rcDst = new Rectangle(pnn.rcDst.X, pnn.rcDst.Y, 15, 15);
                LHeart.Add(pnn);
                xHeart += 15;
            }
        }
        void CreateCH4() 
        {
            int xCH4 = 860;
            for (int i = 0; i < 3; i++)
            {
                CAdvActor pnn = new CAdvActor();
                Bitmap img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\methane.bmp");
                Color clr = img.GetPixel(0, 0);
                img.MakeTransparent(clr);
                pnn.img = img;
                pnn.rcDst.X = xCH4;
                pnn.rcDst.Y = 5;
                pnn.rcSrc = new Rectangle(0, 0, pnn.img.Width, pnn.img.Height);
                pnn.rcDst = new Rectangle(pnn.rcDst.X, pnn.rcDst.Y,45,45);
                LCH4.Add(pnn);
                xCH4 += 35;
            }
        }
        void CreateBullet() 
        {
            CBullet pnn = new CBullet();
            if (LHero[0].iframe == 0)
            {
                pnn.x = LHero[0].x + LHero[0].imgs[0].Width/2;
                pnn.y = LHero[0].y ;
                pnn.flag = 0;

            }
            if (LHero[0].iframe == 1)
            {
                pnn.x = LHero[0].x  ;
                pnn.y = LHero[0].y + (LHero[0].imgs[0].Height / 2) - 20;
                pnn.flag = 1;
            }
            if (LHero[0].iframe == 2)
            {
                pnn.x = LHero[0].x + LHero[0].imgs[0].Width + 20;
                pnn.y = LHero[0].y + LHero[0].imgs[0].Height / 2 - 14;
                pnn.flag = 2;
            }
            if (LHero[0].iframe == 3)
            {
                pnn.x = LHero[0].x + (LHero[0].imgs[0].Width / 2) - 6;
                pnn.y = LHero[0].y + LHero[0].imgs[0].Height - 10 ;
                pnn.flag = 3;
            }
            pnn.w = 7;
            pnn.h = 7;
            LBullet.Add(pnn);

        }
        void CreateGas() 
        {
            CNode pnn = new CNode();
            Bitmap img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\methane.bmp");
            Color clr = img.GetPixel(0, 0);
            img.MakeTransparent(clr);
            pnn.img = img;
            pnn.x = LHero[0].x + LWorld[0].rcSrc.X;
            pnn.y = LHero[0].y + LWorld[0].rcSrc.Y;
            LGas.Add(pnn);
        }
        void CreatePop(int isel) 
        {
            CNode pnn = new CNode();
            Bitmap img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\explosion (2).png");
            Color clr = img.GetPixel(0, 0);
            img.MakeTransparent(clr);
            pnn.img = img;
            pnn.x = LGas[isel].x - 50;
            pnn.y = LGas[isel].y - 60;
            LPop.Add(pnn);
        }
        void CreateGameOver() 
        {
            CNode pnn = new CNode();
            Bitmap img = new Bitmap("C:\\Users\\sohila\\source\\repos\\Mini_Zombie\\game-over (3).bmp");
            Color clr = img.GetPixel(0, 0);
            img.MakeTransparent(clr);
            pnn.img = img;
            pnn.x = 390;
            pnn.y = 100;
            LOver.Add(pnn);
        }
        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        void DrawScene(Graphics g2)
        {
            g2.Clear(Color.Black);
            if (start == true)
            {
                
                for (int i = 0; i < LWorld.Count; i++)
                {
                    g2.DrawImage(LWorld[i].img, LWorld[i].rcDst, LWorld[i].rcSrc, GraphicsUnit.Pixel);
                }

                for (int i = 0; i < LHero.Count; i++)
                {
                    int k = LHero[i].iframe;
                    g2.DrawImage(LHero[i].imgs[k], LHero[i].x, LHero[i].y);
                }

                for (int i = 0; i < LZombie.Count; i++)
                {
                    int k = LZombie[i].iframe;
                    g2.DrawImage(LZombie[i].imgs[k], LZombie[i].x, LZombie[i].y);
                }

                for (int i = 0; i < LDrink.Count; i++)
                {
                    g2.DrawImage(LDrink[i].img, LDrink[i].x - LWorld[0].rcSrc.X, LDrink[i].y - LWorld[0].rcSrc.Y);
                }

                for (int i = 0; i < LHeart.Count; i++)
                {
                    g2.DrawImage(LHeart[i].img, LHeart[i].rcDst, LHeart[i].rcSrc, GraphicsUnit.Pixel);
                }

                for (int i = 0; i < LCH4.Count; i++)
                {
                    g2.DrawImage(LCH4[i].img, LCH4[i].rcDst, LCH4[i].rcSrc, GraphicsUnit.Pixel);
                }

                for (int i = 0; i < LBullet.Count; i++)
                {
                    SolidBrush b = new SolidBrush(Color.White);
                    g2.FillEllipse(b, LBullet[i].x, LBullet[i].y, LBullet[i].w, LBullet[i].h);
                }

                for (int i = 0; i < LGas.Count; i++)
                {
                    g2.DrawImage(LGas[i].img, LGas[i].x - LWorld[0].rcSrc.X, LGas[i].y - LWorld[0].rcSrc.Y);
                }

                for (int i = 0; i < LPop.Count; i++)
                {
                    g2.DrawImage(LPop[i].img, LPop[i].x - LWorld[0].rcSrc.X, LPop[i].y - LWorld[0].rcSrc.Y);
                }

                for (int i = 0; i < LOver.Count; i++)
                {
                    g2.DrawImage(LOver[i].img, LOver[i].x, LOver[i].y);
                }

                string str = 15 + "";
                g2.DrawString("KILLS:", new Font("system", 12), Brushes.White, 500, 3);
                g2.DrawString(NumKill + "", new Font("system", 12), Brushes.White, 555, 3);
                g2.DrawString("HEALTH:", new Font("system", 12), Brushes.White, 10, 3);
            }
            else
            {
                g2.DrawImage(LStart[0].img, LStart[0].rcDst, LStart[0].rcSrc, GraphicsUnit.Pixel);
                if (t > 0)
                {
                    g2.DrawString("Tap to start", new Font("system", 15), Brushes.Black, 450, 495);
                }
            }
        }
    }
}
