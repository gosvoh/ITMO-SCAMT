using System;

namespace ML
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleApp.ModelBuilder.CreateModel("Path to .csv file for training model", "Path to .zip model for saving model");
        }
    }
}
