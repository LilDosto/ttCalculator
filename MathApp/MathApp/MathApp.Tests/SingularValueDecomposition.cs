using System;
using System.Runtime.Serialization;

namespace DotNetMatrix
{
	[Serializable]
	public class SingularValueDecomposition : System.Runtime.Serialization.ISerializable
	{
		private decimal[][] U, V;
		private decimal[] s;
		private int m, n;
		
		public SingularValueDecomposition(GeneralMatrix Arg)
		{
			decimal[][] A = Arg.ArrayCopy;
			m = Arg.RowDimension;
			n = Arg.ColumnDimension;
			int nu = System.Math.Min(m, n);
			s = new decimal[System.Math.Min(m + 1, n)];
			U = new decimal[m][];
			for (int i = 0; i < m; i++)
			{
				U[i] = new decimal[nu];
			}
			V = new decimal[n][];
			for (int i2 = 0; i2 < n; i2++)
			{
				V[i2] = new decimal[n];
			}
			decimal[] e = new decimal[n];
			decimal[] work = new decimal[m];
			bool wantu = true;
			bool wantv = true;
			
			int nct = System.Math.Min(m - 1, n);
			int nrt = System.Math.Max(0, System.Math.Min(n - 2, m));
			for (int k = 0; k < System.Math.Max(nct, nrt); k++)
			{
				if (k < nct)
				{
					s[k] = 0;
					for (int i = k; i < m; i++)
					{
						s[k] = Maths.Hypot(s[k], A[i][k]);
					}
					if (s[k] != 0.0m)
					{
						if (A[k][k] < 0.0m)
						{
							s[k] = - s[k];
						}
						for (int i = k; i < m; i++)
						{
							A[i][k] /= s[k];
						}
						A[k][k] += 1.0m;
					}
					s[k] = - s[k];
				}
				for (int j = k + 1; j < n; j++)
				{
					if ((k < nct) & (s[k] != 0.0m))
					{
						decimal t = 0;
						for (int i = k; i < m; i++)
						{
							t += A[i][k] * A[i][j];
						}
						t = (- t) / A[k][k];
						for (int i = k; i < m; i++)
						{
							A[i][j] += t * A[i][k];
						}
					}
					e[j] = A[k][j];
				}
				if (wantu & (k < nct))
				{
					for (int i = k; i < m; i++)
					{
						U[i][k] = A[i][k];
					}
				}
				if (k < nrt)
				{
					e[k] = 0;
					for (int i = k + 1; i < n; i++)
					{
						e[k] = Maths.Hypot(e[k], e[i]);
					}
					if (e[k] != 0.0m)
					{
						if (e[k + 1] < 0.0m)
						{
							e[k] = - e[k];
						}
						for (int i = k + 1; i < n; i++)
						{
							e[i] /= e[k];
						}
						e[k + 1] += 1.0m;
					}
					e[k] = - e[k];
					if ((k + 1 < m) & (e[k] != 0.0m))
					{
						for (int i = k + 1; i < m; i++)
						{
							work[i] = 0.0m;
						}
						for (int j = k + 1; j < n; j++)
						{
							for (int i = k + 1; i < m; i++)
							{
								work[i] += e[j] * A[i][j];
							}
						}
						for (int j = k + 1; j < n; j++)
						{
							decimal t = (- e[j]) / e[k + 1];
							for (int i = k + 1; i < m; i++)
							{
								A[i][j] += t * work[i];
							}
						}
					}
					if (wantv)
					{
						for (int i = k + 1; i < n; i++)
						{
							V[i][k] = e[i];
						}
					}
				}
			}
			
			int p = System.Math.Min(n, m + 1);
			if (nct < n)
			{
				s[nct] = A[nct][nct];
			}
			if (m < p)
			{
				s[p - 1] = 0.0m;
			}
			if (nrt + 1 < p)
			{
				e[nrt] = A[nrt][p - 1];
			}
			e[p - 1] = 0.0m;
			
			if (wantu)
			{
				for (int j = nct; j < nu; j++)
				{
					for (int i = 0; i < m; i++)
					{
						U[i][j] = 0.0m;
					}
					U[j][j] = 1.0m;
				}
				for (int k = nct - 1; k >= 0; k--)
				{
					if (s[k] != 0.0m)
					{
						for (int j = k + 1; j < nu; j++)
						{
							decimal t = 0;
							for (int i = k; i < m; i++)
							{
								t += U[i][k] * U[i][j];
							}
							t = (- t) / U[k][k];
							for (int i = k; i < m; i++)
							{
								U[i][j] += t * U[i][k];
							}
						}
						for (int i = k; i < m; i++)
						{
							U[i][k] = - U[i][k];
						}
						U[k][k] = 1.0m + U[k][k];
						for (int i = 0; i < k - 1; i++)
						{
							U[i][k] = 0.0m;
						}
					}
					else
					{
						for (int i = 0; i < m; i++)
						{
							U[i][k] = 0.0m;
						}
						U[k][k] = 1.0m;
					}
				}
			}
			
			if (wantv)
			{
				for (int k = n - 1; k >= 0; k--)
				{
					if ((k < nrt) & (e[k] != 0.0m))
					{
						for (int j = k + 1; j < nu; j++)
						{
							decimal t = 0;
							for (int i = k + 1; i < n; i++)
							{
								t += V[i][k] * V[i][j];
							}
							t = (- t) / V[k + 1][k];
							for (int i = k + 1; i < n; i++)
							{
								V[i][j] += t * V[i][k];
							}
						}
					}
					for (int i = 0; i < n; i++)
					{
						V[i][k] = 0.0m;
					}
					V[k][k] = 1.0m;
				}
			}
			
			int pp = p - 1;
			int iter = 0;
			decimal eps = (decimal)Math.Pow(2.0f, - 52.0f);
			while (p > 0)
			{
				int k, kase;
				for (k = p - 2; k >= - 1; k--)
				{
					if (k == - 1)
					{
						break;
					}
					if (System.Math.Abs(e[k]) <= eps * (System.Math.Abs(s[k]) + System.Math.Abs(s[k + 1])))
					{
						e[k] = 0.0m;
						break;
					}
				}
				if (k == p - 2)
				{
					kase = 4;
				}
				else
				{
					int ks;
					for (ks = p - 1; ks >= k; ks--)
					{
						if (ks == k)
						{
							break;
						}
						decimal t = (ks != p?System.Math.Abs(e[ks]):0.0m) + (ks != k + 1?System.Math.Abs(e[ks - 1]):0.0m);
						if (System.Math.Abs(s[ks]) <= eps * t)
						{
							s[ks] = 0.0m;
							break;
						}
					}
					if (ks == k)
					{
						kase = 3;
					}
					else if (ks == p - 1)
					{
						kase = 1;
					}
					else
					{
						kase = 2;
						k = ks;
					}
				}
				k++;
				
				switch (kase)
				{
					case 1:  
					{
						decimal f = e[p - 2];
						e[p - 2] = 0.0m;
						for (int j = p - 2; j >= k; j--)
						{
							decimal t = Maths.Hypot(s[j], f);
							decimal cs = s[j] / t;
							decimal sn = f / t;
							s[j] = t;
							if (j != k)
							{
								f = (- sn) * e[j - 1];
								e[j - 1] = cs * e[j - 1];
							}
							if (wantv)
							{
								for (int i = 0; i < n; i++)
								{
									t = cs * V[i][j] + sn * V[i][p - 1];
									V[i][p - 1] = (- sn) * V[i][j] + cs * V[i][p - 1];
									V[i][j] = t;
								}
							}
						}
					}
					break;
					case 2:  
					{
						decimal f = e[k - 1];
						e[k - 1] = 0.0m;
						for (int j = k; j < p; j++)
						{
							decimal t = Maths.Hypot(s[j], f);
							decimal cs = s[j] / t;
							decimal sn = f / t;
							s[j] = t;
							f = (- sn) * e[j];
							e[j] = cs * e[j];
							if (wantu)
							{
								for (int i = 0; i < m; i++)
								{
									t = cs * U[i][j] + sn * U[i][k - 1];
									U[i][k - 1] = (- sn) * U[i][j] + cs * U[i][k - 1];
									U[i][j] = t;
								}
							}
						}
					}
					break;
					case 3:  
					{
						decimal scale = System.Math.Max(System.Math.Max(System.Math.Max(System.Math.Max(System.Math.Abs(s[p - 1]), System.Math.Abs(s[p - 2])), System.Math.Abs(e[p - 2])), System.Math.Abs(s[k])), System.Math.Abs(e[k]));
						decimal sp = s[p - 1] / scale;
						decimal spm1 = s[p - 2] / scale;
						decimal epm1 = e[p - 2] / scale;
						decimal sk = s[k] / scale;
						decimal ek = e[k] / scale;
						decimal b = ((spm1 + sp) * (spm1 - sp) + epm1 * epm1) / 2.0m;
						decimal c = (sp * epm1) * (sp * epm1);
						decimal shift = 0.0m;
						if ((b != 0.0m) | (c != 0.0m))
						{
							shift = (decimal)DecimalMath.Sqrt(b * b + c);
							if (b < 0.0m)
							{
								shift = - shift;
							}
							shift = c / (b + shift);
						}
						decimal f = (sk + sp) * (sk - sp) + shift;
						decimal g = sk * ek;
						
						for (int j = k; j < p - 1; j++)
						{
							decimal t = Maths.Hypot(f, g);
							decimal cs = f / t;
							decimal sn = g / t;
							if (j != k)
							{
								e[j - 1] = t;
							}
							f = cs * s[j] + sn * e[j];
							e[j] = cs * e[j] - sn * s[j];
							g = sn * s[j + 1];
							s[j + 1] = cs * s[j + 1];
							if (wantv)
							{
								for (int i = 0; i < n; i++)
								{
									t = cs * V[i][j] + sn * V[i][j + 1];
									V[i][j + 1] = (- sn) * V[i][j] + cs * V[i][j + 1];
									V[i][j] = t;
								}
							}
							t = Maths.Hypot(f, g);
							cs = f / t;
							sn = g / t;
							s[j] = t;
							f = cs * e[j] + sn * s[j + 1];
							s[j + 1] = (- sn) * e[j] + cs * s[j + 1];
							g = sn * e[j + 1];
							e[j + 1] = cs * e[j + 1];
							if (wantu && (j < m - 1))
							{
								for (int i = 0; i < m; i++)
								{
									t = cs * U[i][j] + sn * U[i][j + 1];
									U[i][j + 1] = (- sn) * U[i][j] + cs * U[i][j + 1];
									U[i][j] = t;
								}
							}
						}
						e[p - 2] = f;
						iter = iter + 1;
					}
					break;
					case 4:  
					{
						if (s[k] <= 0.0m)
						{
							s[k] = (s[k] < 0.0m?- s[k]:0.0m);
							if (wantv)
							{
								for (int i = 0; i <= pp; i++)
								{
									V[i][k] = - V[i][k];
								}
							}
						}
						while (k < pp)
						{
							if (s[k] >= s[k + 1])
							{
								break;
							}
							decimal t = s[k];
							s[k] = s[k + 1];
							s[k + 1] = t;
							if (wantv && (k < n - 1))
							{
								for (int i = 0; i < n; i++)
								{
									t = V[i][k + 1]; V[i][k + 1] = V[i][k]; V[i][k] = t;
								}
							}
							if (wantu && (k < m - 1))
							{
								for (int i = 0; i < m; i++)
								{
									t = U[i][k + 1]; U[i][k + 1] = U[i][k]; U[i][k] = t;
								}
							}
							k++;
						}
						iter = 0;
						p--;
					}
					break;
				}
			}
		}
		
		virtual public decimal[] SingularValues
		{
			get
			{
				return s;
			}
		}

		virtual public GeneralMatrix S
		{
			get
			{
				GeneralMatrix X = new GeneralMatrix(n, n);
				decimal[][] S = X.Array;
				for (int i = 0; i < n; i++)
				{
					for (int j = 0; j < n; j++)
					{
						S[i][j] = 0.0m;
					}
					S[i][i] = this.s[i];
				}
				return X;
			}
		}
		
		public virtual GeneralMatrix GetU()
		{
			return new GeneralMatrix(U, m, System.Math.Min(m + 1, n));
		}
		
		public virtual GeneralMatrix GetV()
		{
			return new GeneralMatrix(V, n, n);
		}
		
		public virtual decimal Norm2()
		{
			return s[0];
		}
		
		public virtual decimal Condition()
		{
			return s[0] / s[System.Math.Min(m, n) - 1];
		}
		
		public virtual int Rank()
		{
			decimal eps = (decimal)System.Math.Pow(2.0f, - 52.0f);
			decimal tol = System.Math.Max(m, n) * s[0] * eps;
			int r = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] > tol)
				{
					r++;
				}
			}
			return r;
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) 
		{
		}
	}
}
