using System;

/// <summary>
/// Summary description for Constants
/// </summary>
public class Constants
{
	public Constants()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    static public string CURRENCY_FORMAT
    {
        get
        {
            return "R #,###,###,###,##0.00";
            
        }

    }

    static public string DATE_FORMAT
    {
        get
        {
            return "dd/MM/yyyy";

        }

    }

    public const int FIXED_LOAN = 2;
    public const int VARIABLE_LOAN = 1;
}
