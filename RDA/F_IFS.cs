using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

namespace RDA
{
    class F_IFS
    {
        string B = "";
        private Stopwatch st0;
        public string tg="";
        List<float[,][]> M_c;

        int iXz(int x, float cx, List<string> c, List<string> d, List<string> ps)
        {
            int iz = 0;
            float miniz = 1;
            float denta = (float)Math.Sqrt(float.Parse(ps[3]));
            for (int i = 0; i < d.Count; i++)
            {
                if (d[x] == d[i])
                {
                    float ci = float.Parse(c[i]);
                    if (miniz > Math.Max(1 - Math.Abs(cx - ci) / (denta),0))
                    {
                        miniz = Math.Max(1 - Math.Abs(cx - ci) / (denta), 0);
                        iz = i;
                    }
                }
            }
                return iz;
        }


        int findZ1(int x, float cx, List<string> c, List<string> d, float dlc)
        {
            int iz = 0;
            float miniz = 1;
            for (int i = 0; i < d.Count; i++)
            {
                if (d[x] == d[i])
                {
                    float ci = float.Parse(c[i]);
                    if (miniz > Math.Max(1 - Math.Abs(cx - ci)/dlc,0))
                    {
                        miniz = Math.Max(1 - Math.Abs(cx - ci)/dlc,0);
                        iz = i;
                    }
                }
            }
            return iz;
        }

        int findZ(int x, List<float> c, List<string> d)
        {
            int iz = 0;
            float miniz = 1;
            for (int i = 0; i < d.Count; i++)
            {
                if (d[x] == d[i])
                {
                    if (miniz > 1-Math.Abs(c[x]-c[i]))
                    {
                        miniz = 1 - Math.Abs(c[x] - c[i]);
                        iz = i;
                    }
                }
            }
            return iz;
        }

        private float[] _IFRc2020(int x, int y, List<float> c, List<string> d, int n)
        {
            float[] r = new float[2];
            r[0] = 1 - Math.Abs(c[x] - c[y]);
            if (d[x] != d[y]) r[1] = 1 - r[0];
            else
            {
                int z = findZ(x, c, d);
                r[1] = Math.Max(1 - Math.Abs(c[x] - c[z]), 1 - Math.Abs(c[y] - c[z]));
            }
            return r;
        }


        private float[] _IFRc2021(int x, int y, List<float> c, float v, float consi, List<string> d,int n)
        {
            float[] r = new float[2];
            float dlc = (float)Math.Sqrt(v);
            //float a = 1-(float)Math.Pow(consi,1/(1-dlc));
            //float a = (1-dlc)/(1-consi);
            //float a =(dlc+consi)/(1+dlc);
            //float a = (1-consi+dlc)/(consi+dlc);
            float a = 1;
            if (dlc == 0) a = 1; else a = consi / dlc;
            float cxy = Math.Abs(c[x] - c[y]);

            if (v == 1)
                if (c[x] == c[y])
                {
                    r[0] =1;
                    r[1] =0;
                }
                else
                {
                    r[1] = 1;
                    r[0] = 0;
                }
            else
            {
                r[0] = 1 - cxy;
                r[1] = (1 - r[0]) / (1 + a * r[0]);
            }
                      
            return r;
        }


        //tinh phuong sai
        float _PS(List<float> c)
        {
            float _ps = 0;
            float _s = 0;
            for (int i = 0; i <c.Count; i++)
            {
                _s = _s+(float)Math.Pow(c[i]-c.Average(),2);
            }
            _ps = _s / (c.Count-1);
            return _ps;
        }


        public float[,][] Matrixc2021(List<float> c, float v, float consi, List<string> d, int U)
        {
            float[,][] M = new float[U,U][];
            for (int i = 0; i < U; i++)
            {
                for (int j = 0; j < U; j++)
                {
                    M[i, j] = _IFRc2021(i, j, c, v,consi,d,U);

                }
            }
            return M;
        }

        public float[,][] Matrixc2020(List<float> c, List<string> d, int U)
        {
            float[,][] M = new float[U, U][];
            for (int i = 0; i < U; i++)
            {
                for (int j = 0; j < U; j++)
                {
                    M[i, j] = _IFRc2020(i, j, c, d, U);

                }
            }
            return M;
        }

        public float[,][] MatrixC2021(List<float>[] C, List<string> d, float[,] FMd, int U)
        {
            float[,][] MC = initMB(U);
            M_c = new List<float[,][]>();
            float[,][] Mc = new float[U,U][];
            for (int c = 0; c <C.Length; c++)
            {
                float[,] FMc = FMatrixc(C[c], U);
                float[,] FMcd = FgiaoAB(FMc, FMd, U);
                float consi = _FLL(FMcd, U) / _FLL(FMd, U);
                float v = 1;
                if(C[c].Max()<=1) 
                    v=(float)Math.Round(_PS(C[c]),2);
                Mc=Matrixc2021(C[c],v,consi,d,U);
                M_c.Add(Mc);
                //B += "Ma trận dung sai của c:"+c+"\n" + showM(Mc) + "\n";
                MC = giaoAB(MC, Mc, U);
            }
            
            return MC;
        }

        public float[,][] MatrixC2020(List<float>[] C, List<string> d, int U)
        {
            float[,][] MC = initMB(U);
            M_c = new List<float[,][]>();
            float[,][] Mc = new float[U, U][];
            for (int c = 0; c < C.Length; c++)
            {
                Mc = Matrixc2020(C[c], d, U);
                M_c.Add(Mc);
                //B += "Ma trận dung sai của c:"+c+"\n" + showM(Mc) + "\n";
                MC = giaoAB(MC, Mc, U);
            }

            return MC;
        }
        public float[,][] Matrixd(List<string> d, int n)
        {
            float[,][] M = new float[n,n][];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = new float[2];
                    if (d[i] == d[j])
                    {
                        M[i, j][0] = 1;
                        M[i, j][1] = 0;
                    }
                    else
                    {
                        M[i, j][0] = 0;
                        M[i, j][1] = 1;
                    }

                }
            }
            return M;
        }
        
        private float Card(float[,][] A, int n)
        {
            float ll = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    ll += (1 + A[i, j][0] - A[i, j][1]) / 2;
                }
            }
            return ll;
        }

        float[] min_maxAB(float[] a, float[] b)
        {
            float[] min_max = new float[2];
            if (a[0] < b[0]) min_max[0]=a[0]; else min_max[0]=b[0];
            if (a[1] > b[1]) min_max[1] = a[1]; else min_max[1] = b[1];
            return min_max;
        }

        float[] max_minAB(float[] a, float[] b)
        {
            float[] max_min = new float[2];
            if (a[0] > b[0]) max_min[0] = a[0]; else max_min[0] = b[0];
            if (a[1] < b[1]) max_min[1] = a[1]; else max_min[1] = b[1];
            return max_min;
        }

        private float[,][] hopAB(float[,][] MA, float[,][] MB, int n)
        {
            float[,][] Mc = new float[n,n][];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Mc[i, j] = max_minAB(MA[i, j], MB[i, j]);
            }
            return Mc;
        }

        private float[,][] giaoAB(float[,][] MA, float[,][] MB, int n)
        {
            float[,][] Mc = new float[n, n][];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Mc[i, j] = min_maxAB(MA[i, j], MB[i, j]);
            }
            return Mc;
        }

        private float DisAB(float[,][] A, float[,][] B, int n)
        {
            float dis = 0;
            float LL_A = Card(A, n);
            float LL_B = Card(B, n);
            dis = (float)Math.Round((LL_A - LL_B) / (n*n),2);
            return dis;
        }

        public float[,] FMatrixd(List<string> d, int n)
        {
            float[,] M = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (d[i] == d[j]) M[i, j] = 1; else M[i, j] = 0;

                }
            }
            return M;
        }


        public float[,] FMatrixc(List<float> c, int n)
        {
            float[,] M = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = 1 - Math.Abs(c[i]- c[j]);

                }
            }
            return M;
        }

        float FminAB(float a, float b)
        {
            return Math.Min(a,b);

        }

        private float[,] FgiaoAB(float[,] MA, float[,] MB, int n)
        {
            float[,] Mc = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Mc[i, j] = FminAB(MA[i, j], MB[i, j]);
            }
            return Mc;
        }

        private float _FLL(float[,] Ma, int n)
        {
            float ll = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    ll += Ma[i, j];
                }
            }
            return ll;
        }

        private float[,][] initMB(int n)
        {
            float[,][] MB = new float[n,n][];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    MB[i, j] = new float[2];
                    MB[i, j][0] = 1;
                    MB[i, j][1] = 0;
                }
            }
            return MB;
        }
        private string showM(float[,][] M)
        {
            string stringM = "";
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    //stringM += "("+M[i, j]+")" + ";";
                    stringM += "(" + M[i, j][0] + "," + M[i, j][1] + ")" + ";";
                }
                stringM = stringM.Remove(stringM.Length - 1, 1);
                stringM += "\n";
            }
            return stringM;
        }


        private float[,][] initMT(int n)
        {
            float[,][] MT = new float[n, n][];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    MT[i, j] = new float[2];
                }
            }
            return MT;
        }

        private bool next(float[,][] B, float[,][] C,float epsi,int n)
        {
            float st = 0;
            float max_mu=0;
            float max_nu=0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                   if (Math.Abs(B[i, j][0] - C[i, j][0]) > max_mu)
                       max_mu = Math.Abs(B[i, j][1] - C[i, j][1]);
                   if (Math.Abs(B[i, j][1] - C[i, j][1]) > max_nu)
                       max_nu = Math.Abs(B[i, j][1] - C[i, j][1]);
                }
            }
            st = Math.Min(1-max_mu,1-max_nu);
            if (st< epsi)
                return true;
            return false;
        }
        public string IDS_F_DAR(int U, List<float>[] C, List<int> C_num, List<string> D)
        {
            //B += "Đầu vào: \n";
            string R = "";
            int _numR = 0;
            float[,] FMd = FMatrixd(D, U);
            //Khoi tao C,PS, Cnum

            float[,][] IFMC = MatrixC2021(C,D,FMd,U);

            //float[,][] IFMC = MatrixC2020(C, D, U);

            //B += "Ma trận dung sai của C:\n" + showM(MC) + "\n";
            float[,][] IFMd = Matrixd(D, U);
            float[,][] IFMCd = giaoAB(IFMC, IFMd, U);
            //B += "Ma trận dung sai của CD:\n" + showM(MCd) + "\n";
            //B += "Ma trận dung sai của D:\n" + showM(Md) + "\n";
            float[,][] IFMB = initMB(U);
            float[,][] IFMBd = giaoAB(IFMB, IFMd, U);
            //B += "Ma trận dung sai ban đầu của B:\n" + showM(IFMBd) + "\n";
            //float disKCD = DisK(KC(MC, n), KC(hopAB(MC, Md, n), n), n);
            float disCD = DisAB(IFMC,IFMCd,U);
            B += "Dis C va Cd: " + disCD+"\n";
            //float disKBD = DisK(KC(MB, n), KC(hopAB(MB, Md, n), n), n);
            float disBD = DisAB(IFMB, IFMBd, U);
            B += "Dis B va Bd: " + disBD + "\n";
            //B += "Bắt đầu vòng lặp\n";
            st0 = new Stopwatch();

            st0.Start();
            float eps = (float)0.1;
            float[,][] MBc = initMT(U);
            float[,][] MBcD = initMT(U);
            while ((disBD -disCD)>=0.01)
            { 
                float sigB =-1;
                int ida = -1;
                float disBcD = -1;
                float disBcDT = -1;
                float[,][] IFMBT = IFMB;
                foreach (int c in C_num)
                {

                    MBc = giaoAB(IFMB, M_c[c], U);
                    MBcD = giaoAB(MBc, IFMd, U);
                    disBcD = DisAB(MBc, MBcD, U);
                    //B += " " + (c+1) + " =" + disBD + " - " + disBcD + "= " + (disBD - disBcD) + "\n";
                    if ((disBD - disBcD) > sigB)
                    {
                        sigB = disBD -disBcD;
                        IFMBT = (float[,][])MBc.Clone();
                        ida = c;
                        disBcDT = disBcD;
                    }
                }
                if (ida > -1)
                {
                    R += "a" + (ida + 1) +",";
                    IFMB = IFMBT;
                    disBD = disBcDT;
                    C_num.Remove(ida);
                    _numR++;
                }
            }
            st0.Stop();
            tg = ((float.Parse((st0.ElapsedMilliseconds).ToString()))/1000).ToString();
            //B += "Finish:\n";
            //B += "Output:\n";
            B += "There are " + _numR + " attributes for reduct B: " + R;
            return B;
        }
    }
}
