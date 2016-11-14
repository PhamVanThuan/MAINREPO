namespace SAHL.DecisionTree.Shared
{
    public class OriginationNewBusinessSPVDetermination_1Variables
    {
        public OriginationNewBusinessSPVDetermination_1Inputs inputs = new OriginationNewBusinessSPVDetermination_1Inputs();
        public OriginationNewBusinessSPVDetermination_1Outputs outputs;

        private dynamic Enumerations;
        public OriginationNewBusinessSPVDetermination_1Variables(dynamic enumerations)
        {
            Enumerations = enumerations;
            outputs  = new OriginationNewBusinessSPVDetermination_1Outputs(Enumerations);
        }

        public class OriginationNewBusinessSPVDetermination_1Inputs
        {
            public bool IsCapitec { get; set; }
public bool IsAlpha { get; set; }
public bool IsDeveloper { get; set; }
public double LTV { get; set; }
public bool IsBlueBanner { get; set; }

        }

        public class OriginationNewBusinessSPVDetermination_1Outputs
        {
            public string BlueBannerAlpha { get; set; }
public string OldMutualAlpha { get; set; }
public string MainStreet65 { get; set; }
public string OldMutualDeveloper { get; set; }
public string Calibre { get; set; }
public string BlueBanner { get; set; }

            public bool NodeResult { get; set; }
            private dynamic Enumerations;

            public OriginationNewBusinessSPVDetermination_1Outputs(dynamic enumerations)
            {
                Enumerations = enumerations;
                BlueBannerAlpha = string.Empty;
OldMutualAlpha = string.Empty;
MainStreet65 = string.Empty;
OldMutualDeveloper = string.Empty;
Calibre = string.Empty;
BlueBanner = string.Empty;

            }
        }
    }
}