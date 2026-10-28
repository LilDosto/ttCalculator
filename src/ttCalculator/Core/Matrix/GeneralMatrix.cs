using System;
using System.Runtime.Serialization;

namespace DotNetMatrix
{
	internal class Maths 
	{
		public static decimal Hypot(decimal a, decimal b) 
		{
			decimal r;
			if (Math.Abs(a) > Math.Abs(b)) 
			{
				r = b/a;
				r = Math.Abs(a) * (decimal)DecimalMath.Sqrt(1 + r * r);
			} 
			else if (b != 0) 
			{
				r = a/b;
				r = Math.Abs(b) * (decimal)DecimalMath.Sqrt(1 + r * r);
			} 
			else 
			{
				r = 0.0m;
			}
			return r;
		}
	}
	
	[Serializable]
	public class GeneralMatrix : System.ICloneable, System.Runtime.Serialization.ISerializable, System.IDisposable
	{
		private decimal[][] A;
		private int m, n;
		
		public GeneralMatrix(int m, int n)
		{
			this.m = m;
			this.n = n;
			A = new decimal[m][];
			for (int i = 0; i < m; i++)
			{
				A[i] = new decimal[n];
			}
		}
		
		public GeneralMatrix(int m, int n, decimal s)
		{
			this.m = m;
			this.n = n;
			A = new decimal[m][];
			for (int i = 0; i < m; i++)
			{
				A[i] = new decimal[n];
				for (int j = 0; j < n; j++)
				{
					A[i][j] = s;
				}
			}
		}
		
		public GeneralMatrix(decimal[][] A)
		{
			m = A.Length;
			n = A[0].Length;
			for (int i = 0; i < m; i++)
			{
				if (A[i].Length != n)
				{
					throw new System.ArgumentException("All rows must have the same length.");
				}
			}
			this.A = A;
		}
		
		public GeneralMatrix(decimal[][] A, int m, int n)
		{
			this.A = A;
			this.m = m;
			this.n = n;
		}
		
		public GeneralMatrix(decimal[] vals, int m)
		{
			this.m = m;
			n = (m != 0?vals.Length / m:0);
			if (m * n != vals.Length)
			{
				throw new System.ArgumentException("Array length must be a multiple of m.");
			}
			A = new decimal[m][];
			for (int i = 0; i < m; i++)
			{
				A[i] = new decimal[n];
				for (int j = 0; j < n; j++)
				{
					A[i][j] = vals[i + j * m];
				}
			}
		}
		
		public virtual decimal[][] Array
		{
			get
			{
				return A;
			}
		}

		public virtual decimal[][] ArrayCopy
		{
			get
			{
				decimal[][] C = new decimal[m][];
				for (int i = 0; i < m; i++)
				{
					C[i] = new decimal[n];
					for (int j = 0; j < n; j++)
					{
						C[i][j] = A[i][j];
					}
				}
				return C;
			}
		}

		public virtual decimal[] ColumnPackedCopy
		{
			get
			{
				decimal[] vals = new decimal[m * n];
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < n; j++)
					{
						vals[i + j * m] = A[i][j];
					}
				}
				return vals;
			}
		}

		public virtual decimal[] RowPackedCopy
		{
			get
			{
				decimal[] vals = new decimal[m * n];
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < n; j++)
					{
						vals[i * n + j] = A[i][j];
					}
				}
				return vals;
			}
		}

		public virtual int RowDimension
		{
			get
			{
				return m;
			}
		}

		public virtual int ColumnDimension
		{
			get
			{
				return n;
			}
		}
		
		public static GeneralMatrix ConstructWithCopy(decimal[][] A)
		{
			int m = A.Length;
			int n = A[0].Length;
			GeneralMatrix X = new GeneralMatrix(m, n);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				if (A[i].Length != n)
				{
					throw new System.ArgumentException("All rows must have the same length.");
				}
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j];
				}
			}
			return X;
		}
		
		public virtual GeneralMatrix Copy()
		{
			GeneralMatrix X = new GeneralMatrix(m, n);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j];
				}
			}
			return X;
		}
		
		public virtual decimal GetElement(int i, int j)
		{
			return A[i][j];
		}
		
		public virtual GeneralMatrix GetMatrix(int i0, int i1, int j0, int j1)
		{
			GeneralMatrix X = new GeneralMatrix(i1 - i0 + 1, j1 - j0 + 1);
			decimal[][] B = X.Array;
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						B[i - i0][j - j0] = A[i][j];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		public virtual GeneralMatrix GetMatrix(int[] r, int[] c)
		{
			GeneralMatrix X = new GeneralMatrix(r.Length, c.Length);
			decimal[][] B = X.Array;
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						B[i][j] = A[r[i]][c[j]];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		public virtual GeneralMatrix GetMatrix(int i0, int i1, int[] c)
		{
			GeneralMatrix X = new GeneralMatrix(i1 - i0 + 1, c.Length);
			decimal[][] B = X.Array;
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						B[i - i0][j] = A[i][c[j]];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		public virtual GeneralMatrix GetMatrix(int[] r, int j0, int j1)
		{
			GeneralMatrix X = new GeneralMatrix(r.Length, j1 - j0 + 1);
			decimal[][] B = X.Array;
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						B[i][j - j0] = A[r[i]][j];
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
			return X;
		}
		
		public virtual void  SetElement(int i, int j, decimal s)
		{
			A[i][j] = s;
		}
		
		public virtual void  SetMatrix(int i0, int i1, int j0, int j1, GeneralMatrix X)
		{
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						A[i][j] = X.GetElement(i - i0, j - j0);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		public virtual void  SetMatrix(int[] r, int[] c, GeneralMatrix X)
		{
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						A[r[i]][c[j]] = X.GetElement(i, j);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		public virtual void  SetMatrix(int[] r, int j0, int j1, GeneralMatrix X)
		{
			try
			{
				for (int i = 0; i < r.Length; i++)
				{
					for (int j = j0; j <= j1; j++)
					{
						A[r[i]][j] = X.GetElement(i, j - j0);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		public virtual void  SetMatrix(int i0, int i1, int[] c, GeneralMatrix X)
		{
			try
			{
				for (int i = i0; i <= i1; i++)
				{
					for (int j = 0; j < c.Length; j++)
					{
						A[i][c[j]] = X.GetElement(i - i0, j);
					}
				}
			}
			catch (System.IndexOutOfRangeException e)
			{
				throw new System.IndexOutOfRangeException("Submatrix indices", e);
			}
		}
		
		public virtual GeneralMatrix Transpose()
		{
			GeneralMatrix X = new GeneralMatrix(n, m);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[j][i] = A[i][j];
				}
			}
			return X;
		}
		
		public virtual decimal Norm1()
		{
			decimal f = 0;
			for (int j = 0; j < n; j++)
			{
				decimal s = 0;
				for (int i = 0; i < m; i++)
				{
					s += System.Math.Abs(A[i][j]);
				}
				f = System.Math.Max(f, s);
			}
			return f;
		}
		
		public virtual decimal NormInf()
		{
			decimal f = 0;
			for (int i = 0; i < m; i++)
			{
				decimal s = 0;
				for (int j = 0; j < n; j++)
				{
					s += System.Math.Abs(A[i][j]);
				}
				f = System.Math.Max(f, s);
			}
			return f;
		}

		public virtual decimal NormF()
		{
			decimal f = 0;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					f = Maths.Hypot(f, A[i][j]);
				}
			}
			return f;
		}
		
		public virtual GeneralMatrix UnaryMinus()
		{
			GeneralMatrix X = new GeneralMatrix(m, n);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = - A[i][j];
				}
			}
			return X;
		}
		
		public virtual GeneralMatrix Add(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j] + B.A[i][j];
				}
			}
			return X;
		}
		
		public virtual GeneralMatrix AddEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = A[i][j] + B.A[i][j];
				}
			}
			return this;
		}
		
		public virtual GeneralMatrix Subtract(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j] - B.A[i][j];
				}
			}
			return X;
		}
		
		public virtual GeneralMatrix SubtractEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = A[i][j] - B.A[i][j];
				}
			}
			return this;
		}
		
		public virtual GeneralMatrix ArrayMultiply(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j] * B.A[i][j];
				}
			}
			return X;
		}
		
		public virtual GeneralMatrix ArrayMultiplyEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = A[i][j] * B.A[i][j];
				}
			}
			return this;
		}
		
		public virtual GeneralMatrix ArrayRightDivide(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = A[i][j] / B.A[i][j];
				}
			}
			return X;
		}
		
		public virtual GeneralMatrix ArrayRightDivideEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = A[i][j] / B.A[i][j];
				}
			}
			return this;
		}
		
		public virtual GeneralMatrix ArrayLeftDivide(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			GeneralMatrix X = new GeneralMatrix(m, n);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = B.A[i][j] / A[i][j];
				}
			}
			return X;
		}
		
		public virtual GeneralMatrix ArrayLeftDivideEquals(GeneralMatrix B)
		{
			CheckMatrixDimensions(B);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = B.A[i][j] / A[i][j];
				}
			}
			return this;
		}
		
		public virtual GeneralMatrix Multiply(decimal s)
		{
			GeneralMatrix X = new GeneralMatrix(m, n);
			decimal[][] C = X.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i][j] = s * A[i][j];
				}
			}
			return X;
		}
		
		public virtual GeneralMatrix MultiplyEquals(decimal s)
		{
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					A[i][j] = s * A[i][j];
				}
			}
			return this;
		}
		
		public virtual GeneralMatrix Multiply(GeneralMatrix B)
		{
			if (B.m != n)
			{
				throw new System.ArgumentException("GeneralMatrix inner dimensions must agree.");
			}
			GeneralMatrix X = new GeneralMatrix(m, B.n);
			decimal[][] C = X.Array;
			decimal[] Bcolj = new decimal[n];
			for (int j = 0; j < B.n; j++)
			{
				for (int k = 0; k < n; k++)
				{
					Bcolj[k] = B.A[k][j];
				}
				for (int i = 0; i < m; i++)
				{
					decimal[] Arowi = A[i];
					decimal s = 0;
					for (int k = 0; k < n; k++)
					{
						s += Arowi[k] * Bcolj[k];
					}
					C[i][j] = s;
				}
			}
			return X;
		}
		
		public static GeneralMatrix operator +(GeneralMatrix m1, GeneralMatrix m2) 
		{ 
			return m1.Add(m2); 
		} 
		
		public static GeneralMatrix operator -(GeneralMatrix m1, GeneralMatrix m2) 
		{ 
			return m1.Subtract(m2); 
		} 
		
		public static GeneralMatrix operator *(GeneralMatrix m1, GeneralMatrix m2) 
		{ 
			return m1.Multiply(m2); 
		} 
		
		public virtual LUDecomposition LUD()
		{
			return new LUDecomposition(this);
		}
		
		public virtual QRDecomposition QRD()
		{
			return new QRDecomposition(this);
		}
		
		public virtual CholeskyDecomposition chol()
		{
			return new CholeskyDecomposition(this);
		}
		
		public virtual SingularValueDecomposition SVD()
		{
			return new SingularValueDecomposition(this);
		}
		
		public virtual EigenvalueDecomposition Eigen()
		{
			return new EigenvalueDecomposition(this);
		}
		
		public virtual GeneralMatrix Solve(GeneralMatrix B)
		{
			return (m == n ? (new LUDecomposition(this)).Solve(B):(new QRDecomposition(this)).Solve(B));
		}
		
		public virtual GeneralMatrix SolveTranspose(GeneralMatrix B)
		{
			return Transpose().Solve(B.Transpose());
		}
		
		public virtual GeneralMatrix Inverse()
		{
			return Solve(Identity(m, m));
		}
		
		public virtual decimal Determinant()
		{
			return new LUDecomposition(this).Determinant();
		}
		
		public virtual int Rank()
		{
			return new SingularValueDecomposition(this).Rank();
		}
		
		public virtual decimal Condition()
		{
			return new SingularValueDecomposition(this).Condition();
		}
		
		public virtual decimal Trace()
		{
			decimal t = 0;
			for (int i = 0; i < System.Math.Min(m, n); i++)
			{
				t += A[i][i];
			}
			return t;
		}
		
		public static GeneralMatrix Identity(int m, int n)
		{
			GeneralMatrix A = new GeneralMatrix(m, n);
			decimal[][] X = A.Array;
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					X[i][j] = (i == j ? 1.0m : 0.0m);
				}
			}
			return A;
		}		
		
		private void  CheckMatrixDimensions(GeneralMatrix B)
		{
			if (B.m != m || B.n != n)
			{
				throw new System.ArgumentException("GeneralMatrix dimensions must agree.");
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
                GC.SuppressFinalize(this);
		}

		~GeneralMatrix()      
		{
			Dispose(false);
		}

		public System.Object Clone()
		{
			return this.Copy();
		}
		
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) 
		{
		}
	}
}
