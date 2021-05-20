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
    class F_FS
    {
        string B = "";
        List<List<string>> C, PS;
        private Stopwatch st0, st1, st2;
        public string tg = "";
        List<float[,]> M_c;


        //tinh phuong sai
        float _PS(List<float> c)
        {
            float _ps = 0;
            float _s = 0;
            for (int i = 0; i < c.Count; i++)
            {
                _s = _s + (float)Math.Pow(c[i] - c.Average(), 2);
            }
            _ps = _s / (c.Count - 1);
            return _ps;
        }

        private float S_Rc(int x, int y, List<float> c)
        {
            float r=0;
            if (c.Max() <= 1)
            {
                r = (1 - Math.Abs(c[x] - c[y]));
            }
            else
                if (c[x] == c[y]) r = 1; else r = 0;
            return r;
        }

        private float T_Rc(int x, int y, List<float> c, float sd)
        {
            float a= sd / (1 + sd);
            //float r = Math.Max(1 - Math.Abs(c[x] - c[y]) / sd, 0);
            //float r = 1-Math.Max(Math.Min((c[x] - c[y] + sd) / sd, (c[y] - c[x]+sd) / sd), 0)*sd;
            
            float r = 1 - Math.Abs(c[x] - c[y])*a;
            return r;
        }

        public float[,] S_Matrixc(List<float> c, int n)
        {
            float[,] M = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = S_Rc(i, j, c);
                }
            }
            return M;
        }

        public float[,] T_Matrixc(List<float> c, int n, float v)
        {
            float[,] M = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    M[i, j] = T_Rc(i, j, c,v);
                }
            }
            return M;
        }

        public float[,] S_MatrixC(List<float>[] C, int n)
        {
            float[,] MC = initMB(n);
            float[,] Mc = new float[n, n];
            M_c = new List<float[,]>();
            for (int c = 0; c < C.Length; c++)
            {
                Mc = S_Matrixc(C[c], n);
                M_c.Add(Mc);
                MC = giaoAB(MC, Mc, n);
            }

            return MC;
        }

        public float[,] T_MatrixC(List<float>[] C, int n)
        {
            float[,] MC = initMB(n);
            float[,] Mc = new float[n, n];
            M_c = new List<float[,]>();
            for (int c = 0; c < C.Length; c++)
            {
                float v = (float)Math.Sqrt(_PS(C[c]));
                Mc = T_Matrixc(C[c], n,v);
                M_c.Add(Mc);
                MC = giaoAB(MC, Mc, n);
            }

            return MC;
        }


        private float[,] Matrixd(List<string> d, int n)
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

        private float[] _LL1(float[,] Ma, int n)
        {
            float[] ll = new float[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    ll[i] += Ma[i, j];
                }
            }
            return ll;
        }

        private float _LL(float[,] Ma, int n)
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

        private float[,] giaoAB(float[,] MA, float[,] MB, int n)
        {
            float[,] Mc = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Mc[i, j] = Math.Min(MA[i, j], MB[i, j]);
            }
            return Mc;
        }

        private float DisAB(float[,] A, float[,] B, int n)
        {
            float dis = 0;
            float LL_A = _LL(A, n);
            float LL_B = _LL(B, n);
            dis = (float)Math.Round((LL_A - LL_B) / (n*n),2);
            return dis;
        }

        private float[,] initMB(int n)
        {
            float[,] MB = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    MB[i, j] = 1;
            }
            return MB;
        }
        private string showM(float[,] M)
        {
            string stringM = "";
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    stringM += M[i, j] + ",";
                }
                stringM = stringM.Remove(stringM.Length - 1, 1);
                stringM += "\n";
            }
            return stringM;
        }
      
        public string F_FDAR(int U, List<float>[] C, List<int> num_C, List<string> D)
        {
            B += "Đầu vào: \n";
            string R = "";
            int _numR = 0;
            //Khoi tao C,PS, Cnum
            float[,] MC = S_MatrixC(C, U);
            //B += "Ma trận dung sai của C:\n" + showM(MC) + "\n";
            float[,] Md = Matrixd(D, U);
            float[,] MCd = giaoAB(MC, Md, U);
            //B += "Ma trận dung sai của D:\n" + showM(Md) + "\n";
            float[,] MB = initMB(U);
            float[,] MBd = giaoAB(MB, Md, U);
            //B += "Ma trận dung sai ban đầu của B:\n" + showM(MB) + "\n";
            float disCD = DisAB(MC, MCd, U);
            B += "Dis C va Cd: " + disCD + "\n";
            float disBD = DisAB(MB, MBd, U);
            B += "Dis B va Bd: " + disBD + "\n";
            B += "Bắt đầu vòng lặp\n";
            st0 = new Stopwatch();
            st0.Start();
            float tgt = 0;
            float[,] MBc = initMB(U);
            float[,] MBcD = initMB(U);
            while (disBD > disCD)
            {
                float sigB = -1;
                float[,]MBT = MB;
                int ida = -1;
                float disBcD = -1;
                float disBcDT = -1;
                foreach (int c in num_C)
                {
                    MBc = giaoAB(MB, M_c[c], U);
                    MBcD = giaoAB(MBc, Md, U);
                    disBcD = DisAB(MBc, MBcD, U);
                    if ((disBD - disBcD) > sigB)
                    {
                        sigB = disBD - disBcD;
                        ida = c;
                        MBT = MBc;
                        disBcDT = disBcD;
                    }
                }
                if (ida > -1)
                {
                    R += "a" + (ida + 1) + ",";
                    MB = MBT;
                    disBD = disBcDT;
                    num_C.Remove(ida);
                    _numR++;
                }
            }
            st0.Stop();
            tg = ((float.Parse((st0.ElapsedMilliseconds).ToString()) - tgt) / 1000).ToString();
            B += "Kết thúc vòng lặp:\n";
            B += "Đầu ra:\n";
            B += "Có " + _numR + " thuộc tính quan trọng được lựa chọn cho tập thuộc tính rút gọn B là: " + R;
            return B;
        }
    }
}
