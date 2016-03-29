namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface ILegalEntityNaturalPerson
    {
        /// <summary>
        ///
        /// </summary>
        int? AgeNextBirthday { get; }

        /// <summary>
        ///
        /// </summary>
        int? CurrentAge { get; }

        /// <summary>
        ///
        /// </summary>
        System.String IDNumber
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String PassportNumber
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IMaritalStatus MaritalStatus
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IGender Gender
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        ISalutation Salutation
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String FirstNames
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String Initials
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String Surname
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        ICitizenType CitizenType
        {
            get;
            set;
        }

        #region Methods

        ///// <summary>
        ///// Sets the IDNumber. Will automatically change the Citizen Type to RSA Citizen.
        ///// </summary>
        ///// <param name="IDNumber"></param>

        // Nazir J  2008-07-14 (Removed)
        //void SetIDNumber(string IDNumber);

        ///// <summary>
        ///// Sets the PassportNumber. Expects CitizenTypes of Foreigner or ForeignSAResident.
        ///// </summary>
        ///// <param name="PassportNumber"></param>
        ///// <param name="CitizenType"></param>
        // Nazir J  2008-07-14 (Removed)
        //void SetPassportNumber(string PassportNumber, CitizenTypes CitizenType);

        // Nazir J  2008-07-14 (Removed)
        // bool ValidateID(string IDNumber);

        #endregion Methods
    }
}