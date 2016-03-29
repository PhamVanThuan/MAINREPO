namespace DomainService2.SharedServices.Common
{
	public class CalculateAndSaveApplicationCommand : StandardDomainServiceCommand
    {
		public CalculateAndSaveApplicationCommand(int applicationKey, bool isBondExceptionAction)
        {
            this.ApplicationKey = applicationKey;
			this.IsBondExceptionAction = isBondExceptionAction;
        }

		public bool IsBondExceptionAction
		{
			get;
			protected set;
		}

        public int ApplicationKey
        {
            get;
            protected set;
        }
    }
}