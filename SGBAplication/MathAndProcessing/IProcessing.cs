using System.Collections.Generic;

namespace MathAndProcessing
{
    public interface IProcessing
    {
        List<List<System.Numerics.Complex>> Decoder(List<double> rI, List<double> rQ, string startIndex);
        
    }
}