namespace SAHL.DecisionTree.Shared
{
    public class SAHomeLoansBCCScorecard_1Variables
    {
        public SAHomeLoansBCCScorecard_1Inputs inputs = new SAHomeLoansBCCScorecard_1Inputs();
        public SAHomeLoansBCCScorecard_1Outputs outputs;

        private dynamic Enumerations;
        public SAHomeLoansBCCScorecard_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new SAHomeLoansBCCScorecard_1Outputs(Enumerations);
        }

        public class SAHomeLoansBCCScorecard_1Inputs
        {
            public int AT001 { get; set; }
public int AT039 { get; set; }
public int AT040 { get; set; }
public int AT162 { get; set; }
public int AT163 { get; set; }
public int GE010 { get; set; }
public int GE022 { get; set; }
public int GE030 { get; set; }
public int GE032 { get; set; }
public int GE044 { get; set; }
public int GE048 { get; set; }
public int GE051 { get; set; }
public int GE052 { get; set; }
public int GE053 { get; set; }
public int GE054 { get; set; }
public int GE056 { get; set; }
public int ML001 { get; set; }
public int ML039 { get; set; }
public int ML040 { get; set; }
public int ML162 { get; set; }
public int ML163 { get; set; }
public int DM001AL { get; set; }
public string DM003AL { get; set; }
public int EQ001AL { get; set; }
public int EQ001FC { get; set; }
public int EQ001FL { get; set; }
public int EQ001GA { get; set; }
public int EQ001GR { get; set; }
public int EQ001HW { get; set; }
public int EQ001JW { get; set; }
public int EQ001OT { get; set; }
public int EQ003FC { get; set; }
public int EQ003FL { get; set; }
public int EQ004GA { get; set; }
public int EQ004GR { get; set; }
public int EQ004HW { get; set; }
public int EQ004JW { get; set; }
public int EQ004OT { get; set; }
public int NG001AL { get; set; }
public int NG008AL { get; set; }
public int NG011AL { get; set; }
public int PP001AL { get; set; }
public int PP001CA { get; set; }
public int PP003AL { get; set; }
public int PP005AL { get; set; }
public int PP011AL { get; set; }
public int PP046MB { get; set; }
public int PP046VF { get; set; }
public int PP047FC { get; set; }
public int PP047FL { get; set; }
public int PP047NL { get; set; }
public int PP047PL { get; set; }
public int PP054AL { get; set; }
public int PP068CE { get; set; }
public int PP071AL { get; set; }
public int PP074AL { get; set; }
public int PP075CC { get; set; }
public int PP076CC { get; set; }
public int PP077CC { get; set; }
public int PP082CC { get; set; }
public int PP083CC { get; set; }
public int PP087CE { get; set; }
public int PP087NL { get; set; }
public int PP087PL { get; set; }
public int PP104VF { get; set; }

        }

        public class SAHomeLoansBCCScorecard_1Outputs
        {
            public int ScaledBCCScore { get; set; }
public string ToBeScored { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public SAHomeLoansBCCScorecard_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                ScaledBCCScore = (int)0;
ToBeScored = string.Empty;

            }
        }
    }
}