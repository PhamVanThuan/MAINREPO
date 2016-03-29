using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;

namespace PDFWriting
{
    class Program
    {
        static void Main(string[] args)
        {
            DoPDF();
        }
        public static void DoPDF()
        {
            // lOAD THE TEMPLATE - fix path 
            PdfReader pdf = new PdfReader(@"c:\bla blab blaTemplate.pdf");
            try
            {
                // create the output pdf ... as it is it will be in the bin\debug folder
                using (FileStream fs = new FileStream(@".\New.pdf", FileMode.Create))
                {
                    PdfStamper pdfWriter = new PdfStamper(pdf, fs);
                    AcroFields fields = pdfWriter.AcroFields;

                    Hashtable htFields = pdf.AcroFields.Fields;
                    object[] Keys = new object[htFields.Keys.Count];
                    htFields.Keys.CopyTo(Keys, 0);
                    List<string> AllKeys = new List<string>();
                    for (int i = 0; i < Keys.Length; i++)
                    {
                        string Key = Keys[i].ToString().Remove(0, 27);
                        try
                        {
                            switch (Key)
                            {
                                // one of many named fields
                                case "_214_Married[0]":
                                    {
                                        // this will se the current field with whatever string value you want
                                        fields.SetField(Keys[i].ToString(), ""); 
                                        break;
                                    }
                                case "_22_ID_Number[0]":
                                    {

                                        break;
                                    }
                                case "Account_Number[0]":
                                    {

                                        break;
                                    }
                                case "ANC_COP[0]":
                                    {

                                        break;
                                    }
                                case "AUTHORISE_AND_INSTRUCT_THE_FINANCIAL_MANAGER_OF[0]":
                                    {

                                        break;
                                    }
                                case "Bank_Branch_Name[0]":
                                    {
                                        break;
                                    }
                                case "Bank_Name[0]":
                                    {
                                        break;
                                    }
                                case "Branch_Number[0]":
                                    {
                                        break;
                                    }
                                case "Cell_No[0]":
                                    {

                                        break;
                                    }
                                case "Code[0]":
                                    {

                                        break;
                                    }
                                case "COMMENCE_ON[0]":
                                    {
                                        fields.SetField(Keys[i].ToString(), ""); break;
                                    }
                                case "Date_Employed[0]":
                                    {

                                        break;
                                    }
                                case "Employee_Number[0]":
                                    {

                                        break;
                                    }
                                case "Employer_Address[0]": { break; }
                                case "Employer_Company[0]":
                                    {

                                        break;
                                    }
                                case "Employer_Post_Code[0]": { break; }
                                case "Employer_Tel_No[0]":
                                    {
                                        break;
                                    }
                                case "Employer_Unit[0]": { break; }
                                case "Home_Tel_No[0]":
                                    {

                                        break;
                                    }
                                case "Initiation_Fee[0]":
                                    {

                                        break;
                                    }
                                case "Interest_Over_Period[0]":
                                    {

                                        break;
                                    }
                                case "Loan_Amount[0]":
                                    {

                                        break;
                                    }
                                case "Monthly_Repayment[0]":
                                    {

                                        break;
                                    }
                                case "Monthly_Service_Fee_Incl_Vat[0]":
                                    {
                                        break;
                                    }
                                case "Next_of_Kin[0]":
                                    {

                                        break;
                                    }
                                case "Next_of_Kin_Cell_No[0]":
                                    {
                                        break;
                                    }
                                case "Next_of_Kin_Home_Tel_No[0]":
                                    {
                                        break;
                                    }
                                case "Next_of_Kin_Home_Tel_No_Code[0]": { break; }
                                case "Next_of_Kin_Physical_Address1[0]":
                                    {

                                        break;
                                    }
                                case "Next_of_Kin_Physical_Address2[0]":
                                    {

                                        break;
                                    }
                                case "Next_of_Kin_Postcode[0]":
                                    {
                                        break;
                                    }
                                case "Next_of_Kin_Relationship[0]":
                                    {
                                        break;
                                    }
                                case "Number_of_Installments[0]":
                                    {

                                        break;
                                    }
                                case "Physical_Address1[0]":
                                    {

                                        break;
                                    }
                                case "Physical_Address2[0]":
                                    {
                                        break;
                                    }
                                case "PostCode[0]":
                                    {
                                        break;
                                    }
                                case "Signed_at[0]": { fields.SetField(Keys[i].ToString(), ""); break; }
                                case "Signed_Date[0]": { fields.SetField(Keys[i].ToString(), ""); break; }
                                case "TCOC[0]":
                                    {
                                        break;
                                    }
                                case "THE_TOTAL_LOAN_OF_R1[0]":
                                    {
                                        break;
                                    }
                                case "THE_TOTAL_LOAN_OF_R2[0]":
                                    {
                                        fields.SetField(Keys[i].ToString(), "");

                                        break;
                                    }
                                case "TO_DEDUCT_INSTALMENTS_OF_R[0]":
                                    {
                                        break;
                                    }
                                case "Total_Amount_Repayable[0]":
                                    {

                                        break;
                                    }
                                case "Weekly_Repayment[0]": { break; }
                                case "Contact_details_of_preferred_method_of_delivery__By_E-mail[0]":
                                    {

                                        break;
                                    }
                                case "Contact_details_of_preferred_method_of_delivery__By_Fax[0]":
                                    {

                                        break;
                                    }
                                case "Debit_Order_Signed_At[0]": { fields.SetField(Keys[i].ToString(), ""); break; }
                                case "Debit_Order_Signed_day[0]": { fields.SetField(Keys[i].ToString(), ""); break; }
                                case "Debit_Order_Signed_Month[0]": { fields.SetField(Keys[i].ToString(), ""); break; }
                                case "Debit_Order_Signed_Year[0]": { fields.SetField(Keys[i].ToString(), ""); break; }
                                default:
                                    {
                                        fields.SetField(Keys[i].ToString(), Key); break;
                                    }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            throw;
                        }
                    }
                    // cant remember what this does
                    //pdfWriter.FormFlattening = true;
                    //close the file and flush it
                    pdfWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
