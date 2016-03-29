using System;

namespace SAHL.Core.Metrics
{
    internal static class SampleTypeExtension
    {
        internal static ISample NewSample(this SampleType sampleType)
        {
            switch (sampleType)
            {
                case SampleType.Uniform:
                    return new UniformSample(1028);

                case SampleType.Biased:
                    return new ExponentiallyDecayingSample(1028, 0.015);

                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}