using System.Collections.Generic;
using System.Numerics;

namespace MathAndProcessing
{
    public interface IProcessing
    {
        List<List<Complex>> Decoder(List<double> rI, List<double> rQ, string startIndex);

        AOutputData GetOutputData();
    }
}