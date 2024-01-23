const int N = 1000; //Matrixgröße
const int THREADS_NUMBER = 10;

int[,] serialMulRes = new int[N, N]; //Das Ergebniss in einem Thread
int[,] threadMulRes = new int[N, N]; //Das Ergebniss Multithread

int[,] firstMatrix = MatrixGenerator(N, N);
int[,] secondMatrix = MatrixGenerator(N, N);

SerialMatrixMul(firstMatrix, secondMatrix);
PrepareParallelMatrixMul(firstMatrix, secondMatrix);
Console.WriteLine(EqualityMatrix(serialMulRes, threadMulRes));



int[,] MatrixGenerator(int rows, int columns)
{
    Random _rand = new Random(); //Randomzahlen
    int[,] res = new int[rows, columns];
    for (int i = 0; i < res.GetLength(0); i++)
    {
        for (int j = 0; j < res.GetLength(1); j++)
        {
            res[i, j] = _rand.Next(-100, 100); //Matrix von -100 bis +100
        }
    }
    return res;
}

void SerialMatrixMul(int[,] a, int[,] b)
{
    if (a.GetLength(1) != b.GetLength(0)) throw new Exception("" +
        "Solche Matrix können nicht multipliziert werden");

    for (int i = 0; i < a.GetLength(0); i++)
    {
        for (int j = 0; j < b.GetLength(1); j++)
        {
            for (int k = 0; k < b.GetLength(0); k++)
            {
                serialMulRes[i, j] += a[i, k] * b[k, j];
            }
        }
    }
}

void PrepareParallelMatrixMul(int[,] a, int[,] b)
{
    if (a.GetLength(1) != b.GetLength(0)) throw new Exception("" +
        "Solche Matrix können nicht multipliziert werden");
    int eachThreadCalc = N / THREADS_NUMBER;
    Thread[] arr = new Thread[2];
    var threadsList = new List<Thread>();
    for (int i = 0; i < THREADS_NUMBER; i++)
    {
        int startPos = i * eachThreadCalc;
        int endPos = (i + 1) * eachThreadCalc;
        //fuer letzte Thread
        if (i == THREADS_NUMBER - 1) endPos = N;
        threadsList.Add(new Thread(() => ParallelMatrixMul(a, b, startPos, endPos)));
        threadsList[i].Start();
    }
    for (int i = 0; i < THREADS_NUMBER; i++)
    {
        threadsList[i].Join();
    }
}

void ParallelMatrixMul(int[,] a, int[,] b, int startPos, int endPos)
{
    for (int i = startPos; i < endPos; i++)
    {
        for (int j = 0; j < b.GetLength(1); j++)
        {
            for (int k = 0; k < b.GetLength(0); k++)
            {
                threadMulRes[i, j] += a[i, k] * b[k, j];
            }
        }
    }
}

bool EqualityMatrix(int[,] fmatrix, int[,] smatrix)
{
    bool res = true;

    for (int i = 0; i < fmatrix.GetLength(0); i++)
    {
        for (int j = 0; j < fmatrix.GetLength(1); j++)
        {
            res = res && (fmatrix[i, j] == smatrix[i, j]);
        }
    }

    return res;
}



